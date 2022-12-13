using Sdk.Component.Hnime.sdk;
using Sdk.Component.Hnime.sdk.ViewModel;
using Sdk.Component.Hnime.sdk.ViewModel.Enums;
using Sdk.Component.Hnime.sdk.ViewModel.Request;
using Sdk.Component.Hnime.sdk.ViewModel.Response;
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
        /// <summary>
        /// 1 查询 2 分类 
        /// </summary>
        public static int Module = 0;
        public override void OnLoad()
        {
            Task.Run(Init);
        }
        #region Property
        public string Route { get; set; }
        public string Key { get; set; }
        #endregion

        #region Property
        ObservableCollection<HnimeInitResult> _InitResult;
        public ObservableCollection<HnimeInitResult> InitResult
        {
            get => _InitResult;
            set => SetProperty(ref _InitResult, value);
        }
        ObservableCollection<HnimeSearchElementResult> _Result;
        public ObservableCollection<HnimeSearchElementResult> Result
        {
            get => _Result;
            set => SetProperty(ref _Result, value);
        }
        #endregion

        #region Method
       public async void Init()
        {
            try
            {
                Module = 0;
                Activity = true;
                await Task.Delay(DataBus.Delay);
                var result = await HnimeFactory.Hnime(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        CacheSpan = DataBus.Cache,
                        ImplType = DataCenter.ImplType(),
                        HnimeType = HnimeEnum.Init,
                        Init = new HnimeInit() { InitLabel = false }
                    };
                }).RunsAsync();
                InitResult = new ObservableCollection<HnimeInitResult>(result.InitResults);
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
                Module = 2;
                SetState(More);
                await Task.Delay(DataBus.Delay);
                var result = await HnimeFactory.Hnime(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        CacheSpan = DataBus.Cache,
                        ImplType = DataCenter.ImplType(),
                        HnimeType = HnimeEnum.Category,
                        Category = new HnimeCategory
                        {
                            Route = Route,
                            Page = this.Page
                        }
                    };
                }).RunsAsync();
                Total = result.SearchResult.Total;
                if (More)
                    result.SearchResult.ElementResult.ForEach(Result.Add);
                else
                    Result = new ObservableCollection<HnimeSearchElementResult>(result.SearchResult.ElementResult);
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
                Module = 1;
                SetState(More);
                await Task.Delay(DataBus.Delay);
                var result = await HnimeFactory.Hnime(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        CacheSpan = DataBus.Cache,
                        ImplType = DataCenter.ImplType(),
                        HnimeType = HnimeEnum.Search,
                        Search = new HnimeSearch
                        {
                            Page = this.Page,
                            KeyWord= Key
                        }
                    };
                }).RunsAsync();
                Total = result.SearchResult.Total;
                if (More)
                    result.SearchResult.ElementResult.ForEach(Result.Add);
                else
                    Result = new ObservableCollection<HnimeSearchElementResult>(result.SearchResult.ElementResult);
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
        public DelegateCommand<string> GroupCommand => new(input =>
        {
            this.Page = 1;
            this.Route = input;
            Task.Run(() => GroupInit(false));
        });
        public DelegateCommand RefreshCommand => new(() =>
        {
            this.Page = 1;
            if (Module == 1) Task.Run(() => QueryInit(false));
            if (Module == 2) Task.Run(() => GroupInit(false));
        });
        public DelegateCommand MoreCommand => new(() =>
        {

            this.Page += 1;
            if (this.Page > this.Total) return;
            if (Module == 1) Task.Run(() => QueryInit(true));
            if (Module == 2) Task.Run(() => GroupInit(true));
        });
        public DelegateCommand<string> DetailCommand => new(input => { 
        
        });
        #endregion
    }
}
