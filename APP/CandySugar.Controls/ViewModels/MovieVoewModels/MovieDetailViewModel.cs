using CandySugar.Controls.Views.MovieViews;
using Sdk.Component.Movie.sdk;
using Sdk.Component.Movie.sdk.ViewModel;
using Sdk.Component.Movie.sdk.ViewModel.Enums;
using Sdk.Component.Movie.sdk.ViewModel.Request;
using Sdk.Component.Movie.sdk.ViewModel.Response;

namespace CandySugar.Controls.ViewModels.MovieVoewModels
{
    public class MovieDetailViewModel : BaseViewModel
    {
        public override void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            RootResult = query["Query"] as MovieElementResult;
            DetailResult = new ObservableCollection<MovieDetailResult>(query["Data"] as List<MovieDetailResult>);
        }

        #region 属性
        MovieElementResult _RootResult;
        public MovieElementResult RootResult
        {
            get => _RootResult;
            set => SetProperty(ref _RootResult, value);
        }
        ObservableCollection<MovieDetailResult> _DetailResult;
        public ObservableCollection<MovieDetailResult> DetailResult
        {
            get => _DetailResult;
            set => SetProperty(ref _DetailResult, value);
        }
        #endregion

        #region 方法
        async void InitWatch(string input)
        {
            if (IsBusy) return;
            try
            {
                if (CandySoft.NetState.NetworkAccess != NetworkAccess.Internet)
                {
                    StaticResource.PopToast("请检查网络是否通畅!");
                    return;
                }
                ShowBusy();
                await Task.Delay(CandySoft.Wait);
                var result = await MovieFactory.Movie(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        CacheSpan = CandySoft.Cache,
                        Proxy = StaticResource.Proxy(),
                        ImplType = StaticResource.ImplType(),
                        MovieType = MovieEnum.Watch,
                        Play = new MoviePlay
                        {
                            Route = input
                        }
                    };
                }).RunsAsync();
                CloseBusy();
                if (result.PlayResult.Route.IsNullOrEmpty())
                {
                    StaticResource.PopToast("当前播放地址无效,请更换其他线路!");
                    return;
                }
                Navigation(result.PlayResult.Route);
            }
            catch (Exception ex)
            {
                StaticResource.PopToast(ex.Message);
            }
        }
        async void Navigation(string input)
        {
            await Shell.Current.GoToAsync(nameof(MoviePlayView), new Dictionary<string, object> { { "Key", input }});
        }
        #endregion

        #region 命令
        public DelegateCommand<string> ViewAction => new(input =>
        {
            SetRefresh();
            InitWatch(input);
        });
        #endregion
    }
}
