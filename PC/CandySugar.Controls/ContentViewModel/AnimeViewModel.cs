using CandySugar.Library;
using CandySugar.Library.Template;
using CandySugar.Logic.Entity.CandyEntity;
using CandySugar.Logic.IService;
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
        public ICandyAnime CandyAnime;
        public WebView2 WebView { get; set; }
        public AnimeViewModel(IContainer Container, IWindowManager WindowManager)
        {
            this.WindowManager = WindowManager;
            this.Container = Container;
            this.CandyAnime = Container.Get<ICandyAnime>();
            this.CategoryPage = 1;
            this.Switch = true;
            this.StepOne = true;
            this.StepTwo = false;
            OnViewLoaded();
        }

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
        private bool _Switch;
        public bool Switch
        {
            get => _Switch;
            set => SetAndNotify(ref _Switch, value);
        }
        #endregion

        #region 整型
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

        #region 字段
        public string KeyWord;
        private Dictionary<object, object> CateKeyWord;
        private AnimeLetterEnum Letter = AnimeLetterEnum.全部;
        private AnimeTypeEnum Types = AnimeTypeEnum.全部;
        private AnimeAreaEnum Areas = AnimeAreaEnum.全部;
        private string Years = string.Empty;
        #endregion

        #region 属性
        private ObservableCollection<string> _Chars;
        public ObservableCollection<string> Chars
        {
            get => _Chars;
            set => SetAndNotify(ref _Chars, value);
        }
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
        /// <summary>
        /// 初始化结果
        /// </summary>
        private AnimeInitResult _InitResult;
        public AnimeInitResult InitResult
        {
            get => _InitResult;
            set => SetAndNotify(ref _InitResult, value);
        }
        #endregion

        #region 重写
        protected override void OnViewLoaded()
        {
            InitAnime();
        }
        #endregion

        #region 命令
        public void RadioAction(Dictionary<object, object> input)
        {
            if (input.Keys.FirstOrDefault().ToString().Equals("Letter"))
                Letter = Enum.Parse<AnimeLetterEnum>(input.Values.FirstOrDefault().ToString());
            else if (input.Keys.FirstOrDefault().ToString().Equals("Types"))
                Types = Enum.Parse<AnimeTypeEnum>(input.Values.FirstOrDefault().ToString());
            else if (input.Keys.FirstOrDefault().ToString().Equals("Years"))
                Years = input.Values.FirstOrDefault().ToString().Equals("全部") ? string.Empty : input.Values.FirstOrDefault().ToString();
            else Areas = Enum.Parse<AnimeAreaEnum>(input.Values.FirstOrDefault().ToString());
            this.KeyWord = string.Empty;
            InitCategory();
        }
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
            if (Switch)
            {
                if (this.CateKeyWord != null && this.KeyWord.IsNullOrEmpty()) InitCategory(this.CateKeyWord);
                if (this.CateKeyWord == null && !this.KeyWord.IsNullOrEmpty()) InitSearch(KeyWord);
            }
            else
            {
                if (this.KeyWord.IsNullOrEmpty()) InitCategory();
                else InitSearch(KeyWord);
            }
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

        #region 方法
        public async void InitAnime()
        {
            this.Loading = true;
            await Task.Delay(CandySoft.Default.WaitSpan);
            var AnimeInitData = await AnimeFactory.Anime(opt =>
            {
                opt.RequestParam = new Input
                {
                    CacheSpan = CandySoft.Default.Cache,
                    Proxy = StaticResource.Proxy(),
                    ImplType = StaticResource.ImplType(),
                    SourceType = Switch ? AnimeSourceEnum.SBDM : AnimeSourceEnum.YSJDM,
                    AnimeType = AnimeEnum.Init
                };
            }).RunsAsync();
            this.Loading = false;
            InitResult = AnimeInitData.InitResult;
            this.Chars = new ObservableCollection<string>(AnimeInitData.InitResult.Letters.Where(t => !t.Equals("全部")));
            DayResult = new ObservableCollection<AnimeWeekDayIndexResult>(AnimeInitData.InitResult.RecResults ?? new List<AnimeWeekDayIndexResult>());
        }
        private async void InitSearch(string input)
        {
            this.Loading = true;
            await Task.Delay(CandySoft.Default.WaitSpan);
            var AnimeQueryData = await AnimeFactory.Anime(opt =>
            {
                opt.RequestParam = new Input
                {
                    CacheSpan = CandySoft.Default.Cache,
                    Proxy = StaticResource.Proxy(),
                    ImplType = StaticResource.ImplType(),
                    AnimeType = AnimeEnum.Search,
                    SourceType = Switch ? AnimeSourceEnum.SBDM : AnimeSourceEnum.YSJDM,
                    Search = new AnimeSearch
                    {
                        KeyWord = input,
                        Page = CategoryPage
                    }
                };
            }).RunsAsync();
            this.Loading = false;
            CategoryTotal = AnimeQueryData.SeachResult.Total;
            SearchResult = new ObservableCollection<AnimeSearchElementResult>(AnimeQueryData.SeachResult.ElementResult);
        }
        private async void InitCategory(Dictionary<object, object> input)
        {
            var key = input.Keys.FirstOrDefault().ToString();
            var val = input.Values.FirstOrDefault().ToString();
            this.Loading = true;
            await Task.Delay(CandySoft.Default.WaitSpan);
            var AnimeCateData = await AnimeFactory.Anime(opt =>
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
            }).RunsAsync();
            this.Loading = false;
            CategoryTotal = AnimeCateData.SeachResult.Total;
            SearchResult = new ObservableCollection<AnimeSearchElementResult>(AnimeCateData.SeachResult.ElementResult);
        }
        private async void InitCategory()
        {
            this.Loading = true;
            await Task.Delay(CandySoft.Default.WaitSpan);
            var AnimeCateData = await AnimeFactory.Anime(opt =>
            {
                opt.RequestParam = new Input
                {
                    CacheSpan = CandySoft.Default.Cache,
                    Proxy = StaticResource.Proxy(),
                    ImplType = StaticResource.ImplType(),
                    AnimeType = AnimeEnum.Category,
                    SourceType = AnimeSourceEnum.YSJDM,
                    Category = new AnimeCategory
                    {

                        LetterType = Letter,
                        Area = Areas,
                        Type = Types,
                        Year = Years,
                        Page = CategoryPage
                    }
                };
            }).RunsAsync();
            this.Loading = false;
            CategoryTotal = AnimeCateData.SeachResult.Total;
            SearchResult = new ObservableCollection<AnimeSearchElementResult>(AnimeCateData.SeachResult.ElementResult);
        }
        private async void InitDetail(string input)
        {
            Loading = true;
            await Task.Delay(CandySoft.Default.WaitSpan);
            var AnimeDetailData = await AnimeFactory.Anime(opt =>
            {
                opt.RequestParam = new Input
                {
                    CacheSpan = CandySoft.Default.Cache,
                    Proxy = StaticResource.Proxy(),
                    SourceType = Switch ? AnimeSourceEnum.SBDM : AnimeSourceEnum.YSJDM,
                    ImplType = StaticResource.ImplType(),
                    AnimeType = AnimeEnum.Detail,
                    Detail = new AnimeDetail
                    {
                        Route = input
                    }
                };
            }).RunsAsync();
            this.Loading = false;
            DetailResult = new ObservableCollection<AnimeDetailResult>(AnimeDetailData.DetailResults.Where(t => t.IsDownURL == false));
        }
        private async void InitWatch(AnimeDetailResult input)
        {
            this.StepTwo = true;
            this.StepOne = false;
            this.Loading = true;
            await Task.Delay(CandySoft.Default.WaitSpan);
            var AnimeWatchData = await AnimeFactory.Anime(opt =>
            {
                opt.RequestParam = new Input
                {
                    CacheSpan = CandySoft.Default.Cache,
                    Proxy = StaticResource.Proxy(),
                    ImplType = StaticResource.ImplType(),
                    SourceType = Switch ? AnimeSourceEnum.SBDM : AnimeSourceEnum.YSJDM,
                    AnimeType = AnimeEnum.Watch,
                    WatchPlay = new AnimeWatch
                    {
                        CollectName = input.CollectName,
                        Route = input.WatchRoute
                    }
                };
            }).RunsAsync();
            this.Loading = false;
            PlayResult = AnimeWatchData.PlayResult;
            Logic(input);
            await WebView.CoreWebView2.ExecuteScriptAsync($"Play('{PlayResult.PlayURL}','{CandySoft.Default.ScreenHeight - 30}')");
        }
        private async void Logic(AnimeDetailResult input)
        {
            CandyAnimeRoot Root = await this.CandyAnime.AddOrUpdateRoot(new CandyAnimeRoot
            {
                AnimeName = input.Name,
                CollectName = input.CollectName,
                Cover = input.Cover,
                Route = input.WatchRoute
            });
            if (DetailResult != null)
            {
                List<CandyAnimeElement> Elements = DetailResult.Where(t => t.Name == input.Name).Select(t => new CandyAnimeElement
                {
                    Name = t.CollectName,
                    RootId = Root.CandyId,
                    Route = t.WatchRoute,
                }).ToList();
                await this.CandyAnime.AddElement(Elements);
            }
        }
        #endregion
    }
}
