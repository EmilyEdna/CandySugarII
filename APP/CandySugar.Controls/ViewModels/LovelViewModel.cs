﻿using CandySugar.Controls.Views.LovelViews;
using Microsoft.Maui.Storage;
using Sdk.Component.Lovel.sdk;
using Sdk.Component.Lovel.sdk.ViewModel;
using Sdk.Component.Lovel.sdk.ViewModel.Enums;
using Sdk.Component.Lovel.sdk.ViewModel.Request;
using Sdk.Component.Lovel.sdk.ViewModel.Response;
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace CandySugar.Controls.ViewModels
{
    public class LovelViewModel : BaseViewModel
    {
        public LovelViewModel()
        {
            Task.Run(() => InitLovel());
        }
        #region 字段
        string CategoryRoute = string.Empty;
        bool LoadMore = false;
        #endregion

        #region 属性
        /// <summary>
        /// 类别
        /// </summary>
        ObservableCollection<LovelInitResult> _InitResult;
        public ObservableCollection<LovelInitResult> InitResult
        {
            get => _InitResult;
            set => SetProperty(ref _InitResult, value);
        }
        /// <summary>
        /// 分类结果
        /// </summary>
        ObservableCollection<LovelCategoryElementResult> _CategoryResult;

        public ObservableCollection<LovelCategoryElementResult> CategoryResult
        {
            get => _CategoryResult;
            set => SetProperty(ref _CategoryResult, value);
        }
        ObservableCollection<LovelSearchElementResult> _QueryResult;
        public ObservableCollection<LovelSearchElementResult> QueryResult
        {
            get => _QueryResult;
            set => SetProperty(ref _QueryResult, value);
        }
        #endregion

        #region 方法
        async void InitLovel()
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
                var result = await LovelFactory.Lovel(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        CacheSpan = CandySoft.Cache,
                        Proxy = StaticResource.Proxy(),
                        ImplType = StaticResource.ImplType(),
                        LovelType = LovelEnum.Init,
                        Login = new LovelLogin
                        {
                            Account = CandySoft.LightAccount,
                            Password = CandySoft.LightPwd
                        }
                    };
                }).RunsAsync();
                CloseBusy();
                InitResult = new ObservableCollection<LovelInitResult>(result.InitResults);
            }
            catch (Exception ex)
            {
                StaticResource.PopToast(ex.Message);
            }
        }
        async void InitCategory(string input)
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
                var result = await LovelFactory.Lovel(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        CacheSpan = CandySoft.Cache,
                        Proxy = StaticResource.Proxy(),
                        ImplType = StaticResource.ImplType(),
                        LovelType = LovelEnum.Category,
                        Category = new LovelCategory
                        {
                            Page = this.Page,
                            Route = input
                        }
                    };
                }).RunsAsync();
                CloseBusy();
                Total = result.CategoryResult.Total;
                if (LoadMore)
                    result.CategoryResult.ElementResults.ForEach(item =>
                    {
                        CategoryResult.Add(item);
                    });
                else
                    CategoryResult = new ObservableCollection<LovelCategoryElementResult>(result.CategoryResult.ElementResults);
            }
            catch (Exception ex)
            {
                StaticResource.PopToast(ex.Message);
            }
        }
        async void InitQeury()
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
                var result = await LovelFactory.Lovel(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        CacheSpan = CandySoft.Cache,
                        Proxy = StaticResource.Proxy(),
                        ImplType = StaticResource.ImplType(),
                        LovelType = LovelEnum.Search,
                        Search = new LovelSearch
                        {
                            KeyWord = KeyWord,
                            SearchType = LovelSearchEnum.ArticleName,
                            Page = Page
                        }
                    };
                }).RunsAsync();
                CloseBusy();
                Total = result.SearchResult.Total;
                if (LoadMore)
                    result.SearchResult.ElementResults.ForEach(item =>
                    {
                        QueryResult.Add(item);
                    });
                else
                    QueryResult = new ObservableCollection<LovelSearchElementResult>(result.SearchResult.ElementResults);
                CategoryResult = new ObservableCollection<LovelCategoryElementResult>(QueryResult.ToMapest<List<LovelCategoryElementResult>>());
            }
            catch (Exception ex)
            {
                StaticResource.PopToast(ex.Message);
            }
        }
        async void InitDetail(LovelCategoryElementResult input)
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
                await Task.Delay(CandySoft.Wait / 2);
                var result_detail = await LovelFactory.Lovel(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        CacheSpan = CandySoft.Cache,
                        Proxy = StaticResource.Proxy(),
                        ImplType = StaticResource.ImplType(),
                        LovelType = LovelEnum.Detail,
                        Detail = new LovelDetail
                        {
                            Route = input.DetailAddress
                        }
                    };
                }).RunsAsync();

                await Task.Delay(CandySoft.Wait / 2);
                var result_chapter = await LovelFactory.Lovel(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        CacheSpan = CandySoft.Cache,
                        Proxy = StaticResource.Proxy(),
                        ImplType = StaticResource.ImplType(),
                        LovelType = LovelEnum.View,
                        View = new LovelView
                        {
                            Route = result_detail.DetailResult.Route
                        }
                    };
                }).RunsAsync();
                CloseBusy();
                Navigation(input, result_chapter.ViewResult);
            }
            catch (Exception ex)
            {
                StaticResource.PopToast(ex.Message);
            }
        }

        async void Navigation(LovelCategoryElementResult input, List<LovelViewResult> views)
        {
            await Shell.Current.GoToAsync(nameof(LovelDetailView), new Dictionary<string, object> { { "Route", input }, { "Result", views } });
        }
        #endregion

        #region 命令
        public DelegateCommand QueryAction => new(() =>
        {
            this.Page = 1;
            CategoryRoute = string.Empty;
            LoadMore = false;
            Task.Run(() => InitQeury());
        });
        public DelegateCommand RefreshAction => new(() =>
        {
            this.Page = 1;
            SetRefresh(false);
            LoadMore = false;
            if (!CategoryRoute.IsNullOrEmpty())
                Task.Run(() => InitCategory(CategoryRoute));
            else
                Task.Run(() => InitQeury());
        });
        public DelegateCommand LoadMoreAction => new(() =>
        {
            if (Lock) return;
            this.Page += 1;
            if (this.Page > Total) return;
            LoadMore = true;
            SetRefresh();
            if (KeyWord.IsNullOrEmpty()) InitCategory(CategoryRoute);
            else InitQeury();
        });
        public DelegateCommand<string> CategoryAction => new(input =>
        {
            this.Page = 1;
            SetRefresh();
            CategoryRoute = input;
            KeyWord = string.Empty;
            LoadMore = false;
            Task.Run(() => InitCategory(input));
        });
        public DelegateCommand<LovelCategoryElementResult> DetailAction => new(input =>
        {
            SetRefresh();
            InitDetail(input);
        });
        #endregion
    }
}
