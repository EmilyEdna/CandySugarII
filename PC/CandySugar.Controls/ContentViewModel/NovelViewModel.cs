using Sdk.Component.Novel.sdk;
using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sdk.Component.Novel.sdk.ViewModel;
using CandySugar.Resource.Properties;
using CandySugar.Library;
using Sdk.Component.Novel.sdk.ViewModel.Enums;
using System.Collections.ObjectModel;
using Sdk.Component.Novel.sdk.ViewModel.Response;
using Sdk.Component.Novel.sdk.ViewModel.Request;
using HandyControl.Data;

namespace CandySugar.Controls.ContentViewModel
{
    public class NovelViewModel : Screen
    {
        public IContainer Container;
        public IWindowManager WindowManager;
        public NovelViewModel(IContainer Container, IWindowManager WindowManager)
        {
            this.WindowManager = WindowManager;
            this.Container = Container;
            this.Page = 1;
            OnViewLoaded();
        }

        #region CommomProperty
        private bool _Loading;
        public bool Loading
        {
            get => _Loading;
            set => SetAndNotify(ref _Loading, value);
        }
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
        #endregion

        #region Property
        private ObservableCollection<NovelInitRecommendResult> _RecResult;
        /// <summary>
        /// 首页推荐
        /// </summary>
        public ObservableCollection<NovelInitRecommendResult> RecResult
        {
            get => _RecResult;
            set => SetAndNotify(ref _RecResult, value);
        }
        private ObservableCollection<NovelInitCategoryResult> _CateResult;
        /// <summary>
        /// 首页分类
        /// </summary>
        public ObservableCollection<NovelInitCategoryResult> CateResult
        {
            get => _CateResult;
            set => SetAndNotify(ref _CateResult, value);
        }
        private ObservableCollection<NovelSearchResult> _QuqeryResult;
        /// <summary>
        /// 检索结果
        /// </summary>
        public ObservableCollection<NovelSearchResult> QuqeryResult
        {
            get => _QuqeryResult;
            set => SetAndNotify(ref _QuqeryResult, value);
        }
        private ObservableCollection<NovelCategoryElementResult> _CateElementResult;
        /// <summary>
        /// 分类结果
        /// </summary>
        public ObservableCollection<NovelCategoryElementResult> CateElementResult
        {
            get => _CateElementResult;
            set => SetAndNotify(ref _CateElementResult, value);
        }
        #endregion

        #region Override
        protected override void OnViewLoaded()
        {
            InitNovel();
        }
        #endregion

        #region Field
        private string CategoryRoute;
        #endregion

        #region Action
        public void SearchAction(string input) 
        {
            InitSearch(input);
        }
        public void CategoryAction(string input)
        {
            CategoryRoute = input;
            InitCategory(input);
        }

        public void PageCateAction(FunctionEventArgs<int> input)
        {
            Page = input.Info;
            CategoryAction(CategoryRoute);
        }
        #endregion

        #region Method
        private async void InitNovel()
        {
            Loading = true;
            await Task.Delay(CandySoft.Default.WaitSpan);
            var NovelInitData = await NovelFactory.Novel(opt =>
               {
                   opt.RequestParam = new Input
                   {
                       CacheSpan = CandySoft.Default.Cache,
                       Proxy = StaticResource.Proxy(),
                       ImplType = StaticResource.ImplType(),
                       NovelType = NovelEnum.Init
                   };
               }).RunsAsync();
            Loading = false;
            RecResult = new ObservableCollection<NovelInitRecommendResult>(NovelInitData.RecResults);
            CateResult = new ObservableCollection<NovelInitCategoryResult>(NovelInitData.CateInitResults);
        }
        private async void InitSearch(string input)
        {
            Loading = true;
            await Task.Delay(CandySoft.Default.WaitSpan);
            var NovelSearchData = await NovelFactory.Novel(opt =>
            {
                opt.RequestParam = new Input
                {
                    CacheSpan = CandySoft.Default.Cache,
                    Proxy = StaticResource.Proxy(),
                    ImplType = StaticResource.ImplType(),
                    NovelType = NovelEnum.Search,
                    Search = new NovelSearch
                    {
                        KeyWord = input
                    }
                };
            }).RunsAsync();
            Loading = false;
            QuqeryResult = new ObservableCollection<NovelSearchResult>(NovelSearchData.SearchResults);
        }
        private async void InitCategory(string input)
        {
            Loading = true;
            await Task.Delay(CandySoft.Default.WaitSpan);
            var NovelCateData = await NovelFactory.Novel(opt =>
            {
                opt.RequestParam = new Input
                {
                    CacheSpan = CandySoft.Default.Cache,
                    Proxy = StaticResource.Proxy(),
                    ImplType = StaticResource.ImplType(),
                    NovelType = NovelEnum.Category,
                    Category = new NovelCategory
                    {
                        Page = Page,
                        CategoryRoute = input,
                    }
                };
            }).RunsAsync();
            Loading = false;
            Total = NovelCateData.CategoryResult.Total;
            CateElementResult = new ObservableCollection<NovelCategoryElementResult>(NovelCateData.CategoryResult.ElementResults);
        }
        #endregion
    }
}
