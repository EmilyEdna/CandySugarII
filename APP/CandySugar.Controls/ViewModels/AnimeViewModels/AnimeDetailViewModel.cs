using CandySugar.Controls.Views.AnimeViews;
using Sdk.Component.Anime.sdk;
using Sdk.Component.Anime.sdk.ViewModel;
using Sdk.Component.Anime.sdk.ViewModel.Enums;
using Sdk.Component.Anime.sdk.ViewModel.Request;
using Sdk.Component.Anime.sdk.ViewModel.Response;

namespace CandySugar.Controls.ViewModels.AnimeViewModels
{
    public class AnimeDetailViewModel : BaseViewModel
    {
        ICandyService CandyService;
        public AnimeDetailViewModel()
        {
            CandyService = CandyContainer.Instance.Resolve<ICandyService>();
        }

        public override void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            var data = (List<AnimeDetailResult>)query["Route"];
            Name = data.FirstOrDefault().Name;
            Cover = data.FirstOrDefault().Cover;
            Query = new ObservableCollection<AnimeDetailResult>(data);
        }

        #region 属性
        ObservableCollection<AnimeDetailResult> _Qeury;
        public ObservableCollection<AnimeDetailResult> Query
        {
            get => _Qeury;
            set => SetProperty(ref _Qeury, value);
        }
        string _Name;
        public string Name
        {
            get => _Name;
            set => SetProperty(ref _Name, value);
        }
        string _Cover;
        public string Cover
        {
            get => _Cover;
            set => SetProperty(ref _Cover, value);
        }
        #endregion

        #region 命令
        public DelegateCommand<AnimeDetailResult> ViewAction => new(input => InitPlay(input));
        #endregion

        #region 方法
        async void InitPlay(AnimeDetailResult input)
        {
            if (IsBusy) return;
            try
            {
                if (CandySoft.NetState.NetworkAccess != NetworkAccess.Internet)
                {
                    await Shell.Current.DisplayAlert("网络异常！", "请检查网络是否通畅！", "是");
                    return;
                }
                ShowBusy();
                await Task.Delay(CandySoft.Wait);
                var result = await AnimeFactory.Anime(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        CacheSpan = CandySoft.Cache,
                        Proxy = StaticResource.Proxy(),
                        ImplType = StaticResource.ImplType(),
                        AnimeType = AnimeEnum.Watch,
                        WatchPlay = new AnimeWatch
                        {
                            Route = input.WatchRoute,
                            CollectName = input.CollectName,
                        }
                    };
                }).RunsAsync();
                Logic(input, result.PlayResult.PlayURL);
                CloseBusy();
                Navigation(result.PlayResult.PlayURL);
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("错误！", ex.Message, "是");
            }
        }

        async void Navigation(string input)
        {
            await Shell.Current.GoToAsync($"{nameof(AnimePlayView)}?Key={input}");
        }

        void Logic(AnimeDetailResult input, string PlayRoute)
        {
            var Model = input.ToMapest<CandyAnimeRoot>();
            Model.Route = PlayRoute;
            Model.Elements = Query.Select(t => new CandyAnimeElement
            {
                AnimeName=Model.Name,
                Name = t.CollectName,
                Route = t.WatchRoute
            }).ToList();
            CandyService.AddOrAlterAnime(Model);
        }
        #endregion
    }
}
