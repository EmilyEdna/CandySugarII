using CandySugar.Library;
using CandySugar.Library.Template;
using CandySugar.Resource.Properties;
using HandyControl.Data;
using Microsoft.Web.WebView2.Wpf;
using Sdk.Component.Anime.sdk;
using Sdk.Component.Anime.sdk.ViewModel;
using Sdk.Component.Anime.sdk.ViewModel.Enums;
using Sdk.Component.Anime.sdk.ViewModel.Request;
using Sdk.Component.Anime.sdk.ViewModel.Response;
using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XExten.Advance.LinqFramework;

namespace CandySugar.Controls.ContentViewModel
{
    public class AnimeViewModel : Screen
    {
        public IContainer Container;
        public IWindowManager WindowManager;
        public WebView2 WebView { get; set; }
        public AnimeViewModel(IContainer Container, IWindowManager WindowManager)
        {
            this.WindowManager = WindowManager;
            this.Container = Container;
            this.Chars = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z".Split(",").ToList();
            this.CategoryPage = 1;
            this.StepOne = true;
            this.StepTwo = false;
            OnViewLoaded();
        }

        #region CommomProperty_Bool
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

        #region ComomProperty_Int
        private int _CategoryTotal;
        public int CategoryTotal
        {
            get => _CategoryTotal;
            set => SetAndNotify(ref _CategoryTotal, value);
        }
        private int _CategoryPage;
        public int CategoryPage
        {
            get => _CategoryPage;
            set => SetAndNotify(ref _CategoryPage, value);
        }
        #endregion

        #region Field
        private Dictionary<object, object> CateKeyWord;
        private string KeyWord;
        #endregion

        #region Property
        public List<string> Chars { get; set; }
        private ObservableCollection<AnimeWeekDayIndexResult> _DayResult;
        /// <summary>
        /// 初始化结果
        /// </summary>
        public ObservableCollection<AnimeWeekDayIndexResult> DayResult
        {
            get => _DayResult;
            set => SetAndNotify(ref _DayResult, value);
        }
        private ObservableCollection<AnimeSearchElementResult> _SearchResult;
        /// <summary>
        /// 检索分类结果
        /// </summary>
        public ObservableCollection<AnimeSearchElementResult> SearchResult
        {
            get => _SearchResult;
            set => SetAndNotify(ref _SearchResult, value);
        }
        private ObservableCollection<AnimeDetailResult> _DetailResult;
        /// <summary>
        /// 详情结果
        /// </summary>
        public ObservableCollection<AnimeDetailResult> DetailResult
        {
            get => _DetailResult;
            set => SetAndNotify(ref _DetailResult, value);
        }
        private AnimePlayResult _PlayResult;
        /// <summary>
        /// 播放结果
        /// </summary>
        public AnimePlayResult PlayResult
        {
            get => _PlayResult;
            set => SetAndNotify(ref _PlayResult, value);
        }

        #endregion

        #region Override
        protected override void OnViewLoaded()
        {
            InitAnime();
        }
        #endregion

        #region Action
        public void SearchAction(string input)
        {
            this.KeyWord = input;
            this.CateKeyWord = null;
            InitSearch(input);
        }
        public void CategoryAction(Dictionary<object, object> input)
        {
            this.CategoryPage = 1;
            this.CateKeyWord = input;
            this.KeyWord = string.Empty;
            InitCategory(input);
        }
        public void PageCateAction(FunctionEventArgs<int> input)
        {
            this.CategoryPage = input.Info;
            if (this.CateKeyWord != null && this.KeyWord.IsNullOrEmpty()) InitCategory(this.CateKeyWord);
            if (this.CateKeyWord == null && !this.KeyWord.IsNullOrEmpty()) InitSearch(KeyWord);
        }
        public void DetailAction(AnimeSearchElementResult input)
        {
            if (input == null) return;
            InitDetail(input.Route);
        }
        public void WatchAction(AnimeDetailResult input)
        {
            InitWatch(input);
        }

        #endregion

        #region Method
        private async void InitAnime()
        {
            Loading = true;
            await Task.Delay(CandySoft.Default.WaitSpan);
            var AnimeInitData = AnimeFactory.Anime(opt =>
            {
                opt.RequestParam = new Input
                {
                    CacheSpan = CandySoft.Default.Cache,
                    Proxy = StaticResource.Proxy(),
                    ImplType = StaticResource.ImplType(),
                    AnimeType = AnimeEnum.Init
                };
            }).RunsAsync().Result;
            Loading = false;
            DayResult = new ObservableCollection<AnimeWeekDayIndexResult>(AnimeInitData.RecResults);
        }
        private async void InitSearch(string input)
        {
            Loading = true;
            await Task.Delay(CandySoft.Default.WaitSpan);
            var AnimeQueryData = AnimeFactory.Anime(opt =>
            {
                opt.RequestParam = new Input
                {
                    CacheSpan = CandySoft.Default.Cache,
                    Proxy = StaticResource.Proxy(),
                    ImplType = StaticResource.ImplType(),
                    AnimeType = AnimeEnum.Search,
                    Search = new AnimeSearch
                    {
                        KeyWord = input,
                        Page = CategoryPage
                    }
                };
            }).RunsAsync().Result;
            Loading = false;
            CategoryTotal = AnimeQueryData.SeachResult.Total;
            SearchResult = new ObservableCollection<AnimeSearchElementResult>(AnimeQueryData.SeachResult.ElementResult);
        }
        private async void InitCategory(Dictionary<object, object> input)
        {
            var key = input.Keys.FirstOrDefault().ToString();
            var val = input.Values.FirstOrDefault().ToString();
            Loading = true;
            await Task.Delay(CandySoft.Default.WaitSpan);
            var AnimeCateData = AnimeFactory.Anime(opt =>
            {
                opt.RequestParam = new Input
                {
                    CacheSpan = CandySoft.Default.Cache,
                    Proxy = StaticResource.Proxy(),
                    ImplType = StaticResource.ImplType(),
                    AnimeType = key.Equals("Char") ? AnimeEnum.Category : AnimeEnum.CategoryType,
                    Category = key.Equals("Char") ? new AnimeCategory
                    {
                        Route = val,
                        LetterType = Enum.Parse<AnimeLetterEnum>(val),
                        Page = CategoryPage
                    } : new AnimeCategory
                    {
                        Route = val,
                        Page = CategoryPage
                    }
                };
            }).RunsAsync().Result;
            Loading = false;
            CategoryTotal = AnimeCateData.SeachResult.Total;
            SearchResult = new ObservableCollection<AnimeSearchElementResult>(AnimeCateData.SeachResult.ElementResult);
        }
        private async void InitDetail(string input)
        {
            Loading = true;
            await Task.Delay(CandySoft.Default.WaitSpan);
            var AnimeDetailData = AnimeFactory.Anime(opt =>
            {
                opt.RequestParam = new Input
                {
                    CacheSpan = CandySoft.Default.Cache,
                    Proxy = StaticResource.Proxy(),
                    ImplType = StaticResource.ImplType(),
                    AnimeType = AnimeEnum.Detail,
                    Detail = new AnimeDetail
                    {
                        Route = input
                    }
                };
            }).RunsAsync().Result;
            Loading = false;
            DetailResult = new ObservableCollection<AnimeDetailResult>(AnimeDetailData.DetailResults.Where(t => t.IsDownURL == false));
        }
        private async void InitWatch(AnimeDetailResult input)
        {
            this.StepTwo = true;
            this.StepOne = false;
            Loading = true;
            await Task.Delay(CandySoft.Default.WaitSpan);
            var AnimeWatchData = AnimeFactory.Anime(opt =>
            {
                opt.RequestParam = new Input
                {
                    CacheSpan = CandySoft.Default.Cache,
                    Proxy = StaticResource.Proxy(),
                    ImplType = StaticResource.ImplType(),
                    AnimeType = AnimeEnum.Watch,
                    WatchPlay = new AnimeWatch
                    {
                        CollectName = input.CollectName,
                        Route = input.WatchRoute
                    }
                };
            }).RunsAsync().Result;
            Loading = false;
            PlayResult = AnimeWatchData.PlayResult;

            await WebView.CoreWebView2.ExecuteScriptAsync($"Play('{PlayResult.PlayURL}','{CandySoft.Default.ScreenHeight - 30}')");
        }
        #endregion
    }
}
