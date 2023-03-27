using CandySugar.Com.Controls.UIExtenControls;
using Sdk.Component.Lovel.sdk;
using Sdk.Core;
using Serilog;
using Stylet;
using System;
using Sdk.Component.Lovel.sdk.ViewModel;
using Sdk.Component.Lovel.sdk.ViewModel.Enums;
using Sdk.Component.Lovel.sdk.ViewModel.Request;
using Sdk.Component.Lovel.sdk.ViewModel.Response;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Sdk.Component.Plugins;
using CandySugar.Com.Options.ComponentObject;
using System.Collections.ObjectModel;
using CandySugar.Com.Library;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Controls;
using System.Collections;
using CandySugar.Com.Library.VisualTree;
using System.Windows.Data;
using XExten.Advance.LinqFramework;
using System.Runtime.InteropServices;

namespace CandySugar.LightNovel.ViewModels
{
    public class IndexViewModel : PropertyChangedBase
    {

        private object lockObject = new object();
        public IndexViewModel()
        {
            SdkLicense.Register(new SdkLicenseModel
            {
                Account = "EmilyEdna",
                Password = DateTime.Now.ToString("yyyyMMdd")
            });
            OnInit();

        }

        #region Field
        private int InfomationTotal;
        private string InfomationRoute;
        private int InfomationPageIndex = 1;
        private int SearchTotal;
        private string SearchKeyword;
        private int SearchPageIndex = 1;
        /// <summary>
        /// 操作类型 1:分类 2:查询
        /// </summary>
        private int HandleType = 1;
        #endregion

        #region Property
        /// <summary>
        /// 分类菜单
        /// </summary>
        private ObservableCollection<LovelInitResult> _MenuIndex;
        public ObservableCollection<LovelInitResult> MenuIndex
        {
            get => _MenuIndex;
            set => SetAndNotify(ref _MenuIndex, value);
        }
        private ObservableCollection<LovelCategoryElementResult> _InformationElement;
        /// <summary>
        /// 分类结果
        /// </summary>
        public ObservableCollection<LovelCategoryElementResult> InformationElement
        {
            get => _InformationElement;
            set => SetAndNotify(ref _InformationElement, value);
        }
        #endregion

        #region Command
        public void ActiveCommand(string route)
        {
            HandleType = 1;
            InfomationPageIndex = 1;
            InfomationRoute = route;
            OnInitInformation();
        }
        public RelayCommand<ScrollChangedEventArgs> ScrollCommand => new((obj) =>
        {
            if (HandleType == 1)
                if (InfomationPageIndex <= InfomationTotal && obj.VerticalOffset + obj.ViewportHeight == obj.ExtentHeight && obj.VerticalChange > 0)
                {
                    InfomationPageIndex += 1;
                    OnLoadMoreInfomation();
                }
            if (HandleType == 2)
                if (SearchPageIndex <= SearchTotal && obj.VerticalOffset + obj.ViewportHeight == obj.ExtentHeight && obj.VerticalChange > 0)
                {
                    SearchPageIndex += 1;
                    OnLoadMoreSearch();
                }
        });
        #endregion

        #region Method
        /// <summary>
        /// 初始化
        /// </summary>
        private void OnInit()
        {
            Task.Run(async () =>
            {
                try
                {
                    var result = (await LovelFactory.Lovel(opt =>
                    {
                        opt.RequestParam = new Input
                        {
                            CacheSpan = ComponentBinding.OptionObjectModels.Cache,
                            ImplType = SdkImpl.Rest,
                            LovelType = LovelEnum.Init,
                            Login = new()
                        };
                    }).RunsAsync()).InitResults;
                    MenuIndex = new ObservableCollection<LovelInitResult>(result);
                }
                catch (Exception ex)
                {
                    Log.Logger.Error(ex, "");
                    new ScreenNotifyView(CommonHelper.ComponentErrorInformation).Show();
                }
            });
        }
        /// <summary>
        /// 初始化分类
        /// </summary>
        /// <param name="route"></param>
        private void OnInitInformation()
        {
            Task.Run(async () =>
            {
                try
                {
                    var result = (await LovelFactory.Lovel(opt =>
                    {
                        opt.RequestParam = new Input
                        {
                            CacheSpan = ComponentBinding.OptionObjectModels.Cache,
                            ImplType = SdkImpl.Rest,
                            LovelType = LovelEnum.Category,
                            Category = new LovelCategory
                            {
                                Page = 1,
                                Route = InfomationRoute
                            }
                        };
                    }).RunsAsync()).CategoryResult;
                    InfomationTotal = result.Total;
                    InformationElement = new ObservableCollection<LovelCategoryElementResult>(result.ElementResults);
                    // 这一句很关键，开启集合的异步访问支持
                    BindingOperations.EnableCollectionSynchronization(InformationElement, lockObject);
                }
                catch (Exception ex)
                {
                    Log.Logger.Error(ex, "");
                    new ScreenNotifyView(CommonHelper.ComponentErrorInformation).Show();
                }
            });
        }
        /// <summary>
        /// 加载更多分类
        /// </summary>
        private void OnLoadMoreInfomation()
        {
            Task.Run(async () =>
            {
                try
                {
                    var result = (await LovelFactory.Lovel(opt =>
                    {
                        opt.RequestParam = new Input
                        {
                            CacheSpan = ComponentBinding.OptionObjectModels.Cache,
                            ImplType = SdkImpl.Rest,
                            LovelType = LovelEnum.Category,
                            Category = new LovelCategory
                            {
                                Page = InfomationPageIndex,
                                Route = InfomationRoute
                            }
                        };
                    }).RunsAsync()).CategoryResult;
                    lock (lockObject)
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            result.ElementResults.ForEach(index =>
                            {
                                InformationElement.Add(index);
                            });
                        });
                    }
                }
                catch (Exception ex)
                {
                    Log.Logger.Error(ex, "");
                    new ScreenNotifyView(CommonHelper.ComponentErrorInformation).Show();
                }
            });
        }
        /// <summary>
        /// 初始化搜索
        /// </summary>
        private void OnInitSearch()
        {
            Task.Run(async () =>
            {
                try
                {
                    var result = (await LovelFactory.Lovel(opt =>
                    {
                        opt.RequestParam = new Input
                        {
                            CacheSpan = ComponentBinding.OptionObjectModels.Cache,
                            ImplType = SdkImpl.Rest,
                            LovelType = LovelEnum.Search,
                            Search = new LovelSearch
                            {
                                Page = 1,
                                SearchType = LovelSearchEnum.ArticleName,
                                KeyWord = SearchKeyword
                            }
                        };
                    }).RunsAsync()).SearchResult;
                    SearchTotal = result.Total;
                    InformationElement = new ObservableCollection<LovelCategoryElementResult>(result.ElementResults.ToMapest<List<LovelCategoryElementResult>>());
                    // 这一句很关键，开启集合的异步访问支持
                    BindingOperations.EnableCollectionSynchronization(InformationElement, lockObject);
                }
                catch (Exception ex)
                {
                    Log.Logger.Error(ex, "");
                    new ScreenNotifyView(CommonHelper.ComponentErrorInformation).Show();
                }
            });
        }
        /// <summary>
        /// 加载更多搜索
        /// </summary>
        private void OnLoadMoreSearch()
        {
            Task.Run(async () =>
            {
                try
                {
                    var result = (await LovelFactory.Lovel(opt =>
                    {
                        opt.RequestParam = new Input
                        {
                            CacheSpan = ComponentBinding.OptionObjectModels.Cache,
                            ImplType = SdkImpl.Rest,
                            LovelType = LovelEnum.Search,
                            Search = new LovelSearch
                            {
                                Page = SearchPageIndex,
                                SearchType = LovelSearchEnum.ArticleName,
                                KeyWord = SearchKeyword
                            }
                        };
                    }).RunsAsync()).SearchResult;
                    lock (lockObject)
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            result.ElementResults.ForEach(index =>
                            {
                                InformationElement.Add(index.ToMapest<LovelCategoryElementResult>());
                            });
                        });
                    }
                }
                catch (Exception ex)
                {
                    Log.Logger.Error(ex, "");
                    new ScreenNotifyView(CommonHelper.ComponentErrorInformation).Show();
                }
            });
        }
        #endregion

        #region ExternalCalls
        /// <summary>
        /// 检索数据
        /// </summary>
        /// <param name="keyword"></param>
        public void SearchHandler(string keyword)
        {
            HandleType = 2;
            SearchPageIndex = 1;
            SearchKeyword = keyword;
            OnInitSearch();
        }
        #endregion
    }
}
