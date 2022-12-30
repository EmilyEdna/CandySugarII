using Sdk.Component.Novel.sdk;
using Sdk.Component.Novel.sdk.ViewModel;
using Sdk.Component.Novel.sdk.ViewModel.Enums;
using Sdk.Component.Novel.sdk.ViewModel.Request;
using System.Collections.ObjectModel;

namespace CandySugar.Controls
{
    public class E2ViewModel : ViewModelBase
    {
        readonly IService Service;
        public E2ViewModel(BaseServices baseServices) : base(baseServices)
        {
            Service = this.Container.Resolve<IService>();
        }
        public override void Initialize(INavigationParameters parameters)
        {
            Route = parameters.GetValue<string>("Route");
            Task.Run(InitContent);
        }

        #region Property
        public string Route { get; set; }
        #endregion

        #region Property
        string _Title;
        public string Title
        {
            get => _Title;
            set => SetProperty(ref _Title, value);
        }
        ObservableCollection<string> _Content;
        public ObservableCollection<string> Content
        {
            get { return _Content; }
            set { SetProperty(ref _Content, value); }
        }
        string _Next;
        public string Next
        {
            get { return _Next; }
            set { SetProperty(ref _Next, value); }
        }
        #endregion

        #region Method
        async void InitContent()
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
                        NovelType = NovelEnum.View,
                        View = new NovelView
                        {
                            Route = Route
                        }
                    };
                }).RunsAsync();
                this.Title = result.ContentResult.ChapterName;

                Next = result.ContentResult.NextPage.IsNullOrEmpty() ? result.ContentResult.NextChapter : result.ContentResult.NextPage;
                result.ContentResult.Content = result.ContentResult.Content.Replace("　", "\t");

                if (this.Content == null)
                    this.Content = new ObservableCollection<string>();

                result.ContentResult.Content.Split("\t", StringSplitOptions.RemoveEmptyEntries).ForEnumerEach(t =>
                {
                    this.Content.Add("\t\t\t\t\t" + t + "\r\n");
                });
                SetState();

            }
            catch (Exception ex)
            {
                await Service.AddLog("E2InitContent异常", ex);
                "E2InitContent异常".OpenToast();
            }

        }
        #endregion

        #region Command
        public DelegateCommand BackCommand => new(() =>
        {
            Nav.GoBackAsync();
        });
        public DelegateCommand MoreCommand => new(() =>
        {
            if (Content.Count > 1000)
            {
                var temp = Content.ToList();
                temp.RemoveRange(0, 1000);
                Content = new ObservableCollection<string>(temp);
            }
            if (!Next.IsNullOrEmpty())
            {
                if (!Next.Equals(Route))
                {
                    Route = Next;
                    Task.Run(InitContent);
                }
            }
        });
        #endregion
    }
}
