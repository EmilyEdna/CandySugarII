using CandySugar.Library;
using Prism.Navigation;
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
        public B1ViewModel(BaseServices baseServices) : base(baseServices) { }

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
            Activity = false;
        }
        #endregion

        #region Command
        public DelegateCommand BackCommand => new(() =>
        {
            Nav.GoBackAsync();
        });
        public DelegateCommand<AnimeDetailResult> PlayCommand => new(input => { 
        
        });
        #endregion
    }
}
