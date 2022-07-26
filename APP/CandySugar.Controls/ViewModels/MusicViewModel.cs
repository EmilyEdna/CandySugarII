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
            QueryType = 1;
            PlatformType = PlatformEnum.NeteaseMusic;
        }

        #region 字段
        PlatformEnum PlatformType;
        int QueryType;
        #endregion

        #region 命令
        public DelegateCommand QueryAction => new(() =>
        {
            this.Page = 1;
            SetRefresh(false);
            if (QueryType == 1) InitSong(); else InitPlayList();
        });
        public DelegateCommand<string> HandleAction => new(input =>
        {
            this.Page = 1;
            PlatformType = (PlatformEnum)input.AsInt();
            SetRefresh(false);
            if (KeyWord.IsNullOrEmpty()) return;
            if (QueryType == 1) InitSong(); else InitPlayList();
        });
        public DelegateCommand<string> TabAction => new(input =>
        {
            this.Page = 1;
            QueryType = input.AsInt();
            SetRefresh(false);
            if (KeyWord.IsNullOrEmpty()) return;
            if (QueryType == 1) InitSong(); else InitPlayList();
        });
        public DelegateCommand LoadMoreSongAction => new(() =>
        {
            SetRefresh();
        });
        public DelegateCommand LoadMorePlayListAction => new(() =>
        {
            SetRefresh();
        });
        public DelegateCommand RefreshAction => new(() =>
        {
            this.Page = 1;
            SetRefresh(false);
            if (QueryType == 1) InitSong(); else InitPlayList();
        });
        #endregion

        #region 属性
        ObservableCollection<MusicSongElementResult> _SongResult;
        public ObservableCollection<MusicSongElementResult> SongResult
        {
            get => _SongResult;
            set => SetProperty(ref _SongResult, value);
        }
        #endregion

        #region 方法
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
                this.Total = result.SongResult.Total==null?0: result.SongResult.Total.Value;
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

            }
            catch (Exception ex)
            {
                StaticResource.PopToast(ex.Message);
            }
        }
        #endregion
    }
}
