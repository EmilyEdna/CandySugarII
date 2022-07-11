using CandySugar.Controls.Views.LovelViews;
using CandySugar.Library;
using Sdk.Component.Lovel.sdk;
using Sdk.Component.Lovel.sdk.ViewModel;
using Sdk.Component.Lovel.sdk.ViewModel.Enums;
using Sdk.Component.Lovel.sdk.ViewModel.Request;
using Sdk.Component.Lovel.sdk.ViewModel.Response;
using Sdk.Component.Novel.sdk;
using Sdk.Component.Novel.sdk.ViewModel.Response;

namespace CandySugar.Controls.ViewModels
{
    public class LovelViewModel : BaseViewModel
    {
        public LovelViewModel()
        {
            InitLovel();
        }
        #region 字段
        string CategoryRoute = string.Empty;
        #endregion

        #region 属性
        string _KeyWord;
        public string KeyWord
        {
            get => _KeyWord;
            set
            {
                SetProperty(ref _KeyWord, value);
            }
        }
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
        async Task InitLovel()
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
                if (CategoryResult == null)
                    CategoryResult = new ObservableCollection<LovelCategoryElementResult>(result.CategoryResult.ElementResults);
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
        async void InitQeury()
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
                if (QueryResult == null)
                    QueryResult = new ObservableCollection<LovelSearchElementResult>(result.SearchResult.ElementResults);
                else
                    result.SearchResult.ElementResults.ForEach(item =>
                    {
                        QueryResult.Add(item);
                    });
                CategoryResult = new ObservableCollection<LovelCategoryElementResult>(QueryResult.ToMapest<List<LovelCategoryElementResult>>());
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("错误！", ex.Message, "是");
            }
        }
        async void Navgation(string input)
        {
            await Shell.Current.GoToAsync($"{nameof(LovelDetailView)}?Route={input}");
        }
        #endregion

        #region 命令
        public DelegateCommand QueryAction => new(() =>
        {
            this.Page = 1;
            CategoryRoute = string.Empty;
            InitQeury();
        });
        public DelegateCommand RefreshAction => new(() =>
        {
            if (!CategoryRoute.IsNullOrEmpty())
                InitCategory(this.CategoryRoute);
        });
        public DelegateCommand LoadMoreAction => new(() =>
        {
            this.Page += 1;
            if (KeyWord.IsNullOrEmpty()) InitCategory(CategoryRoute);
            else InitQeury();
        });
        public DelegateCommand<string> CategoryAction => new(input =>
        {
            this.Page = 1;
            CategoryRoute = input;
            KeyWord = string.Empty;
            InitCategory(input);
        });
        public DelegateCommand<string> DetailAction => new(input =>
        {
            Navgation(input);
        });
        #endregion
    }
}
