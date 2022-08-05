using CandySugar.Controls.Views.MovieViews;
using Sdk.Component.Movie.sdk;
using Sdk.Component.Movie.sdk.ViewModel;
using Sdk.Component.Movie.sdk.ViewModel.Enums;
using Sdk.Component.Movie.sdk.ViewModel.Request;
using Sdk.Component.Movie.sdk.ViewModel.Response;

namespace CandySugar.Controls.ViewModels
{
    public class MovieViewModel : BaseViewModel
    {
        public MovieViewModel()
        {
            this.Page = 1;
            Task.Run(() => InitMovie());
        }

        #region 字段
        string CategoryRoute = string.Empty;
        bool LoadMore = false;
        #endregion

        #region 属性
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

        #region 方法
        async void InitMovie()
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
                        MovieType = MovieEnum.Init
                    };
                }).RunsAsync();
                CloseBusy();
                InitResult = new ObservableCollection<MovieInitResult>(result.InitResults);
            }
            catch (Exception ex)
            {
                StaticResource.PopToast(ex.Message);
            }
        }
        async void InitCatagory()
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
                        MovieType = MovieEnum.Category,
                        Category = new MovieCategory
                        {
                            Page = this.Page,
                            Route = this.CategoryRoute
                        }
                    };
                }).RunsAsync();
                CloseBusy();
                this.Total = result.RootResult.Total;
                if (LoadMore)
                    result.RootResult.ElementResults.ForEach(item =>
                    {
                        RootResult.Add(item);
                    });
                else
                    RootResult = new ObservableCollection<MovieElementResult>(result.RootResult.ElementResults);
            }
            catch (Exception ex)
            {
                StaticResource.PopToast(ex.Message);
            }

        }
        async void InitSearch()
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
                        MovieType = MovieEnum.Search,
                        Search = new MovieSearch
                        {
                            Page = this.Page,
                            KeyWord = this.KeyWord
                        }
                    };
                }).RunsAsync();
                CloseBusy();
                this.Total = result.RootResult.Total;
                if (LoadMore)
                    result.RootResult.ElementResults.ForEach(item =>
                    {
                        RootResult.Add(item);
                    });
                else
                    RootResult = new ObservableCollection<MovieElementResult>(result.RootResult.ElementResults);
            }
            catch (Exception ex)
            {
                StaticResource.PopToast(ex.Message);
            }
        }
        async void InitDetail(MovieElementResult input)
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
                        MovieType = MovieEnum.Detail,
                        Detail = new MovieDetail
                        {
                            Route = input.Route
                        }
                    };
                }).RunsAsync();
                CloseBusy();
                Navigation(input, result.DetailResults);
            }
            catch (Exception ex)
            {
                StaticResource.PopToast(ex.Message);
            }
        }
        async void Navigation(MovieElementResult query, List<MovieDetailResult> input)
        {
            await Shell.Current.GoToAsync(nameof(MovieDetailView), new Dictionary<string, object> { { "Data", input }, { "Query", query } });
        }
        #endregion

        #region 命令
        public DelegateCommand QueryAction => new(() =>
        {
            this.Page = 1;
            this.CategoryRoute = string.Empty;
            InitSearch();
        });
        public DelegateCommand RefreshAction => new(() =>
        {
            this.Page = 1;
            SetRefresh(false);
            this.LoadMore = false;
            if (!KeyWord.IsNullOrEmpty()) InitSearch();
            else InitCatagory();
        });
        public DelegateCommand LoadMoreAction => new(() =>
        {
            SetRefresh();
            if (Lock) return;
            this.Page += 1;
            if (this.Page > Total) return;
            LoadMore = true;
            if (KeyWord.IsNullOrEmpty()) InitCatagory();
            else InitSearch();
        });
        public DelegateCommand<string> CategoryAction => new(input =>
        {
            SetRefresh();
            this.Page = 1;
            this.CategoryRoute = input;
            this.KeyWord = string.Empty;
            this.LoadMore = false;
            InitCatagory();
        });
        public DelegateCommand<MovieElementResult> DetailAction => new(input =>
        {
            SetRefresh();
            InitDetail(input);
        });
        #endregion
    }
}
