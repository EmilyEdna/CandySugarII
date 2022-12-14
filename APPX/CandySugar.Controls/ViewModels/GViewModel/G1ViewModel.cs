using CandySugar.Library.Common;
using Sdk.Component.Movie.sdk;
using Sdk.Component.Movie.sdk.ViewModel;
using Sdk.Component.Movie.sdk.ViewModel.Enums;
using Sdk.Component.Movie.sdk.ViewModel.Request;
using Sdk.Component.Movie.sdk.ViewModel.Response;
using System.Collections.ObjectModel;

namespace CandySugar.Controls;

internal class G1ViewModel : ViewModelBase
{
    readonly IService Service;
    public G1ViewModel(BaseServices baseServices) : base(baseServices)
    {
        Service = this.Container.Resolve<IService>();
    }

    public override void Initialize(INavigationParameters parameters)
    {
        Element = parameters.GetValue<GRootEntity>("Data");
        Task.Run(DetailInit);
    }

    #region Property
    public string Route { get; set; }
    public string PlayRoute { get; set; }
    #endregion

    #region Property
    GRootEntity _Element;
    public GRootEntity Element
    {
        get => _Element;
        set => SetProperty(ref _Element, value);
    }

    ObservableCollection<MovieDetailResult> _Result;
    public ObservableCollection<MovieDetailResult> Result
    {
        get => _Result;
        set => SetProperty(ref _Result, value);
    }
    #endregion

    #region Method
    async void DetailInit()
    {
        try
        {
            Activity = true;
            await Task.Delay(DataBus.Delay);
            var result = await MovieFactory.Movie(opt =>
            {
                opt.RequestParam = new Input
                {
                    CacheSpan = DataBus.Cache,
                    ImplType = DataCenter.ImplType(),
                    MovieType = MovieEnum.Detail,
                    Detail = new MovieDetail
                    {
                        Route = Element.Route
                    }
                };
            }).RunsAsync();
            Result = new ObservableCollection<MovieDetailResult>(result.DetailResults);
            SetState();
        }
        catch (Exception ex)
        {
            await Service.AddLog("G1DetailInit异常", ex);
            "F1DetailInit异常".OpenToast();
        }
    }
    async void WatchInit()
    {
        try
        {
            Activity = true;
            await Task.Delay(DataBus.Delay);
            var result = await MovieFactory.Movie(opt =>
            {
                opt.RequestParam = new Input
                {
                    CacheSpan = DataBus.Cache,
                    ImplType = DataCenter.ImplType(),
                    MovieType = MovieEnum.Watch,
                    Play = new MoviePlay
                    {
                        Route = Route
                    }
                };
            }).RunsAsync();
            if (result.PlayResult.Route.IsNullOrEmpty())
            {
                SetState();
                "当前播放地址无效,请更换其他线路!".OpenToast();
                return;
            }
            PlayRoute = result.PlayResult.Route;
            Navigation();
            SetState();
        }
        catch (Exception ex)
        {
            await Service.AddLog("G1WatchInit异常", ex);
            "F1WatchInit异常".OpenToast();
        }
    }
    async void Navigation()
    {
        Nav.NavigateAsync(new Uri("G2", UriKind.Relative), new NavigationParameters { { "Route", PlayRoute } });
    }
    async void Add()
    {
        await Service.GAdd(Element);
    }
    #endregion

    #region Command
    public DelegateCommand<string> PlayCommand => new(input =>
    {
        Add();
        this.Route = Route;
        Task.Run(WatchInit);
    });
    public DelegateCommand BackCommand => new(() =>
    {
        Nav.GoBackAsync();
    });
    #endregion
}
