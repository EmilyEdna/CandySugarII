using CandySugar.Library;
using CandySugar.Logic.Entity.CandyEntity;
using CandySugar.Logic.IService;
using CandySugar.Resource.Properties;
using HandyControl.Controls;
using HandyControl.Data;
using Sdk.Component.Lovel.sdk;
using Sdk.Component.Lovel.sdk.ViewModel;
using Sdk.Component.Lovel.sdk.ViewModel.Enums;
using Sdk.Component.Lovel.sdk.ViewModel.Request;
using Sdk.Component.Lovel.sdk.ViewModel.Response;
using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XExten.Advance.LinqFramework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CandySugar.Controls.ContentViewModel
{
    public class LovelViewModel : Screen
    {
        public IContainer Container;
        public IWindowManager WindowManager;
        public ICandyLovel CandyLovel;
        public LovelViewModel(IContainer Container, IWindowManager WindowManager)
        {
            this.WindowManager = WindowManager;
            this.Container = Container;
            this.CandyLovel = Container.Get<ICandyLovel>();
            this.CategoryPage = 1;
            this.StepOne = true;
            this.StepTwo = false;
            this.StepThree = false;
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
        private bool _StepThree;
        public bool StepThree
        {
            get => _StepThree;
            set => SetAndNotify(ref _StepThree, value);
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

        #region Property
        private ObservableCollection<LovelInitResult> _CateResult;
        /// <summary>
        /// 类别
        /// </summary>
        public ObservableCollection<LovelInitResult> CateResult
        {
            get => _CateResult;
            set => SetAndNotify(ref _CateResult, value);
        }
        private ObservableCollection<LovelCategoryElementResult> _CateElementResult;
        /// <summary>
        /// 分类结果
        /// </summary>
        public ObservableCollection<LovelCategoryElementResult> CateElementResult
        {
            get => _CateElementResult;
            set => SetAndNotify(ref _CateElementResult, value);
        }
        private ObservableCollection<LovelViewResult> _ViewResult;
        /// <summary>
        /// 章节结果
        /// </summary>
        public ObservableCollection<LovelViewResult> ViewResult
        {
            get => _ViewResult;
            set => SetAndNotify(ref _ViewResult, value);
        }
        private LovelContentResult _ContentResult;
        /// <summary>
        /// 内容结果
        /// </summary>
        public LovelContentResult ContentResult
        {
            get => _ContentResult;
            set => SetAndNotify(ref _ContentResult, value);
        }
        #endregion

        #region Override
        protected override void OnViewLoaded()
        {
            InitLovel();
        }
        #endregion

        #region Field
        private string CategoryRoute;
        private string DetailRoute;
        private string KeyWord;
        private Guid PrimaryId = Guid.Empty;
        #endregion

        #region Action
        public void SearchAction(string input)
        {
            this.KeyWord = input;
            this.CategoryRoute = string.Empty;
            InitSearch(input);
        }
        public void CategoryAction(string input)
        {
            this.CategoryPage = 1;
            this.CategoryRoute = input;
            this.KeyWord = string.Empty;
            InitCategory(input);
        }
        public void DetailAction(LovelCategoryElementResult input)
        {
            if (input == null) return;
            this.DetailRoute = input.DetailAddress;
            InitDetail(this.DetailRoute);
        }
        public void ContentAction(dynamic input)
        {
            if (input == null) return;
            if (input is LovelViewResult Lovel)
            {
                if (Lovel.IsDown) InitDown(Lovel);
                else InitContent(Lovel.ChapterRoute);
            }
            else
            {
                InitContent(input);
            }
        }
        public void PageCateAction(FunctionEventArgs<int> input)
        {
            this.CategoryPage = input.Info;
            if (!this.CategoryRoute.IsNullOrEmpty()) CategoryAction(this.CategoryRoute);
            else SearchAction(this.KeyWord);
        }
        public void HistoryAction()
        {
            StepOne = true;
            StepTwo = false;
            StepThree = false;
        }
        #endregion

        #region Method
        private async void InitLovel()
        {
            Loading = true;
            await Task.Delay(CandySoft.Default.WaitSpan);
            var LovelInitData = await LovelFactory.Lovel(opt =>
            {
                opt.RequestParam = new Input
                {
                    CacheSpan = CandySoft.Default.Cache,
                    Proxy = StaticResource.Proxy(),
                    ImplType = StaticResource.ImplType(),
                    LovelType = LovelEnum.Init,
                    Login = new LovelLogin
                    {
                        Account = CandySoft.Default.WA,
                        Password = CandySoft.Default.WP
                    }
                };
            }).RunsAsync();
            Loading = false;
            CateResult = new ObservableCollection<LovelInitResult>(LovelInitData.InitResults);
        }
        private async void InitCategory(string input)
        {
            Loading = true;
            await Task.Delay(CandySoft.Default.WaitSpan);
            var LovelCateData = await LovelFactory.Lovel(opt =>
            {
                opt.RequestParam = new Input
                {
                    CacheSpan = CandySoft.Default.Cache,
                    Proxy = StaticResource.Proxy(),
                    ImplType = StaticResource.ImplType(),
                    LovelType = LovelEnum.Category,
                    Category = new LovelCategory
                    {
                        Page = CategoryPage,
                        Route = input
                    }
                };
            }).RunsAsync();
            Loading = false;
            CategoryTotal = LovelCateData.CategoryResult.Total;
            CateElementResult = new ObservableCollection<LovelCategoryElementResult>(LovelCateData.CategoryResult.ElementResults);
        }
        private async void InitDetail(string input)
        {
            Loading = true;
            await Task.Delay(CandySoft.Default.WaitSpan);
            var LovelDetailData = await LovelFactory.Lovel(opt =>
            {
                opt.RequestParam = new Input
                {
                    CacheSpan = CandySoft.Default.Cache,
                    Proxy = StaticResource.Proxy(),
                    ImplType = StaticResource.ImplType(),
                    LovelType = LovelEnum.Detail,
                    Detail = new LovelDetail
                    {
                        Route = input
                    }
                };
            }).RunsAsync();
            await Task.Delay(CandySoft.Default.WaitSpan);
            var LovelViewData = await LovelFactory.Lovel(opt =>
            {
                opt.RequestParam = new Input
                {
                    CacheSpan = CandySoft.Default.Cache,
                    Proxy = StaticResource.Proxy(),
                    ImplType = StaticResource.ImplType(),
                    LovelType = LovelEnum.View,
                    View = new LovelView
                    {
                        Route = LovelDetailData.DetailResult.Route
                    }
                };
            }).RunsAsync();
            Loading = false;
            ViewResult = new ObservableCollection<LovelViewResult>(LovelViewData.ViewResult);
        }
        private async void InitDown(LovelViewResult input)
        {
            var LovelDownData = await LovelFactory.Lovel(opt =>
            {
                opt.RequestParam = new Input
                {
                    CacheSpan = CandySoft.Default.Cache,
                    Proxy = StaticResource.Proxy(),
                    ImplType = StaticResource.ImplType(),
                    LovelType = LovelEnum.Download,
                    Down = new LovelDown
                    {
                        BookName = input.BookName,
                        UId = input.ChapterRoute.AsInt()
                    }
                };
            }).RunsAsync();
            StaticResource.Download(LovelDownData.DownResult.Bytes, "Lovel", input.BookName, "txt");
        }
        private async void InitContent(string input)
        {
            Loading = true;
            await Task.Delay(CandySoft.Default.WaitSpan);
            var LovelContentData = await LovelFactory.Lovel(opt =>
            {
                opt.RequestParam = new Input
                {
                    CacheSpan = CandySoft.Default.Cache,
                    Proxy = StaticResource.Proxy(),
                    ImplType = StaticResource.ImplType(),
                    LovelType = LovelEnum.Content,
                    Content = new LovelContent
                    {
                        ChapterRoute = input
                    }
                };
            }).RunsAsync();

            if (LovelContentData.ContentResult.Content.Equals("因版权问题，文库不再提供该小说的阅读！"))
            {
                Growl.Info("因版权问题，不再提供该小说的阅读");
                return;
            }
            if (LovelContentData.ContentResult.Image.IsNullOrEmpty())
            {
                StepOne = false;
                StepTwo = false;
                StepThree = true;
            }
            else
            {
                StepOne = false;
                StepTwo = true;
                StepThree = false;
            }
            Logic(input);
            Loading = false;
            ContentResult = LovelContentData.ContentResult;
        }
        private async void InitSearch(string input)
        {
            Loading = true;
            await Task.Delay(CandySoft.Default.WaitSpan);
            var LovelQueryData = await LovelFactory.Lovel(opt =>
            {
                opt.RequestParam = new Input
                {
                    CacheSpan = CandySoft.Default.Cache,
                    Proxy = StaticResource.Proxy(),
                    ImplType = StaticResource.ImplType(),
                    LovelType = LovelEnum.Search,
                    Search = new LovelSearch
                    {
                        KeyWord = input,
                        SearchType = LovelSearchEnum.ArticleName,
                        Page = CategoryPage
                    }
                };
            }).RunsAsync();
            Loading = false;
            CategoryTotal = LovelQueryData.SearchResult.Total;
            CateElementResult = new ObservableCollection<LovelCategoryElementResult>(LovelQueryData.SearchResult.ElementResults.ToMapest<List<LovelCategoryElementResult>>());
        }
        private async void AddLovel(CandyLovel input)
        {
            await this.CandyLovel.AddOrUpdate(input);
        }
        protected void Logic(string input)
        {
            var Views = ViewResult?.FirstOrDefault(t => t.ChapterRoute == input);
            var Infos = CateElementResult?.FirstOrDefault(t => t.BookName == Views.BookName);
            if (Views != null && Infos != null)
            {
                var Entity = Infos.ToMapest<CandyLovel>();
                Entity.BookType = Infos.Category;
                Entity.Route = input;
                Entity.Chapter = Views.ChapterName;
                AddLovel(Entity);
            }
        }
        #endregion
    }
}
