using CandySugar.Controls.Views.HnimeViews.Popups;
using Sdk.Component.Hnime.sdk;
using Sdk.Component.Hnime.sdk.ViewModel;
using Sdk.Component.Hnime.sdk.ViewModel.Enums;
using Sdk.Component.Hnime.sdk.ViewModel.Request;
using Sdk.Component.Hnime.sdk.ViewModel.Response;

namespace CandySugar.Controls.ViewModels
{
    public class HnimeViewModel : BaseViewModel
    {
       
        public HnimeViewModel()
        {
            //Task.Run(() => InitHnime());
            MessagingCenter.Subscribe<HnimeLableView, HnimeSearch>(this, "Query", (sender, parm) =>
            {
                Query = parm;
            });
        }

        #region 字段
        bool LoadMore = false;
        string CategoryRoute = string.Empty;
        static HnimeSearch Query = null;
        #endregion

        #region 属性
        ObservableCollection<HnimeInitResult> _InitResult;
        public ObservableCollection<HnimeInitResult> InitResult
        {
            get => _InitResult;
            set => SetProperty(ref _InitResult, value);
        }
        ObservableCollection<HnimeSearchElementResult> _CategoryResult;
        public ObservableCollection<HnimeSearchElementResult> CategoryResult
        {
            get => _CategoryResult;
            set => SetProperty(ref _CategoryResult, value);
        }
        #endregion

        #region 命令
        public DelegateCommand QueryAction => new(() =>
        {
            Query ??= new HnimeSearch();
            Query.KeyWord = this.KeyWord;
            this.CategoryRoute = String.Empty;
            SetRefresh();
            this.Page = 1;
            LoadMore = false;
            Task.Run(() => InitSearch());
        });
        public DelegateCommand LoadMoreAction => new(() =>
        {
            SetRefresh();
            //网络慢不能使用异步加载更多
            if (Lock) return;
            LoadMore = true;
            this.Page += 1;
            if (this.Page > Total) return;
            if (Query != null) Task.Run(() => InitCategory());
            else Task.Run(() => InitSearch());

        });
        public DelegateCommand LabelAction => new(() =>
        {
            PushPopup();
        });
        public DelegateCommand RefreshAction => new(() =>
        {
            SetRefresh(false);
            LoadMore = false;
            this.Page = 1;
            if(Query!=null) Task.Run(() => InitCategory());
            else Task.Run(() => InitSearch());
        });
        public DelegateCommand<string> CategoryAction => new(input => 
        {
            SetRefresh();
            this.Page = 1;
            this.CategoryRoute = input;
            Query = null;
            LoadMore = false;
            Task.Run(() => InitCategory());
        });
        public DelegateCommand<HnimeSearchElementResult> DetailAction => new(input => { });
        #endregion

        #region 方法
        async void InitHnime()
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
                var result = await HnimeFactory.Hnime(opt =>
                 {
                     opt.RequestParam = new Input
                     {
                         HnimeType = HnimeEnum.Init,
                         CacheSpan = CandySoft.Cache,
                         Proxy = StaticResource.Proxy(),
                         ImplType = StaticResource.ImplType(),
                         Init = new HnimeInit() { InitLabel = false }
                     };
                 }).RunsAsync();
                CloseBusy();
                InitResult = new ObservableCollection<HnimeInitResult>(result.InitResults);
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("错误！", ex.Message, "是");
            }
        }
        async void InitCategory()
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
                var result = await HnimeFactory.Hnime(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        HnimeType = HnimeEnum.Category,
                        CacheSpan = CandySoft.Cache,
                        Proxy = StaticResource.Proxy(),
                        ImplType = StaticResource.ImplType(),
                        Category = new HnimeCategory
                        {
                            Route = CategoryRoute,
                            Page = this.Page
                        }
                    };
                }).RunsAsync();
                CloseBusy();
                Total = result.SearchResult.Total;
                if (LoadMore)
                    result.SearchResult.ElementResult.ForEach(item =>
                    {
                        CategoryResult.Add(item);
                    });
                else
                    CategoryResult = new ObservableCollection<HnimeSearchElementResult>(result.SearchResult.ElementResult);
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("错误！", ex.Message, "是");
            }
        }
        async void InitSearch()
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
                var result = await HnimeFactory.Hnime(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        HnimeType = HnimeEnum.Search,
                        CacheSpan = CandySoft.Cache,
                        Proxy = StaticResource.Proxy(),
                        ImplType = StaticResource.ImplType(),
                        Search = Query
                    };
                }).RunsAsync();
                CloseBusy();
                Total = result.SearchResult.Total;
                if (LoadMore)
                    result.SearchResult.ElementResult.ForEach(item =>
                    {
                        CategoryResult.Add(item);
                    });
                else
                    CategoryResult = new ObservableCollection<HnimeSearchElementResult>(result.SearchResult.ElementResult);
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("错误！", ex.Message, "是");
            }
        }
        async void PushPopup()
        {
            HnimeLableView LabelView = new HnimeLableView();
            await MopupService.Instance.PushAsync(LabelView);
        }
        #endregion
    }
}
