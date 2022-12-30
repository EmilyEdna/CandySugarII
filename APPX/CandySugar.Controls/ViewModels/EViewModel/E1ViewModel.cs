using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sdk.Component.Novel.sdk;
using Sdk.Component.Novel.sdk.ViewModel;
using Sdk.Component.Novel.sdk.ViewModel.Enums;
using Sdk.Component.Novel.sdk.ViewModel.Request;
using Sdk.Component.Novel.sdk.ViewModel.Response;

namespace CandySugar.Controls
{
    public class E1ViewModel : ViewModelBase
    {
        readonly IService Service;
        public E1ViewModel(BaseServices baseServices) : base(baseServices)
        {
            Service = this.Container.Resolve<IService>();
        }
        public override void Initialize(INavigationParameters parameters)
        {
            Route = parameters.GetValue<string>("Route");
            Task.Run(() => DetailInit(false));
        }

        #region Property
        public string Route { get; set; }
        public bool Sort { get; set; }
        #endregion

        #region Property
        NovelDetailRootResult _Result;
        public NovelDetailRootResult Result
        {
            get => _Result;
            set => SetProperty(ref _Result, value);
        }
        ObservableCollection<NovelDetailElementResult> _ElementResult;
        public ObservableCollection<NovelDetailElementResult> ElementResult
        {
            get => _ElementResult;
            set => SetProperty(ref _ElementResult, value);
        }
        #endregion

        #region Method
        async void DetailInit(bool More)
        {
            try
            {
                SetActivity();
                await Task.Delay(DataBus.Delay);
                var result = await NovelFactory.Novel(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        CacheSpan = DataBus.Cache,
                        ImplType = DataCenter.ImplType(),
                        NovelType = NovelEnum.Detail,
                        Detail = new NovelDetail
                        {
                            Page = Sort ? this.Total : this.Page,
                            DetailRoute = this.Route,
                        }
                    };
                }).RunsAsync();
                Result = result.DetailResult;
                if (Sort)
                    result.DetailResult.ElementResults.Reverse();
                if (More)
                    result.DetailResult.ElementResults.ForEach(ElementResult.Add);
                else
                    ElementResult = new ObservableCollection<NovelDetailElementResult>(result.DetailResult.ElementResults);
                Add();
                SetState();
            }
            catch (Exception ex)
            {
                await Service.AddLog("E1DetailInit异常", ex);
                "E1DetailInit异常".OpenToast();
            }
        }
        async void Add()
        {
            await Service.EAdd(new ERootEntity
            {
                Author = Result.Author,
                Name = Result.BookName,
                Route = Route
            });
        }
        #endregion

        #region Command
        public DelegateCommand BackCommand => new(() =>
        {
            Nav.GoBackAsync();
        });
        public DelegateCommand ChangeCommand => new(() =>
        {
            if (Sort) Sort = false; else Sort = true;
            this.Total = Result.Total;
            Task.Run(() => DetailInit(false));
        });
        public DelegateCommand<NovelDetailElementResult> WatchCommand => new(input =>
        {
            Nav.NavigateAsync(new Uri("E2", UriKind.Relative), new NavigationParameters { { "Route", input.ChapterRoute } });
        });
        public DelegateCommand MoreCommand => new(() =>
        {
            if (Sort)
            {
                Total -= 1;
                if (this.Total >= 1)
                    Task.Run(() => DetailInit(true));
            }
            else
            {
                this.Page += 1;
                if (this.Page > Result.Total) return;
                Task.Run(() => DetailInit(true));
            }
        });
        #endregion
    }
}
