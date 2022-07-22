using CandySugar.Controls.Views.AxgleViews;
using Sdk.Component.Axgle.sdk;
using Sdk.Component.Axgle.sdk.ViewModel;
using Sdk.Component.Axgle.sdk.ViewModel.Enums;
using Sdk.Component.Axgle.sdk.ViewModel.Request;
using Sdk.Component.Axgle.sdk.ViewModel.Response;

namespace CandySugar.Controls.ViewModels
{
    public class AxgleViewModel : BaseViewModel
    {
        ICandyService CandyService;
        public AxgleViewModel()
        {
            CandyService = CandyContainer.Instance.Resolve<ICandyService>();
            Task.Run(() => InitAxgle());
        }

        #region 字段
        string CategoryId = string.Empty;
        bool LoadMore = false;
        AxgleDescEnum Desc = AxgleDescEnum.TopRated;
        #endregion

        #region 属性
        ObservableCollection<AxgleInitResult> _InitResult;
        public ObservableCollection<AxgleInitResult> InitResult
        {
            get => _InitResult;
            set => SetProperty(ref _InitResult, value);
        }
        ObservableCollection<AxgleSearchElementResult> _QueryResult;
        public ObservableCollection<AxgleSearchElementResult> QueryResult
        {
            get => _QueryResult;
            set => SetProperty(ref _QueryResult, value);
        }
        #endregion

        #region 方法
        async void InitAxgle()
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
                var result = await AxgleFactory.Axgle(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        CacheSpan = CandySoft.Cache,
                        Proxy = StaticResource.Proxy(),
                        ImplType = StaticResource.ImplType(),
                        AxgleType = AxgleEnum.Init,
                    };
                }).RunsAsync();
                CloseBusy();
                InitResult = new ObservableCollection<AxgleInitResult>(result.InitResults);
            }
            catch (Exception ex)
            {
                StaticResource.PopToast(ex.Message);
            }
        }
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
                var result = await AxgleFactory.Axgle(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        CacheSpan = CandySoft.Cache,
                        Proxy = StaticResource.Proxy(),
                        ImplType = StaticResource.ImplType(),
                        AxgleType = AxgleEnum.Search,
                        Search = new AxgleSearch
                        {
                            KeyWord = KeyWord,
                            Page = Page
                        }
                    };
                }).RunsAsync();
                CloseBusy();
                this.Total = result.SearchResult.Total;
                if (LoadMore)
                {
                    result.SearchResult.ElementResult.ForEach(item =>
                    {
                        this.QueryResult.Add(item);
                    });
                }
                else
                {
                    this.QueryResult = new ObservableCollection<AxgleSearchElementResult>(result.SearchResult.ElementResult);
                }

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
                var result = await AxgleFactory.Axgle(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        CacheSpan = CandySoft.Cache,
                        Proxy = StaticResource.Proxy(),
                        ImplType = StaticResource.ImplType(),
                        AxgleType = AxgleEnum.Category,
                        Category = new AxgleCategory
                        {
                            CId = CategoryId.AsInt(),
                            Page = Page,
                            Desc = Desc
                        }
                    };
                }).RunsAsync();
                CloseBusy();
                this.Total = result.CategoryResult.Total;
                if (LoadMore)
                {
                    result.CategoryResult.ElementResult.ToMapest<List<AxgleSearchElementResult>>().ForEach(item =>
                    {
                        this.QueryResult.Add(item);
                    });
                }
                else
                {
                    var Target = result.CategoryResult.ElementResult.ToMapest<List<AxgleSearchElementResult>>();
                    this.QueryResult = new ObservableCollection<AxgleSearchElementResult>(Target);
                }
            }
            catch (Exception ex)
            {
                StaticResource.PopToast(ex.Message);
            }
        }
        async void Navigation(string input)
        {
            await Shell.Current.GoToAsync($"{nameof(AxglePlayView)}?Key={input}");
        }
        void Logic(AxgleSearchElementResult input)
        {
            CandyService.AddOrAlterAxgle(input.ToMapest<CandyAxgle>());
            StaticResource.PopToast("已收藏成功!");
        }
        #endregion

        #region 命令
        public DelegateCommand QueryAction => new(() =>
        {
            this.Page = 1;
            SetRefresh();
            LoadMore = false;
            this.CategoryId = string.Empty;
            Task.Run(() => InitSearch());
        });
        public DelegateCommand LoadMoreAction => new(() =>
        {
            if (Lock) return;
            this.Page += 1;
            if (this.Page > Total) return;
            LoadMore = true;
            SetRefresh();
            if (KeyWord.IsNullOrEmpty()) InitCategory();
            else InitSearch();
        });
        public DelegateCommand RefreshAction => new(() =>
        {
            this.Page = 1;
            SetRefresh(false);
            LoadMore = false;
            if (!CategoryId.IsNullOrEmpty())
                Task.Run(() => InitCategory());
            else
                Task.Run(() => InitSearch());
        });
        public DelegateCommand<string> PlayAction => new(input =>
        {
            Navigation(input);
        });
        public DelegateCommand<string> CategoryAction => new(input =>
        {
            SetRefresh();
            this.CategoryId = input;
            this.KeyWord = string.Empty;
            this.Page = 1;
            LoadMore = false;
            Task.Run(() => InitCategory());
        });
        public DelegateCommand<string> HandlerAction => new(input =>
        {
            Desc =(AxgleDescEnum)input.AsInt();
            if (!CategoryId.IsNullOrEmpty())
            {
                this.KeyWord = string.Empty;
                this.Page = 1;
                LoadMore = false;
                Task.Run(() => InitCategory());
            }
        });
        public DelegateCommand<AxgleSearchElementResult> StarAction => new(input => {
            Logic(input);
        });
        #endregion
    }
}
