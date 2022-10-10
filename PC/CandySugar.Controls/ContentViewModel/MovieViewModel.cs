using CandySugar.Library;
using CandySugar.Logic.Entity.CandyEntity;
using CandySugar.Logic.IService;
using CandySugar.Resource.Properties;
using HandyControl.Controls;
using HandyControl.Data;
using Microsoft.Web.WebView2.Wpf;
using Sdk.Component.Movie.sdk;
using Sdk.Component.Movie.sdk.ViewModel;
using Sdk.Component.Movie.sdk.ViewModel.Enums;
using Sdk.Component.Movie.sdk.ViewModel.Request;
using Sdk.Component.Movie.sdk.ViewModel.Response;
using Stylet;
using StyletIoC;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using XExten.Advance.LinqFramework;

namespace CandySugar.Controls.ContentViewModel
{
    public class MovieViewModel : Screen
    {
        public IContainer Container;
        public IWindowManager WindowManager;
        public ICandyMovie CandyMovie;
        public WebView2 WebView { get; set; }
        public MovieViewModel(IContainer Container, IWindowManager WindowManager)
        {
            this.WindowManager = WindowManager;
            this.Container = Container;
            this.CandyMovie = Container.Get<ICandyMovie>();
            this.Page = 1;
            this.StepOne = true;
            this.StepTwo = false;
            OnViewLoaded();
        }
        #region 字段
        private string KeyWord;
        private string CateRoute;
        private MovieElementResult CurrentResult;
        #endregion

        #region 布尔
        private bool _Loading;
        public bool Loading
        {
            get => _Loading;
            set => SetAndNotify(ref _Loading, value);
        }
        private bool _StepOne;
        public bool StepOne
        {
            get => _StepOne;
            set => SetAndNotify(ref _StepOne, value);
        }
        private bool _StepTwo;
        public bool StepTwo
        {
            get => _StepTwo;
            set => SetAndNotify(ref _StepTwo, value);
        }
        #endregion

        #region 整型
        private int _Page;
        public int Page
        {
            get => _Page;
            set => SetAndNotify(ref _Page, value);
        }
        private int _Total;
        public int Total
        {
            get => _Total;
            set => SetAndNotify(ref _Total, value);
        }
        private int _SearchId;
        public int SearchId
        {
            get => _SearchId;
            set => SetAndNotify(ref _SearchId, value);
        }
        #endregion

        #region 属性
        private ObservableCollection<MovieInitResult> _InitResult;
        public ObservableCollection<MovieInitResult> InitResult
        {
            get => _InitResult;
            set => SetAndNotify(ref _InitResult, value);
        }
        private ObservableCollection<MovieElementResult> _EleResult;
        public ObservableCollection<MovieElementResult> EleResult
        {
            get => _EleResult;
            set => SetAndNotify(ref _EleResult, value);
        }
        private ObservableCollection<MovieDetailResult> _DetailResult;
        public ObservableCollection<MovieDetailResult> DetailResult
        {
            get => _DetailResult;
            set => SetAndNotify(ref _DetailResult, value);
        }
        #endregion

        #region 命令
        public void CategoryAction(string input)
        {
            CateRoute = input;
            this.Page = 1;
            InitCategory(input);
        }
        public void LinkAction(MovieElementResult input)
        {
            CurrentResult = input;
            InitWatch(input.Route);
        }
        public void PlayAction(MovieDetailResult input)
        {
            InitPlay(input.Route);
        }
        public void SearchAction(string input)
        {
            this.SearchId = 0;
            this.KeyWord = input;
            InitQuery(input);
        }
        public void PageAction(FunctionEventArgs<int> input)
        {
            if (CateRoute.IsNullOrEmpty())
            {
                this.Page = input.Info;
                InitQuery(KeyWord);
            }
            else
            {
                this.Page = input.Info;
                InitCategory(CateRoute);
            }
        }
        public void WatchAction(string input)
        {
            Play(input);
        }
        #endregion

        #region 重写
        protected override void OnViewLoaded()
        {
            InitMovie();
        }
        #endregion

        #region 方法
        private async void InitMovie()
        {
            this.Loading = true;
            await Task.Delay(CandySoft.Default.WaitSpan);
            var InitData = await MovieFactory.Movie(opt =>
             {
                 opt.RequestParam = new Input
                 {
                     CacheSpan = CandySoft.Default.Cache,
                     Proxy = StaticResource.Proxy(),
                     ImplType = StaticResource.ImplType(),
                     MovieType = MovieEnum.Init,
                 };
             }).RunsAsync();
            this.Loading = false;
            InitResult = new ObservableCollection<MovieInitResult>(InitData.InitResults);
        }
        private async void InitPlay(string input)
        {
            this.Loading = true;
            await Task.Delay(CandySoft.Default.WaitSpan);
            var InitPlay = await MovieFactory.Movie(opt =>
            {
                opt.RequestParam = new Input
                {
                    CacheSpan = CandySoft.Default.Cache,
                    Proxy = StaticResource.Proxy(),
                    ImplType = StaticResource.ImplType(),
                    MovieType = MovieEnum.Watch,
                    Play = new MoviePlay
                    {
                        Route = input
                    }
                };
            }).RunsAsync();
            this.Loading = false;
            if (InitPlay.PlayResult.Route.IsNullOrEmpty())
            {
                Growl.Info("当前播放地址无效,请更换其他线路!");
                return;
            }
            Logic(InitPlay.PlayResult.Route);
            Play(InitPlay.PlayResult.Route);
        }
        private async void InitCategory(string input)
        {
            this.Loading = true;
            await Task.Delay(CandySoft.Default.WaitSpan);
            var InitCate = await MovieFactory.Movie(opt =>
            {
                opt.RequestParam = new Input
                {
                    CacheSpan = CandySoft.Default.Cache,
                    Proxy = StaticResource.Proxy(),
                    ImplType = StaticResource.ImplType(),
                    MovieType = MovieEnum.Category,
                    Category = new MovieCategory
                    {
                        Page = this.Page,
                        Route = input
                    }
                };
            }).RunsAsync();
            this.Loading = false;
            this.Total = InitCate.RootResult.Total;
            this.EleResult = new ObservableCollection<MovieElementResult>(InitCate.RootResult.ElementResults);
        }
        private async void InitWatch(string input)
        {
            this.Loading = true;
            await Task.Delay(CandySoft.Default.WaitSpan);
            var InitDetail = await MovieFactory.Movie(opt =>
            {
                opt.RequestParam = new Input
                {
                    CacheSpan = CandySoft.Default.Cache,
                    Proxy = StaticResource.Proxy(),
                    ImplType = StaticResource.ImplType(),
                    MovieType = MovieEnum.Detail,
                    Detail = new MovieDetail
                    {
                        Route = input
                    }
                };
            }).RunsAsync();
            this.Loading = false;
            DetailResult = new ObservableCollection<MovieDetailResult>(InitDetail.DetailResults);
        }
        private async void InitQuery(string input)
        {
            this.Loading = true;
            await Task.Delay(CandySoft.Default.WaitSpan);
            var InitSearch = await MovieFactory.Movie(opt =>
            {
                opt.RequestParam = new Input
                {
                    CacheSpan = CandySoft.Default.Cache,
                    Proxy = StaticResource.Proxy(),
                    ImplType = StaticResource.ImplType(),
                    MovieType = MovieEnum.Search,
                    Search = new MovieSearch
                    {
                        Page = this.Page,
                        KeyWord = input,
                        SearchId = this.SearchId
                    }
                };
            }).RunsAsync();
            this.Loading = false;
            this.Total = InitSearch.RootResult.Total;
            this.SearchId = InitSearch.RootResult.SearchId;
            this.EleResult = new ObservableCollection<MovieElementResult>(InitSearch.RootResult.ElementResults);
        }
        private async void Logic(string input)
        {
            await CandyMovie.AddOrUpdate(new CandyMovie
            {
                Route = input,
                Cover = CurrentResult.Cover,
                Title = CurrentResult.Title
            });
        }
        private async void Play(string input)
        {
            this.StepTwo = true;
            this.StepOne = false;
            await WebView.CoreWebView2.ExecuteScriptAsync($"Play('{input}','{CandySoft.Default.ScreenHeight - 30}')");
        }
        #endregion
    }
}
