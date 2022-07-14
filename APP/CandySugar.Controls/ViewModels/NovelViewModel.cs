using CandySugar.Library;
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
            InitNovel();
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
                    await Shell.Current.DisplayAlert("网络异常！", "请检查网络是否通畅！", "是");
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
                await Shell.Current.DisplayAlert("错误！", ex.Message, "是");
            }
        }
        async void InitCategory(string input)
        {
            if (IsBusy) return;
            try
            {
                if (CandySoft.NetState.NetworkAccess != NetworkAccess.Internet)
                {
                    await Shell.Current.DisplayAlert("网络异常！", "请检查网络是否通畅！", "是");
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
                            CategoryRoute = input,
                        }
                    };
                }).RunsAsync();
                CloseBusy();
                Total = Math.Ceiling(result.CategoryResult.Total / 20d);
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
                await Shell.Current.DisplayAlert("错误！", ex.Message, "是");
            }
        }
        async void InitQuery()
        {
            if (IsBusy) return;
            try
            {
                if (CandySoft.NetState.NetworkAccess != NetworkAccess.Internet)
                {
                    await Shell.Current.DisplayAlert("网络异常！", "请检查网络是否通畅！", "是");
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
                await Shell.Current.DisplayAlert("错误！", ex.Message, "是");
            }
        }
        #endregion

        #region 命令
        public DelegateCommand<string> CategoryAction => new(input =>
        {
            this.Page = 1;
            SetRefresh();
            CategoryRoute = input;
            InitCategory(input);
        });
        public DelegateCommand RefreshAction => new(() =>
        {
            SetRefresh(false);
            InitCategory(CategoryRoute);
        });
        public DelegateCommand LoadMoreAction => new(() =>
        {
            if (Lock) return;
            this.Page += 1;
            InitCategory(CategoryRoute);
        });
        public DelegateCommand QueryAction => new(() =>
        {
            if (!KeyWord.IsNullOrEmpty())
            {
                InitQuery();
            }
        });
        #endregion
    }
}
