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
            this.Page = 1;
            InitNovel();
        }

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
        private async void InitNovel()
        {
            if (IsBusy) return;
            try
            {
                if (CandySoft.NetState.NetworkAccess != NetworkAccess.Internet)
                {
                    await Shell.Current.DisplayAlert("网络异常！", "请检查网络是否通畅！", "是");
                    return;
                }
                IsBusy = true;
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
                InitResult = new ObservableCollection<NovelInitCategoryResult>(result.CateInitResults);
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("错误！", ex.Message, "是");
            }
            finally
            {
                IsBusy = false;
                IsRefreshing = false;
            }
        }
        private async void InitCategory(string input)
        {
            if (IsBusy) return;
            try
            {
                if (CandySoft.NetState.NetworkAccess != NetworkAccess.Internet)
                {
                    await Shell.Current.DisplayAlert("网络异常！", "请检查网络是否通畅！", "是");
                    return;
                }
                IsBusy = true;
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
                Total = result.CategoryResult.Total;
                CategoryResult = new ObservableCollection<NovelCategoryElementResult>(result.CategoryResult.ElementResults);
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("错误！", ex.Message, "是");
            }
            finally
            {
                IsBusy = false;
                IsRefreshing = false;
            }
        }
        #endregion

        #region 命令
        public DelegateCommand<string> CategoryAction => new(input =>
        {
            InitCategory(input);
        });
        #endregion
    }
}
