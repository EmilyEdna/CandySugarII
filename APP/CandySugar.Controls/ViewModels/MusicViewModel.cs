using Sdk.Component.Music.sdk.ViewModel;
using Sdk.Component.Music.sdk.ViewModel.Response;
using Sdk.Component.Music.sdk;
using Sdk.Component.Music.sdk.ViewModel.Enums;
using Sdk.Component.Music.sdk.ViewModel.Request;
using CandySugar.Controls.Views.MusicViews;
using XExten.Advance.HttpFramework.MultiFactory;

namespace CandySugar.Controls.ViewModels
{
    public class MusicViewModel : BaseViewModel
    {
        public MusicViewModel()
        {
            CandyService = CandyContainer.Instance.Resolve<ICandyService>();
            this.SongVisible = true;
            this.PlayVisible = false;
        }

        #region 字段
        PlatformEnum PlatformType = PlatformEnum.NeteaseMusic;
        int QueryType = 1;
        bool LoadMore = false;
        ICandyService CandyService;
        #endregion

        #region 命令
        public DelegateCommand<MusicSongElementResult> AddPlayAction => new(input =>
        {
            Task.Run(() => InitDown(input));
        });
        public DelegateCommand QueryAction => new(() =>
        {
            this.Page = 1;
            this.LoadMore = false;
            SetRefresh(false);
            if (KeyWord.IsNullOrEmpty()) return;
            CommonAsync();
            MessagingCenter.Send(this, "QueryStart", true);
        });
        public DelegateCommand<string> HandleAction => new(input =>
        {
            this.Page = 1;
            this.LoadMore = false;
            PlatformType = (PlatformEnum)input.AsInt();
            SetRefresh(false);
            if (KeyWord.IsNullOrEmpty()) return;
            CommonAsync();
        });
        public DelegateCommand<string> TabAction => new(input =>
        {
            this.Page = 1;
            QueryType = input.AsInt();
            this.LoadMore = false;
            SetRefresh();
            if (KeyWord.IsNullOrEmpty()) return;
            CommonAsync();
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
            if (KeyWord.IsNullOrEmpty()) return;
            SetRefresh(false);
            CommonAsync();
        });
        public DelegateCommand<MusicSheetElementResult> DetailAction => new(input =>
        {
            InitPlayListDetail(input.SongSheetId.ToString());
        });
        public DelegateCommand<MusicSongElementResult> AlbumAction => new(input =>
        {
            InitAlbum(input.SongAlbumId);
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
            set => SetProperty(ref _SongVisible, value);
        }
        bool _PlayVisible;
        public bool PlayVisible
        {
            get => _PlayVisible;
            set => SetProperty(ref _PlayVisible, value);
        }
        #endregion

        #region 方法 
        async void NavigationToAlbum(List<MusicSongElementResult> input)
        {
            await Shell.Current.GoToAsync(nameof(MusicAlbumView), new Dictionary<string, object> { { "Data", input },{"Key", (int)PlatformType } });
        }
        async void NavigationToDetail(MusicSheetDetailRootResult input, string playId)
        {
            await Shell.Current.GoToAsync(nameof(MusicDetailView), new Dictionary<string, object> { { "Data", input }, { "Key", playId } });
        }
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
        void CommonAsync()
        {
            if (QueryType == 1)
            {
                this.SongVisible = true;
                this.PlayVisible = false;
                Task.Run(() => InitSong());
            }
            else
            {
                this.SongVisible = false;
                this.PlayVisible = true;
                Task.Run(() => InitPlayList());
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
        async void InitAlbum(string input)
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
                        MusicType = MusicEnum.AlbumDetail,
                        AlbumDetail = new MusicAlbumDetail
                        {
                            AlbumId = input
                        }
                    };
                }).RunsAsync();
                CloseBusy();
                NavigationToAlbum(result.AlbumResult.ElementResults);
            }
            catch (Exception ex)
            {
                StaticResource.PopToast(ex.Message);
            }
        }
        async void InitPlayListDetail(string input)
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
                        MusicType = MusicEnum.SheetDetail,
                        SheetDetail = new MusicSheetDetail
                        {
                            Id = input
                        }
                    };
                }).RunsAsync();
                CloseBusy();
                NavigationToDetail(result.SheetDetailResult, input);
            }
            catch (Exception ex)
            {
                StaticResource.PopToast(ex.Message);
            }
        }
        async void InitDown(MusicSongElementResult input)
        {
            if (CandyService.GetMusic().FirstOrDefault(t => t.SongId == input.SongId) == null)
            {
                var result = await MusicFactory.Music(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        CacheSpan = CandySoft.Cache,
                        Proxy = StaticResource.Proxy(),
                        PlatformType = PlatformType,
                        ImplType = StaticResource.ImplType(),
                        MusicType = MusicEnum.Route,
                        Play = PlatformType == PlatformEnum.KuGouMusic ? new MusicPlaySearch
                        {
                            Dynamic = input.SongId,
                            KuGouAlbumId = input.SongAlbumId,
                        } : new MusicPlaySearch
                        {
                            Dynamic = input.SongId,
                        }
                    };
                }).RunsAsync();

                if (result.PlayResult.CanPlay == false)
                {
                    StaticResource.PopToast("当前歌曲已下架");
                    return;
                }

                var SongFile = $"{input.SongName}({input.SongAlbumName})-{string.Join(",", input.SongArtistName)}_{PlatformType}.mp3";
                var Directory = SyncStatic.CreateDir(Path.Combine(ICrossExtension.Instance.AndriodPath, "CandyDown", "Music"));
                var Files = SyncStatic.CreateFile(Path.Combine(Directory, SongFile));

                if (this.PlatformType == PlatformEnum.BiliBiliMusic)
                {
                    var CacheAddress = SyncStatic.WriteFile(result.PlayResult.BilibiliFileBytes, Files);
                }
                else
                {
                    var filebytes = IHttpMultiClient.HttpMulti.AddNode(opt => opt.NodePath = result.PlayResult.SongURL).Build().RunBytesFirst();
                    var CacheAddress = SyncStatic.WriteFile(filebytes, Files);
                }

                CandyService.AddMusic(new CandyMusic
                {
                    AlbumId = input.SongAlbumId,
                    AlbumName = input.SongAlbumName,
                    SongArtist = String.Join(",", input.SongArtistName),
                    IsComplete = true,
                    LocalRoute = Files,
                    NetRoute = result.PlayResult.SongURL,
                    SongId = input.SongId,
                    Platform = (int)PlatformType,
                    SongName = input.SongName
                });
            }
        }
        #endregion
    }
}
