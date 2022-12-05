using CandySugar.Library;
using CandySugar.Logic;
using Prism.Navigation;
using Prism.Navigation.Xaml;
using Sdk.Component.Anime.sdk;
using Sdk.Component.Anime.sdk.ViewModel;
using Sdk.Component.Anime.sdk.ViewModel.Enums;
using Sdk.Component.Anime.sdk.ViewModel.Request;
using Sdk.Component.Anime.sdk.ViewModel.Response;
using System.Collections.ObjectModel;

namespace CandySugar.Controls
{
    public class B1ViewModel : ViewModelBase
    {
        readonly IBService Service;

        public B1ViewModel(BaseServices baseServices, IBService service) : base(baseServices) { Service = service; }

        public override void Initialize(INavigationParameters parameters)
        {
            Route = parameters.GetValue<string>("Route");
            Task.Run(DetailInit);
        }

        #region Property
        public string Route { get; set; }
        public BRootEntity BRoot { get; set; }
        #endregion

        #region Property
        ObservableCollection<AnimeDetailResult> _Result;
        public ObservableCollection<AnimeDetailResult> Result
        {
            get => _Result;
            set => SetProperty(ref _Result, value);
        }
        string _Cover;
        public string Cover
        {
            get => _Cover;
            set => SetProperty(ref _Cover, value);
        }
        string _Name;
        public string Name
        {
            get => _Name;
            set => SetProperty(ref _Name, value);
        }
        #endregion

        #region Method
        async void DetailInit()
        {
            Activity = true;
            await Task.Delay(100);
            var result = await AnimeFactory.Anime(opt =>
            {
                opt.RequestParam = new Input
                {
                    CacheSpan = DataBus.Cache,
                    ImplType = DataCenter.ImplType(),
                    AnimeType = AnimeEnum.Detail,
                    Detail = new AnimeDetail
                    {
                        Route = Route
                    }
                };
            }).RunsAsync();
            Result = new ObservableCollection<AnimeDetailResult>(result.DetailResults.Where(t => t.IsDownURL == false));
            Name = Result.FirstOrDefault().Name;
            Cover = Result.FirstOrDefault().Cover;
            Add();
            Activity = false;
        }
        async void PlayInit(string Route, string Name)
        {
            Activity = true;
            await Task.Delay(100);
            var result = await AnimeFactory.Anime(opt =>
            {
                opt.RequestParam = new Input
                {
                    CacheSpan = DataBus.Cache,
                    ImplType = DataCenter.ImplType(),
                    AnimeType = AnimeEnum.Watch,
                    WatchPlay = new AnimeWatch
                    {
                        Route = Route,
                        CollectName = Name,
                    }
                };
            }).RunsAsync();
            Alter(Route);
            Navigation(result.PlayResult.PlayURL);
            Activity = false;
        }
        /// <summary>
        /// 写入数据库
        /// </summary>
        async void Add()
        {
            var Root = new BRootEntity
            {
                Cover = Cover,
                Name = Name,
                Collection = new List<BElementEntity>()
            };

            Result.ToList().ForEach(item =>
            {
                Root.Collection.Add(new BElementEntity
                {
                    CollectName = item.CollectName,
                    WatchRoute = item.WatchRoute,
                });
            });

            BRoot = await Service.Add(Root);
        }
        /// <summary>
        /// 修改数据库
        /// </summary>
        async void Alter(string Route)
        {
            var Element = BRoot.Collection.FirstOrDefault(t => t.WatchRoute == Route);
            await Service.Alter(Element);
        }
        void Navigation(string Route)
        {
            Nav.NavigateAsync(new Uri("B2", UriKind.Relative), new NavigationParameters { { "Route", Route } });
        }
        #endregion

        #region Command
        public DelegateCommand BackCommand => new(() =>
        {
            Nav.GoBackAsync();
        });
        public DelegateCommand<AnimeDetailResult> PlayCommand => new(input =>
        {
            PlayInit(input.WatchRoute, input.CollectName);
        });
        #endregion
    }
}
