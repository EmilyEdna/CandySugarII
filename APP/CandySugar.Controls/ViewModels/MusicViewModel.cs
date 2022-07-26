using Sdk.Component.Music.sdk.ViewModel;
using Sdk.Component.Music.sdk.ViewModel.Response;
using Sdk.Component.Music.sdk;
using Sdk.Component.Music.sdk.ViewModel.Enums;
using Sdk.Component.Music.sdk.ViewModel.Request;

namespace CandySugar.Controls.ViewModels
{
    public class MusicViewModel : BaseViewModel
    {

        public MusicViewModel()
        {
            this.SongVisible = true;
            this.PlayVisible = false;
        }

        #region 字段
        PlatformEnum PlatformType = PlatformEnum.NeteaseMusic;
        int QueryType = 1;
        bool LoadMore = false;
        #endregion

        #region 命令
        public DelegateCommand QueryAction => new(() =>
        {
            this.Page = 1;
            this.LoadMore = false;
            SetRefresh(false);
            if (KeyWord.IsNullOrEmpty()) return;
            Common();
        });
        public DelegateCommand<string> HandleAction => new(input =>
        {
            this.Page = 1;
            this.LoadMore = false;
            PlatformType = (PlatformEnum)input.AsInt();
            SetRefresh(false);
            if (KeyWord.IsNullOrEmpty()) return;
            Common();
        });
        public DelegateCommand<string> TabAction => new(input =>
        {
            this.Page = 1;
            QueryType = input.AsInt();
            this.LoadMore = false;
            SetRefresh(false);
            if (KeyWord.IsNullOrEmpty()) return;
            Common();
        });
        public DelegateCommand LoadMoreSongAction => new(() =>
        {
            if (Lock) return;
            this.Page += 1;
            if (this.Page > Total) return;
            if (KeyWord.IsNullOrEmpty()) return;
            SetRefresh();
            this.LoadMore = true;
            Common();
        });
        public DelegateCommand LoadMorePlayListAction => new(() =>
        {
            if (Lock) return;
            this.Page += 1;
            if (this.Page > Total) return;
            if (KeyWord.IsNullOrEmpty()) return;
            SetRefresh();
            this.LoadMore = true;
            Common();
        });
        public DelegateCommand RefreshAction => new(() =>
        {
            this.Page = 1;
            this.LoadMore = false;
            SetRefresh(false);
            Common();
        });
        #endregion

        #region 属性
        ObservableCollection<MusicSongElementResult> _SongResult;
        public ObservableCollection<MusicSongElementResult> SongResult
        {
            get => _SongResult;
            set => SetProperty(ref _SongResult, value);
        }
        ObservableCollection<MusicSheetElementResult> _PlayListResult;
        public ObservableCollection<MusicSheetElementResult> PlayListResult
        {
            get => _PlayListResult;
            set => SetProperty(ref _PlayListResult, value);
        }

        bool _SongVisible;
        public bool SongVisible
        {
            get => _SongVisible;
            set=>SetProperty(ref _SongVisible, value);
        }
        bool _PlayVisible;
        public bool PlayVisible
        {
            get => _PlayVisible;
            set => SetProperty(ref _PlayVisible, value);
        }
        #endregion

        #region 方法 
        void Common() 
        {
            if (QueryType == 1)
            {
                this.SongVisible = true;
                this.PlayVisible = false;
                InitSong();
            }
            else
            {
                this.SongVisible = false;
                this.PlayVisible = true;
                InitPlayList();
            }
        }
        async void InitSong()
        {
            if (IsBusy) return;
            try
            {
                if (CandySoft.NetState.NetworkAccess != NetworkAccess.Internet)
                {
                    StaticResource.PopToast("请检查网络是否通畅!");
                    return;
                }
                ShowBusy();
                await Task.Delay(CandySoft.Wait);
                var result = await MusicFactory.Music(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        CacheSpan = CandySoft.Cache,
                        Proxy = StaticResource.Proxy(),
                        PlatformType = PlatformType,
                        ImplType = StaticResource.ImplType(),
                        MusicType = MusicEnum.Song,
                        Search = new MusicSearch
                        {
                            Page = this.Page,
                            KeyWord = KeyWord
                        }
                    };
                }).RunsAsync();
                CloseBusy();
                this.Total = result.SongResult.Total == null ? 0 : result.SongResult.Total.Value;
                if (LoadMore)
                    result.SongResult.ElementResults.ForEach(item =>
                    {
                        SongResult.Add(item);
                    });
                else
                    SongResult = new ObservableCollection<MusicSongElementResult>(result.SongResult.ElementResults);
            }
            catch (Exception ex)
            {
                StaticResource.PopToast(ex.Message);
            }
        }
        async void InitPlayList()
        {
            if (IsBusy) return;
            try
            {
                if (CandySoft.NetState.NetworkAccess != NetworkAccess.Internet)
                {
                    StaticResource.PopToast("请检查网络是否通畅!");
                    return;
                }
                ShowBusy();
                await Task.Delay(CandySoft.Wait);
                var result = await MusicFactory.Music(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        PlatformType = PlatformType,
                        CacheSpan = CandySoft.Cache,
                        Proxy = StaticResource.Proxy(),
                        ImplType = StaticResource.ImplType(),
                        MusicType = MusicEnum.Sheet,
                        Search = new MusicSearch
                        {
                            Page = this.Page,
                            KeyWord = KeyWord
                        }
                    };
                }).RunsAsync();
                CloseBusy();
                this.Total = result.SheetResult.Total == null ? 0 : result.SheetResult.Total.Value;
                if (LoadMore)
                    result.SheetResult.ElementResults.ForEach(item =>
                    {
                        PlayListResult.Add(item);
                    });
                else
                    PlayListResult = new ObservableCollection<MusicSheetElementResult>(result.SheetResult.ElementResults);
            }
            catch (Exception ex)
            {
                StaticResource.PopToast(ex.Message);
            }
        }
        #endregion
    }
}
