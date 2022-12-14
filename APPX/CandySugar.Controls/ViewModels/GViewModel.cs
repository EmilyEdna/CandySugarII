using Sdk.Component.Movie.sdk;
using Sdk.Component.Movie.sdk.ViewModel;
using Sdk.Component.Movie.sdk.ViewModel.Enums;
using Sdk.Component.Movie.sdk.ViewModel.Request;
using Sdk.Component.Movie.sdk.ViewModel.Response;
using System.Collections.ObjectModel;

namespace CandySugar.Controls
{
    public class GViewModel : ViewModelBase
    {
        readonly IService Service;
        public GViewModel(BaseServices baseServices) : base(baseServices)
        {
            Service = this.Container.Resolve<IService>();
        }
        /// <summary>
        /// 1 分类 2 查询
        /// </summary>
        public static int Module = 0;
        public override void OnLoad()
        {
            Task.Run(Init);
        }

        #region Property
        public string Key { get; set; }
        public string Route { get; set; }
        public int SearchId { get; set; }
        #endregion

        #region Property
        ObservableCollection<MovieInitResult> _InitResult;
        public ObservableCollection<MovieInitResult> InitResult
        {
            get => _InitResult;
            set => SetProperty(ref _InitResult, value);
        }
        ObservableCollection<MovieElementResult> _RootResult;
        public ObservableCollection<MovieElementResult> RootResult
        {
            get => _RootResult;
            set => SetProperty(ref _RootResult, value);
        }
        #endregion

        #region Method
        async void Init()
        {
            try
            {
                Module = 0;
                Activity = true;
                await Task.Delay(DataBus.Delay);
                var result = await MovieFactory.Movie(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        CacheSpan = DataBus.Cache,
                        ImplType = DataCenter.ImplType(),
                        MovieType = MovieEnum.Init
                    };
                }).RunsAsync();
                InitResult = new ObservableCollection<MovieInitResult>(result.InitResults);
                SetState();
            }
            catch (Exception ex)
            {
                await Service.AddLog("GInit异常", ex);
                "GInit异常".OpenToast();
            }
        }
        async void GroupInit(bool More)
        {
            try
            {
                Module = 1;
                SetState(More);
                await Task.Delay(DataBus.Delay);
                var result = await MovieFactory.Movie(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        CacheSpan = DataBus.Cache,
                        ImplType = DataCenter.ImplType(),
                        MovieType = MovieEnum.Category,
                        Category = new MovieCategory
                        {
                            Page = this.Page,
                            Route = this.Route
                        }
                    };
                }).RunsAsync();
                this.Total = result.RootResult.Total;
                if (More)
                    result.RootResult.ElementResults.ForEach(RootResult.Add);
                else
                    RootResult = new ObservableCollection<MovieElementResult>(result.RootResult.ElementResults);
                SetState();
            }
            catch (Exception ex)
            {
                await Service.AddLog("GroupInit异常", ex);
                "GroupInit异常".OpenToast();
            }
        }
        public async void QueryInit(bool More)
        {
            try
            {
                Module = 2;
                SetState(More);
                await Task.Delay(DataBus.Delay);
                var result = await MovieFactory.Movie(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        CacheSpan = DataBus.Cache,
                        ImplType = DataCenter.ImplType(),
                        MovieType = MovieEnum.Search,
                        Search = new MovieSearch
                        {
                            Page = this.Page,
                            KeyWord = this.Key,
                            SearchId= this.SearchId,
                        }
                    };
                }).RunsAsync();
                this.Total = result.RootResult.Total;
                this.SearchId = result.RootResult.SearchId;
                if (More)
                    result.RootResult.ElementResults.ForEach(RootResult.Add);
                else
                    RootResult = new ObservableCollection<MovieElementResult>(result.RootResult.ElementResults);
                SetState();
            }
            catch (Exception ex)
            {
                await Service.AddLog("GroupInit异常", ex);
                "GroupInit异常".OpenToast();
            }
        }
        #endregion

        #region Command
        public DelegateCommand<string> GroupCammand => new(input =>
        {
            this.Page = 1;
            this.Route = input;
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
        public DelegateCommand<MovieElementResult> DetailCommand => new(input =>
        {
            Nav.NavigateAsync(new Uri("G1", UriKind.Relative), new NavigationParameters { { "Data", input.ToMapest<GRootEntity>() } });
        });
        #endregion
    }
}
