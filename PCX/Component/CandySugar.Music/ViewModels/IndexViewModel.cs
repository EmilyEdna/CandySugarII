﻿using Sdk.Component.Music.sdk.ViewModel;
using Sdk.Component.Music.sdk;
using Sdk.Component.Music.sdk.ViewModel.Enums;
using Sdk.Component.Music.sdk.ViewModel.Request;
using Sdk.Component.Music.sdk.ViewModel.Response;
using System.Windows.Input;

namespace CandySugar.Music.ViewModels
{
    public class IndexViewModel : PropertyChangedBase
    {
        private object lockObject = new object();
        public IndexViewModel()
        {
            MenuIndex = new Dictionary<PlatformEnum, string> {
                { PlatformEnum.QQMusic,"QQ音乐"},
                { PlatformEnum.NeteaseMusic,"网易音乐"},
                { PlatformEnum.KuGouMusic,"酷狗音乐"},
                { PlatformEnum.KuWoMusic,"酷我音乐"},
                { PlatformEnum.MiGuMusic,"咪咕音乐"}
            };
            GenericDelegate.SearchAction = new(SearchHandler);
        }

        #region Field
        /// <summary>
        /// 1 单曲 2歌单 3列表
        /// </summary>
        private int HandleType = 1;
        private string SearchKeyword;
        private PlatformEnum Platform;
        /// <summary>
        /// 单曲页码
        /// </summary>
        private int SearchPageIndex = 1;
        /// <summary>
        /// 单曲总数
        /// </summary>
        private int SearchTotal;
        /// <summary>
        /// 歌单页码
        /// </summary>
        private int SheetPageIndex = 1;
        /// <summary>
        /// 歌单总数
        /// </summary>
        private int SheetTotal;
        /// <summary>
        /// 侧边栏开关状态 1 开 2关
        /// </summary>
        public int SliderStatus = 2;
        #endregion

        #region Property
        private Dictionary<PlatformEnum, string> _MenuIndex;
        /// <summary>
        /// 平台菜单
        /// </summary>
        public Dictionary<PlatformEnum, string> MenuIndex
        {
            get => _MenuIndex;
            set => SetAndNotify(ref _MenuIndex, value);
        }
        private ObservableCollection<MusicSongElementResult> _SingleResult;
        /// <summary>
        /// 单曲
        /// </summary>
        public ObservableCollection<MusicSongElementResult> SingleResult
        {
            get => _SingleResult;
            set => SetAndNotify(ref _SingleResult, value);
        }
        private ObservableCollection<MusicSheetElementResult> _SheetResult;
        /// <summary>
        /// 歌单
        /// </summary>
        public ObservableCollection<MusicSheetElementResult> SheetResult
        {
            get => _SheetResult;
            set => SetAndNotify(ref _SheetResult, value);
        }
        private ObservableCollection<MusicSongElementResult> _AlbumResult;
        /// <summary>
        /// 专辑
        /// </summary>
        public ObservableCollection<MusicSongElementResult> AlbumResult
        {
            get => _AlbumResult;
            set => SetAndNotify(ref _AlbumResult, value);
        }
        #endregion

        #region Command
        public void ChangeCommand(int key)
        {
            HandleType = key;
            if (SearchKeyword.IsNullOrEmpty())
            {
                new ScreenNotifyView(CommonHelper.SearckWordErrorInfomartion).Show();
                return;
            }
            if (SingleResult == null)
                OnInitSingle();
            if (SheetResult == null)
                OnInitSheet();
        }
        public void ActiveCommand(PlatformEnum platform)
        {
            Platform = platform;
            if (SearchKeyword.IsNullOrEmpty())
            {
                new ScreenNotifyView(CommonHelper.SearckWordErrorInfomartion).Show();
                return;
            }
            SearchPageIndex = SheetPageIndex = 1;
            if (HandleType == 1)
                OnInitSingle();
            if (HandleType == 2)
                OnInitSheet();
        }

        public void AlbumCommand(string albumId) 
        {
            OnInitAlbum(albumId);
        }

        public void DownCommand(string songId) 
        { 
        
        }

        public RelayCommand<ScrollChangedEventArgs> ScrollCommand => new((obj) =>
        {
            if (HandleType == 1)
                if (SearchPageIndex <= SearchTotal && obj.VerticalOffset + obj.ViewportHeight == obj.ExtentHeight && obj.VerticalChange > 0)
                {
                    SearchPageIndex += 1;
                    OnLoadMoreSingle();
                }
            if (HandleType == 2)
                if (SheetPageIndex <= SheetTotal && obj.VerticalOffset + obj.ViewportHeight == obj.ExtentHeight && obj.VerticalChange > 0)
                {
                    SheetPageIndex += 1;
                    OnLoadMoreSheet();
                }
        });
        #endregion

        #region Method
        private void OnInitAlbum(string albumId)
        {
            Task.Run(async () =>
            {
                try
                {
                    var result = (await MusicFactory.Music(opt =>
                    {
                        opt.RequestParam = new Input
                        {
                            PlatformType = Platform,
                            CacheSpan = ComponentBinding.OptionObjectModels.Cache,
                            ImplType = SdkImpl.Rest,
                            MusicType = MusicEnum.AlbumDetail,
                            AlbumDetail = new MusicAlbumDetail
                            {
                                AlbumId = albumId
                            }
                        };
                    }).RunsAsync()).AlbumResult;
                    AlbumResult = new ObservableCollection<MusicSongElementResult>(result.ElementResults);
                    // 这一句很关键，开启集合的异步访问支持
                    BindingOperations.EnableCollectionSynchronization(AlbumResult, lockObject);
                    WeakReferenceMessenger.Default.Send(new MessageNotify { SliderStatus = 1 });
                }
                catch (Exception ex)
                {
                    Log.Logger.Error(ex, "");
                    ErrorNotify();
                }
            });
        }
        /// <summary>
        /// 单曲查询
        /// </summary>
        private void OnInitSingle()
        {
            Task.Run(async () =>
            {
                try
                {
                    var result = (await MusicFactory.Music(opt =>
                     {
                         opt.RequestParam = new Input
                         {
                             PlatformType = Platform,
                             CacheSpan = ComponentBinding.OptionObjectModels.Cache,
                             ImplType = SdkImpl.Rest,
                             MusicType = MusicEnum.Song,
                             Search = new MusicSearch
                             {
                                 Page = 1,
                                 KeyWord = SearchKeyword
                             }
                         };
                     }).RunsAsync()).SongResult;
                    SearchTotal = result.Total ?? 0;
                    SingleResult = new ObservableCollection<MusicSongElementResult>(result.ElementResults);
                    // 这一句很关键，开启集合的异步访问支持
                    BindingOperations.EnableCollectionSynchronization(SingleResult, lockObject);
                }
                catch (Exception ex)
                {
                    Log.Logger.Error(ex, "");
                    ErrorNotify();
                }
            });
        }
        /// <summary>
        /// 加载更多单曲
        /// </summary>
        private void OnLoadMoreSingle()
        {
            Task.Run(async () =>
            {
                try
                {
                    var result = (await MusicFactory.Music(opt =>
                    {
                        opt.RequestParam = new Input
                        {
                            PlatformType = Platform,
                            CacheSpan = ComponentBinding.OptionObjectModels.Cache,
                            ImplType = SdkImpl.Rest,
                            MusicType = MusicEnum.Song,
                            Search = new MusicSearch
                            {
                                Page = SearchPageIndex,
                                KeyWord = SearchKeyword
                            }
                        };
                    }).RunsAsync()).SongResult;
                    lock (lockObject)
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            result.ElementResults.ForEach(item =>
                            {
                                SingleResult.Add(item);
                            });
                        });
                    }
                }
                catch (Exception ex)
                {
                    Log.Logger.Error(ex, "");
                    ErrorNotify();
                }
            });
        }
        /// <summary>
        /// 歌单查询
        /// </summary>
        private void OnInitSheet()
        {
            Task.Run(async () =>
            {
                try
                {
                    var result = (await MusicFactory.Music(opt =>
                    {
                        opt.RequestParam = new Input
                        {
                            PlatformType = Platform,
                            CacheSpan = ComponentBinding.OptionObjectModels.Cache,
                            ImplType = SdkImpl.Rest,
                            MusicType = MusicEnum.Sheet,
                            Search = new MusicSearch
                            {
                                Page = 1,
                                KeyWord = SearchKeyword
                            }
                        };
                    }).RunsAsync()).SheetResult;
                    SheetTotal = result.Total ?? 0;
                    SheetResult = new ObservableCollection<MusicSheetElementResult>(result.ElementResults);
                    // 这一句很关键，开启集合的异步访问支持
                    BindingOperations.EnableCollectionSynchronization(SheetResult, lockObject);
                }
                catch (Exception ex)
                {
                    Log.Logger.Error(ex, "");
                    ErrorNotify();
                }
            });
        }
        /// <summary>
        /// 加载更多歌单
        /// </summary>
        private void OnLoadMoreSheet()
        {
            Task.Run(async () =>
            {
                try
                {
                    var result = (await MusicFactory.Music(opt =>
                    {
                        opt.RequestParam = new Input
                        {
                            PlatformType = Platform,
                            CacheSpan = ComponentBinding.OptionObjectModels.Cache,
                            ImplType = SdkImpl.Rest,
                            MusicType = MusicEnum.Sheet,
                            Search = new MusicSearch
                            {
                                Page = SheetPageIndex,
                                KeyWord = SearchKeyword
                            }
                        };
                    }).RunsAsync()).SheetResult;
                    lock (lockObject)
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            result.ElementResults.ForEach(item =>
                            {
                                SheetResult.Add(item);
                            });
                        });
                    }
                }
                catch (Exception ex)
                {
                    Log.Logger.Error(ex, "");
                    ErrorNotify();
                }
            });
        }
        private void ErrorNotify()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                new ScreenNotifyView(CommonHelper.ComponentErrorInformation).Show();
            });
        }
        #endregion

        #region ExternalCalls
        /// <summary>
        /// 检索数据
        /// </summary>
        /// <param name="keyword"></param>
        private void SearchHandler(string keyword)
        {
            SearchKeyword = keyword;
            Platform = PlatformEnum.NeteaseMusic;
            SearchPageIndex = 1;
            OnInitSingle();
        }
        #endregion
    }
}