using Microsoft.Maui;
using Sdk.Component.Axgle.sdk;
using Sdk.Component.Axgle.sdk.ViewModel;
using Sdk.Component.Axgle.sdk.ViewModel.Enums;
using Sdk.Component.Axgle.sdk.ViewModel.Request;
using Sdk.Component.Axgle.sdk.ViewModel.Response;
using System.Collections.ObjectModel;

namespace CandySugar.Controls
{
    public class IViewModel : ViewModelBase
    {
        readonly IService Service;
        public IViewModel(BaseServices baseServices) : base(baseServices)
        {
            Service = this.Container.Resolve<IService>();
        }
        public override void OnLoad()
        {
            Task.Run(Init);
        }
        /// <summary>
        /// 1 分类 2 查询
        /// </summary>
        public static int Module = 0;

        #region Property
        public string Key { get; set; }
        public int CID { get; set; }
        #endregion

        #region Property
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

        #region Method
        async void Init()
        {
            try
            {
                Module = 0;
                SetActivity();
                await Task.Delay(DataBus.Delay);
                var result = await AxgleFactory.Axgle(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        CacheSpan = DataBus.Cache,
                        ImplType = DataCenter.ImplType(),
                        AxgleType = AxgleEnum.Init,
                    };
                }).RunsAsync();
                InitResult = new ObservableCollection<AxgleInitResult>(result.InitResults);
                SetState();
            }
            catch (Exception ex)
            {
                await Service.AddLog("IInit异常", ex);
                "IInit异常".OpenToast();
            }
        }
        async void GroupInit(bool More)
        {
            try
            {
                Module = 1;
                SetActivity();
                await Task.Delay(DataBus.Delay);
                var result = await AxgleFactory.Axgle(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        CacheSpan = DataBus.Cache,
                        ImplType = DataCenter.ImplType(),
                        AxgleType = AxgleEnum.Category,
                        Category = new AxgleCategory
                        {
                            CId = CID,
                            Page = Page,
                        }
                    };
                }).RunsAsync();
                Total = result.CategoryResult.Total;
                if (More)
                    result.CategoryResult.ElementResult.ToMapest<List<AxgleSearchElementResult>>().ForEach(this.QueryResult.Add);
                else
                    this.QueryResult = new ObservableCollection<AxgleSearchElementResult>(result.CategoryResult.ElementResult.ToMapest<List<AxgleSearchElementResult>>());
                SetState();
            }
            catch (Exception ex)
            {
                await Service.AddLog("IGroupInit异常", ex);
                "IGroupInit异常".OpenToast();
            }
        }
        public async void QueryInit(bool More)
        {
            try
            {
                Module = 2;
                SetActivity();
                await Task.Delay(DataBus.Delay);
                var result = await AxgleFactory.Axgle(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        CacheSpan = DataBus.Cache,
                        ImplType = DataCenter.ImplType(),
                        AxgleType = AxgleEnum.Search,
                        Search = new AxgleSearch
                        {
                            KeyWord = Key,
                            Page = Page
                        }
                    };
                }).RunsAsync();
                Total = result.SearchResult.Total;
                if (More)
                    result.SearchResult.ElementResult.ForEach(this.QueryResult.Add);
                else
                    this.QueryResult = new ObservableCollection<AxgleSearchElementResult>(result.SearchResult.ElementResult);
                SetState();
            }
            catch (Exception ex)
            {
                await Service.AddLog("IQueryInit异常", ex);
                "IQueryInit异常".OpenToast();
            }
        }
        #endregion

        #region Command
        public DelegateCommand<string> GroupCammand => new(input =>
        {
            this.Page = 1;
            this.CID = input.AsInt();
            Task.Run(() => GroupInit(false));
        });
        public DelegateCommand RefreshCommand => new(() =>
        {
            this.Page = 1;
            if (Module == 1) Task.Run(() => GroupInit(false));
            if (Module == 2) Task.Run(() => QueryInit(false));
        });
        public DelegateCommand MoreCommand => new(() =>
        {
            this.Page += 1;
            if (this.Page > this.Total) return;
            if (Module == 1) Task.Run(() => GroupInit(true));
            if (Module == 2) Task.Run(() => QueryInit(true));
        });
        public DelegateCommand<string> DetailCommand => new(input =>
        {
            Nav.NavigateAsync(new Uri("I1", UriKind.Relative), new NavigationParameters { { "Route", input } });
        });
        #endregion
    }
}
