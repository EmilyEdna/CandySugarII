using CandySugar.Com.Controls.UIExtenControls;
using CandySugar.Com.Library;
using CandySugar.Com.Options.ComponentGeneric;
using Sdk.Component.Wallhav.sdk.ViewModel;
using Sdk.Component.Wallhav.sdk.ViewModel.Enums;
using Sdk.Component.Wallhav.sdk;
using Serilog;
using System.Windows;
using XExten.Advance.LinqFramework;
using CandySugar.Com.Options.ComponentObject;
using Sdk.Component.Plugins;
using Sdk.Component.Wallhav.sdk.ViewModel.Request;
using Sdk.Component.Wallhav.sdk.ViewModel.Response;
using System.Collections.ObjectModel;
using System.Windows.Data;
using CommunityToolkit.Mvvm.Input;
using CandySugar.Com.Library.FileDown;

namespace CandySugar.WallPaper.ViewModels
{
    public class WallhavViewModel : PropertyChangedBase
    {
        private object LockObject = new object();
        public WallhavViewModel()
        {
            GenericDelegate.SearchAction = new(SearchHandler);
            OnGeneralInit();
            var LocalDATA = DownUtil.ReadFile<List<WallhavSearchElementResult>>("Wallhaven", FileTypes.Dat, "WallPaper");
            CollectResult = new ObservableCollection<WallhavSearchElementResult>();
            if (LocalDATA != null)
            {
                LocalDATA.ForEach(CollectResult.Add);
            }
        }

        #region Field
        private string Keyword;
        private int GeneralTotal;
        private int GeneralPageIndex = 1;
        private int AnimeTotal;
        private int AnimePageIndex = 1;
        private int PeopleTotal;
        private int PeoplePageIndex = 1;
        /// <summary>
        /// 1：常规 2：动漫 3：人物 4：收藏
        /// </summary>
        private int ChangeType = 1;
        #endregion

        #region Property
        private ObservableCollection<WallhavSearchElementResult> _GeneralResult;
        public ObservableCollection<WallhavSearchElementResult> GeneralResult
        {
            get => _GeneralResult;
            set => SetAndNotify(ref _GeneralResult, value);
        }

        private ObservableCollection<WallhavSearchElementResult> _AnimeResult;
        public ObservableCollection<WallhavSearchElementResult> AnimeResult
        {
            get => _AnimeResult;
            set => SetAndNotify(ref _AnimeResult, value);
        }

        private ObservableCollection<WallhavSearchElementResult> _PeopleResult;
        public ObservableCollection<WallhavSearchElementResult> PeopleResult
        {
            get => _PeopleResult;
            set => SetAndNotify(ref _PeopleResult, value);
        }
        private ObservableCollection<WallhavSearchElementResult> _CollectResult;
        public ObservableCollection<WallhavSearchElementResult> CollectResult
        {
            get => _CollectResult;
            set => SetAndNotify(ref _CollectResult, value);
        }
        #endregion

        #region Method
        private void OnGeneralInit()
        {
            Task.Run(async () =>
            {
                try
                {
                    var result = (await WallhavFactory.Wall(opt =>
                    {
                        opt.RequestParam = new Input
                        {

                            CacheSpan = ComponentBinding.OptionObjectModels.Cache,
                            ImplType = SdkImpl.Rest,
                            WallhavType = WallhavEnum.Search,
                            Search = new WallhavSearch
                            {
                                KeyWord = this.Keyword,
                                PageIndex = 1,
                                QueryType = QueryEnum.General
                            }

                        };
                    }).RunsAsync()).SearchResult;
                    GeneralTotal = result.Total;
                    GeneralResult = new ObservableCollection<WallhavSearchElementResult>(result.ElementResult);
                    BindingOperations.EnableCollectionSynchronization(GeneralResult, LockObject);
                }
                catch (Exception ex)
                {
                    Log.Logger.Error(ex, "");
                    ErrorNotify();
                }
            });
        }
        private void OnAnimeInit()
        {
            Task.Run(async () =>
            {
                try
                {
                    var result = (await WallhavFactory.Wall(opt =>
                    {
                        opt.RequestParam = new Input
                        {

                            CacheSpan = ComponentBinding.OptionObjectModels.Cache,
                            ImplType = SdkImpl.Rest,
                            WallhavType = WallhavEnum.Search,
                            Search = new WallhavSearch
                            {
                                KeyWord = this.Keyword,
                                PageIndex = 1,
                                QueryType = QueryEnum.Anime
                            }

                        };
                    }).RunsAsync()).SearchResult;
                    AnimeTotal = result.Total;
                    AnimeResult = new ObservableCollection<WallhavSearchElementResult>(result.ElementResult);
                    BindingOperations.EnableCollectionSynchronization(AnimeResult, LockObject);
                }
                catch (Exception ex)
                {
                    Log.Logger.Error(ex, "");
                    ErrorNotify();
                }
            });
        }
        private void OnPeopleInit()
        {
            Task.Run(async () =>
            {
                try
                {
                    var result = (await WallhavFactory.Wall(opt =>
                    {
                        opt.RequestParam = new Input
                        {

                            CacheSpan = ComponentBinding.OptionObjectModels.Cache,
                            ImplType = SdkImpl.Rest,
                            WallhavType = WallhavEnum.Search,
                            Search = new WallhavSearch
                            {
                                KeyWord = this.Keyword,
                                PageIndex = 1,
                                QueryType = QueryEnum.People
                            }

                        };
                    }).RunsAsync()).SearchResult;
                    PeopleTotal = result.Total;
                    PeopleResult = new ObservableCollection<WallhavSearchElementResult>(result.ElementResult);
                    BindingOperations.EnableCollectionSynchronization(PeopleResult, LockObject);
                }
                catch (Exception ex)
                {
                    Log.Logger.Error(ex, "");
                    ErrorNotify();
                }
            });
        }

        private void OnLoadMoreGeneralInit()
        {
            Task.Run(async () =>
            {
                try
                {
                    var result = (await WallhavFactory.Wall(opt =>
                    {
                        opt.RequestParam = new Input
                        {

                            CacheSpan = ComponentBinding.OptionObjectModels.Cache,
                            ImplType = SdkImpl.Rest,
                            WallhavType = WallhavEnum.Search,
                            Search = new WallhavSearch
                            {
                                KeyWord = this.Keyword,
                                PageIndex = this.GeneralPageIndex,
                                QueryType = QueryEnum.General
                            }

                        };
                    }).RunsAsync()).SearchResult;
                    Application.Current.Dispatcher.Invoke(() => result.ElementResult.ForEach(GeneralResult.Add));
                }
                catch (Exception ex)
                {
                    Log.Logger.Error(ex, "");
                    ErrorNotify();
                }
            });
        }
        private void OnLoadMoreAnimeInit()
        {
            Task.Run(async () =>
            {
                try
                {
                    var result = (await WallhavFactory.Wall(opt =>
                    {
                        opt.RequestParam = new Input
                        {

                            CacheSpan = ComponentBinding.OptionObjectModels.Cache,
                            ImplType = SdkImpl.Rest,
                            WallhavType = WallhavEnum.Search,
                            Search = new WallhavSearch
                            {
                                KeyWord = this.Keyword,
                                PageIndex = this.AnimePageIndex,
                                QueryType = QueryEnum.Anime
                            }

                        };
                    }).RunsAsync()).SearchResult;
                    Application.Current.Dispatcher.Invoke(() => result.ElementResult.ForEach(AnimeResult.Add));
                }
                catch (Exception ex)
                {
                    Log.Logger.Error(ex, "");
                    ErrorNotify();
                }
            });
        }
        private void OnLoadMorePeopleInit()
        {
            Task.Run(async () =>
            {
                try
                {
                    var result = (await WallhavFactory.Wall(opt =>
                    {
                        opt.RequestParam = new Input
                        {

                            CacheSpan = ComponentBinding.OptionObjectModels.Cache,
                            ImplType = SdkImpl.Rest,
                            WallhavType = WallhavEnum.Search,
                            Search = new WallhavSearch
                            {
                                KeyWord = this.Keyword,
                                PageIndex = this.PeoplePageIndex,
                                QueryType = QueryEnum.People
                            }

                        };
                    }).RunsAsync()).SearchResult;
                    Application.Current.Dispatcher.Invoke(() => result.ElementResult.ForEach(PeopleResult.Add));
                }
                catch (Exception ex)
                {
                    Log.Logger.Error(ex, "");
                    ErrorNotify();
                }
            });
        }
        private void ErrorNotify(string Info = "")
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                new ScreenNotifyView(Info.IsNullOrEmpty() ? CommonHelper.ComponentErrorInformation : Info).Show();
            });
        }
        #endregion

        #region Command

        /// <summary>
        /// 加载更多
        /// </summary>
        public RelayCommand<ScrollChangedEventArgs> ScrollCommand => new((obj) =>
        {
            if (ChangeType == 1)
            {
                if (GeneralPageIndex <= GeneralTotal && obj.VerticalOffset + obj.ViewportHeight == obj.ExtentHeight && obj.VerticalChange > 0)
                {
                    GeneralPageIndex += 1;
                    OnLoadMoreGeneralInit();
                }
            }
            if (ChangeType == 2)
            {
                if (AnimePageIndex <= AnimeTotal && obj.VerticalOffset + obj.ViewportHeight == obj.ExtentHeight && obj.VerticalChange > 0)
                {
                    AnimePageIndex += 1;
                    OnLoadMoreAnimeInit();
                }
            }
            if (ChangeType == 3)
            {
                if (PeoplePageIndex <= PeopleTotal && obj.VerticalOffset + obj.ViewportHeight == obj.ExtentHeight && obj.VerticalChange > 0)
                {
                    PeoplePageIndex += 1;
                    OnLoadMorePeopleInit();
                }
            }
        });

        public void ChangeCommand(int type)
        {
            ChangeType = type;
            if (ChangeType == 1 && GeneralResult == null)
                OnGeneralInit();
            if (ChangeType == 2 && AnimeResult == null)
                OnAnimeInit();
            if (ChangeType == 3 && PeopleResult == null)
                OnPeopleInit();

        }

        public void CollectCommand(WallhavSearchElementResult element)
        {
            CollectResult.Add(element);
            CollectResult.ToList().DeleteAndCreate("Wallhaven", FileTypes.Dat, "WallPaper");
        }
        #endregion

        #region ExternalCalls
        /// <summary>
        /// 检索数据
        /// </summary>
        /// <param name="keyword"></param>
        private void SearchHandler(string keyword)
        {
            this.Keyword = keyword;
            GeneralPageIndex = AnimePageIndex = PeoplePageIndex = 1;
            if (ChangeType == 1)
                OnGeneralInit();
            if (ChangeType == 2)
                OnAnimeInit();
            if (ChangeType == 3)
                OnPeopleInit();
        }
        #endregion
    }
}
