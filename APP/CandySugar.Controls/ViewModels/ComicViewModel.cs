using CandySugar.Controls.Views.ComicViews;
using Sdk.Component.Comic.sdk;
using Sdk.Component.Comic.sdk.ViewModel;
using Sdk.Component.Comic.sdk.ViewModel.Enums;
using Sdk.Component.Comic.sdk.ViewModel.Request;
using Sdk.Component.Comic.sdk.ViewModel.Response;
using Sdk.Component.Hnime.sdk.ViewModel.Request;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XExten.Advance.LinqFramework;

namespace CandySugar.Controls.ViewModels
{
    public class ComicViewModel : BaseViewModel
    {
        public override void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("Key"))
            {
                if (!query["Key"].ToString().IsNullOrEmpty())
                {
                    this.Page = 1;
                    SetRefresh();
                    LoadMore = false;
                    CategoryRoute = query["Key"].ToString();
                    Keyword = string.Empty;
                    Task.Run(() => InitCategory());
                }
            }
        }

        #region 字段
        bool LoadMore = false;
        string Keyword;
        string CategoryRoute;
        #endregion

        #region  属性
        ObservableCollection<ComicSearchElementResult> _SearchResult;
        public ObservableCollection<ComicSearchElementResult> SearchResult
        {
            get => _SearchResult;
            set => SetProperty(ref _SearchResult, value);
        }
        #endregion

        #region 命令
        public DelegateCommand QueryAction => new(() =>
        {
            this.Page = 1;
            Keyword = this.KeyWord;
            this.CategoryRoute = String.Empty;
            SetRefresh();
            LoadMore = false;
            Task.Run(() => InitSearch());

        });
        public DelegateCommand<ComicSearchElementResult> DetailAction => new(input => InitView(input));
        public DelegateCommand RefreshAction => new(() => {

            SetRefresh(false);
            LoadMore = false;
            this.Page = 1;
            if (!Keyword.IsNullOrEmpty()) Task.Run(() => InitSearch());
            else Task.Run(() => InitCategory());

        });
        public DelegateCommand LoadMoreAction => new(() => {

            SetRefresh();
            //网络慢不能使用异步加载更多
            if (Lock) return;
            LoadMore = true;
            this.Page += 1;
            if (this.Page > Total) return;
            if (!Keyword.IsNullOrEmpty())
                Task.Run(() => InitSearch());
            else
                Task.Run(() => InitCategory());
        });
        #endregion

        #region 方法
        async void InitSearch()
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
                var result = await ComicFactory.Comic(opt =>
            {
                opt.RequestParam = new Input
                {
                    CacheSpan = CandySoft.Cache,
                    Proxy = StaticResource.Proxy(),
                    ImplType = StaticResource.ImplType(),
                    ComicType = ComicEnum.Search,
                    Search = new ComicSearch
                    {
                        Page = Page,
                        Keyword = Keyword
                    }
                };
            }).RunsAsync();
                CloseBusy();
                this.Total = result.SearchResult.Total;
                if (LoadMore)
                    result.SearchResult.ElementResults.ForEach(item =>
                    {
                        SearchResult.Add(item);
                    });
                else
                    SearchResult = new ObservableCollection<ComicSearchElementResult>(result.SearchResult.ElementResults);
            }
            catch (Exception ex)
            {
                StaticResource.PopToast(ex.Message);
            }
        }
        async void InitCategory() 
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
                var result = await ComicFactory.Comic(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        CacheSpan = CandySoft.Cache,
                        Proxy = StaticResource.Proxy(),
                        ImplType = StaticResource.ImplType(),
                        ComicType = ComicEnum.Category,
                        Category = new ComicCategory
                        {
                            Page = Page,
                            Route = CategoryRoute
                        }
                    };
                }).RunsAsync();
                CloseBusy();
                this.Total = result.CategoryResult.Total;
                if (LoadMore)
                    result.CategoryResult.ElementResults.ToMapest<List<ComicSearchElementResult>>().ForEach(item =>
                    {
                        SearchResult.Add(item);
                    });
                else
                    SearchResult = new ObservableCollection<ComicSearchElementResult>(result.CategoryResult.ElementResults.ToMapest<List<ComicSearchElementResult>>());
            }
            catch (Exception ex)
            {
                StaticResource.PopToast(ex.Message);
            }
        }
        async void InitView(ComicSearchElementResult input)
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
                var result = await ComicFactory.Comic(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        CacheSpan = CandySoft.Cache,
                        Proxy = StaticResource.Proxy(),
                        ImplType = StaticResource.ImplType(),
                        ComicType = ComicEnum.View,
                        View = new  ComicView
                        {
                             Route = input.Route
                        }
                    };
                }).RunsAsync();
                CloseBusy();
                Navigation(input, result.ViewResult);
            }
            catch (Exception ex)
            {
                StaticResource.PopToast(ex.Message);
            }
        }
        async void Navigation(ComicSearchElementResult input, ComicViewResult result)
        {
            await Shell.Current.GoToAsync(nameof(ComicDetailView), new Dictionary<string, object> { { "Key", input },{ "View",result} });
        }
        #endregion
    }
}
