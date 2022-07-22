using CandySugar.Controls.Views.NovelViews;
using Sdk.Component.Novel.sdk;
using Sdk.Component.Novel.sdk.ViewModel;
using Sdk.Component.Novel.sdk.ViewModel.Enums;
using Sdk.Component.Novel.sdk.ViewModel.Request;
using Sdk.Component.Novel.sdk.ViewModel.Response;

namespace CandySugar.Controls.ViewModels
{
    public class NovelViewModel : BaseViewModel
    {
        public NovelViewModel()
        {
            Task.Run(() => InitNovel());
        }

        #region 字段
        string CategoryRoute = string.Empty;

        #endregion

        #region 属性
        /// <summary>
        /// 首页分类
        /// </summary>
        ObservableCollection<NovelInitCategoryResult> _InitResult;
        public ObservableCollection<NovelInitCategoryResult> InitResult
        {
            get => _InitResult;
            set => SetProperty(ref _InitResult, value);
        }
        /// <summary>
        /// 分类结果
        /// </summary>
        ObservableCollection<NovelCategoryElementResult> _CategoryResult;
        public ObservableCollection<NovelCategoryElementResult> CategoryResult
        {
            get => _CategoryResult;
            set => SetProperty(ref _CategoryResult, value);
        }
        #endregion

        #region 方法
        async void InitNovel()
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
                var result = await NovelFactory.Novel(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        CacheSpan = CandySoft.Cache,
                        Proxy = StaticResource.Proxy(),
                        ImplType = StaticResource.ImplType(),
                        NovelType = NovelEnum.Init
                    };
                }).RunsAsync();
                CloseBusy();
                InitResult = new ObservableCollection<NovelInitCategoryResult>(result.CateInitResults);
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
                var result = await NovelFactory.Novel(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        CacheSpan = CandySoft.Cache,
                        Proxy = StaticResource.Proxy(),
                        ImplType = StaticResource.ImplType(),
                        NovelType = NovelEnum.Category,
                        Category = new NovelCategory
                        {
                            Page = this.Page,
                            CategoryRoute = this.CategoryRoute,
                        }
                    };
                }).RunsAsync();
                CloseBusy();
                Total = result.CategoryResult.Total;
                if (CategoryResult == null)
                    CategoryResult = new ObservableCollection<NovelCategoryElementResult>(result.CategoryResult.ElementResults);
                else
                    result.CategoryResult.ElementResults.ForEach(item =>
                    {
                        CategoryResult.Add(item);
                    });

            }
            catch (Exception ex)
            {
                StaticResource.PopToast(ex.Message);
            }
        }
        async void InitQuery()
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
                var result = await NovelFactory.Novel(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        CacheSpan = CandySoft.Cache,
                        Proxy = StaticResource.Proxy(),
                        ImplType = StaticResource.ImplType(),
                        NovelType = NovelEnum.Search,
                        Search = new NovelSearch
                        {
                            KeyWord = KeyWord,
                        }
                    };
                }).RunsAsync();
                CloseBusy();
                CategoryResult = new ObservableCollection<NovelCategoryElementResult>(result.SearchResults.ToMapest<List<NovelCategoryElementResult>>());
            }
            catch (Exception ex)
            {
                StaticResource.PopToast(ex.Message);
            }
        }
        async void InitDetail(string input)
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
                var result = await NovelFactory.Novel(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        CacheSpan = CandySoft.Cache,
                        Proxy = StaticResource.Proxy(),
                        ImplType = StaticResource.ImplType(),
                        NovelType = NovelEnum.Detail,
                        Detail = new NovelDetail
                        {
                            DetailRoute = input
                        }
                    };
                }).RunsAsync();
                CloseBusy();
                await Shell.Current.GoToAsync(nameof(NovelDetailView), new Dictionary<string, object> { { "Key", result.DetailResult }, { "Route", input } });
            }
            catch (Exception ex)
            {
                StaticResource.PopToast(ex.Message);
            }
        }
        #endregion

        #region 命令
        public DelegateCommand<string> CategoryAction => new(input =>
        {
            this.Page = 1;
            SetRefresh();
            CategoryRoute = input;
            CategoryResult = null;
            Task.Run(() => InitCategory());
        });
        public DelegateCommand RefreshAction => new(() =>
        {
            this.Page = 1;
            SetRefresh(false);
            CategoryResult = null;
            Task.Run(() => InitCategory());
        });
        public DelegateCommand LoadMoreAction => new(() =>
        {
            if (Lock) return;
            this.Page += 1;
            if (this.Page > Total) return;
            InitCategory();
        });
        public DelegateCommand QueryAction => new(() =>
        {
            if (!KeyWord.IsNullOrEmpty())
                Task.Run(() => InitQuery());
        });
        public DelegateCommand<string> DetailAction => new(input =>
        {
            SetRefresh();
            InitDetail(input);
        });
        #endregion
    }
}
