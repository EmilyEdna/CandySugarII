using Sdk.Component.Manga.sdk;
using Sdk.Component.Manga.sdk.ViewModel;
using Sdk.Component.Manga.sdk.ViewModel.Enums;
using Sdk.Component.Manga.sdk.ViewModel.Request;
using Sdk.Component.Manga.sdk.ViewModel.Response;
using System.Collections.ObjectModel;

namespace CandySugar.Controls
{
    public class D1ViewModel : ViewModelBase
    {
        readonly IService Service;
        public D1ViewModel(BaseServices baseServices) : base(baseServices)
        {
            Service = this.Container.Resolve<IService>();
        }

        public override void Initialize(INavigationParameters parameters)
        {
            Route = parameters.GetValue<string>("Route");
            Cover = parameters.GetValue<string>("Cover");
            Task.Run(InitDetail);
        }

        #region Property
        public string Cover { get; set; }
        #endregion

        #region Property
        string _Route;
        public string Route
        {
            get => _Route;
            set => SetProperty(ref _Route, value);
        }
        ObservableCollection<MangaChapterDetailResult> _Result;
        public ObservableCollection<MangaChapterDetailResult> Result
        {
            get => _Result;
            set => SetProperty(ref _Result, value);
        }
        #endregion

        #region Method
        async void InitDetail()
        {
            try
            {
                Activity = true;
                await Task.Delay(DataBus.Delay);
                var result = await MangaFactory.Manga(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        CacheSpan = DataBus.Cache,
                        ImplType = DataCenter.ImplType(),
                        MangaType = MangaEnum.Detail,
                        Detail = new MangaDetail
                        {
                            Route = Route
                        }
                    };
                }).RunsAsync();
                Result = new ObservableCollection<MangaChapterDetailResult>(result.ChapterResults);
                SetState();
            }
            catch (Exception ex)
            {
                await Service.AddLog("D1DetailInit异常", ex);
                "D1DetailInit异常".OpenToast();
            }
        }
        async void Add()
        {
            var Root = new DRootEntity
            {
                Cover = Cover,
                Name = Result.FirstOrDefault().Name,
            };
            Root.Children = Result.Select(t => new DElementEntity
            {
                Route = t.Route,
                Title = t.Title
            }).ToList();
            await Service.DAdd(Root);
        }
        #endregion

        #region Command
        public DelegateCommand BackCommand => new(() =>
        {
            Nav.GoBackAsync();
        });
        public DelegateCommand<MangaChapterDetailResult> WatchCommand => new(input =>
        {
            Add();
            Nav.NavigateAsync(new Uri("D2", UriKind.Relative), new NavigationParameters { { "Route", input.Route }, { "Title", input.Title } });
        });
        #endregion
    }
}
