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
        readonly IService Service;

        public B1ViewModel(BaseServices baseServices) : base(baseServices)
        {
            Service = this.Container.Resolve<IService>();
        }

        public override void Initialize(INavigationParameters parameters)
        {
            Route = parameters.GetValue<string>("Route");
            Task.Run(DetailInit);
        }

        #region Property
        public string Route { get; set; }
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
            try
            {
                Activity = true;
                await Task.Delay(DataBus.Delay);
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
                SetState();
            }
            catch (Exception ex)
            {
                await Service.AddLog("B1DetailInit异常", ex);
                "B1DetailInit异常".OpenToast();
            }
        }
        async void PlayInit(string Route, string Name)
        {
            try
            {
                Activity = true;
                await Task.Delay(DataBus.Delay);
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
                Add(result.PlayResult.All);
                Navigation(result.PlayResult.PlayURL, result.PlayResult.InnerPlayer);
                SetState();
            }
            catch (Exception ex)
            {
                await Service.AddLog("B1PlayInit异常", ex);
                "B1PlayInit异常".OpenToast();
            }
        }
        /// <summary>
        /// 写入数据库
        /// </summary>
        async void Add(Dictionary<string,string> Map)
        {
            var Root = new BRootEntity
            {
                Cover = Cover,
                Name = Name,
                Children = new List<BElementEntity>()
            };

            foreach (var item in Map)
            {
                Root.Children.Add(new BElementEntity
                {
                    CollectName = item.Key,
                    WatchRoute = item.Value,
                    Inner = item.Value.Contains("m3u8.php") ? false : true
                });
            }

          await Service.BAdd(Root);
        }
        void Navigation(string Route,bool Inner)
        {
            Nav.NavigateAsync(new Uri("B2", UriKind.Relative), new NavigationParameters { { "Route", Route },{ "Inner", Inner } });
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
