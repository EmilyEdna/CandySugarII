﻿using Sdk.Component.Novel.sdk;
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
using XExten.Advance.LinqFramework;

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
            this.CategoryPage = 1;
            this.SearchVisible = false;
            this.DetailVisible = false;
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
        private bool _DetailVisible;
        public bool DetailVisible
        {
            get => _DetailVisible;
            set => SetAndNotify(ref _DetailVisible, value);
        }
        private bool _SearchVisible;
        public bool SearchVisible
        {
            get => _SearchVisible;
            set => SetAndNotify(ref _SearchVisible, value);
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

        #region CommomProperty_Int
        private int _CategoryPage;
        public int CategoryPage
        {
            get => _CategoryPage;
            set => SetAndNotify(ref _CategoryPage, value);
        }
        private int _CategoryTotal;
        public int CategoryTotal
        {
            get => _CategoryTotal;
            set => SetAndNotify(ref _CategoryTotal, value);
        }
        private int _DetailPage;
        public int DetailPage
        {
            get => _DetailPage;
            set => SetAndNotify(ref _DetailPage, value);
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
        private ObservableCollection<NovelCategoryElementResult> _CateElementResult;
        /// <summary>
        /// 分类结果
        /// </summary>
        public ObservableCollection<NovelCategoryElementResult> CateElementResult
        {
            get => _CateElementResult;
            set => SetAndNotify(ref _CateElementResult, value);
        }
        private NovelDetailRootResult _DetailResult;
        /// <summary>
        /// 详情结果
        /// </summary>
        public NovelDetailRootResult DetailResult
        {
            get => _DetailResult;
            set => SetAndNotify(ref _DetailResult, value);
        }
        private NovelContentResult _ViewResult;
        /// <summary>
        /// 内容结果
        /// </summary>
        public NovelContentResult ViewResult
        {
            get => _ViewResult;
            set => SetAndNotify(ref _ViewResult, value);
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
        private string DetailRoute;
        #endregion

        #region Action
        public void SearchAction(string input)
        {
            this.SearchVisible = true;
            InitSearch(input);
        }
        public void CategoryAction(string input)
        {
            this.CategoryRoute = input;
            this.SearchVisible = true;
            this.DetailVisible = false;
            InitCategory(input);
        }
        public void ViewAction(string input)
        {
            if (input.IsNullOrEmpty()) return;
            this.StepOne = false;
            this.StepTwo = true;
            InitView(input);
        }
        public void DetailAction(NovelCategoryElementResult input)
        {
            if (input == null) return;
            this.SearchVisible = false;
            this.DetailVisible = true;
            this.DetailPage = 1;
            DetailRoute = input.DetailRoute;
            InitDetail(DetailRoute);
        }
        public void PageDetailAction(FunctionEventArgs<int> input)
        {
            this.DetailPage = input.Info;
            InitDetail(DetailRoute);
        }
        public void PageCateAction(FunctionEventArgs<int> input)
        {
            this.CategoryPage = input.Info;
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

            CateElementResult = new ObservableCollection<NovelCategoryElementResult>(NovelSearchData.SearchResults.Select(t => new NovelCategoryElementResult
            {
                Author = t.Author,
                BookName = t.BookName,
                DetailRoute = t.DetailRoute,
                UpdateDate = t.UpdateDate,
            }));
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
                        Page = this.CategoryPage,
                        CategoryRoute = input,
                    }
                };
            }).RunsAsync();
            Loading = false;
            CategoryTotal = NovelCateData.CategoryResult.Total;
            CateElementResult = new ObservableCollection<NovelCategoryElementResult>(NovelCateData.CategoryResult.ElementResults);
        }
        private async void InitDetail(string input)
        {
            Loading = true;
            await Task.Delay(CandySoft.Default.WaitSpan);
            var NovelDetailData = await NovelFactory.Novel(opt =>
            {
                opt.RequestParam = new Input
                {
                    CacheSpan = CandySoft.Default.Cache,
                    Proxy = StaticResource.Proxy(),
                    ImplType = StaticResource.ImplType(),
                    NovelType = NovelEnum.Detail,
                    Detail = new NovelDetail
                    {
                        Page = this.DetailPage,
                        DetailRoute = input,
                    }
                };
            }).RunsAsync();
            Loading = false;
            DetailResult = NovelDetailData.DetailResult;
        }
        private async void InitView(string input)
        {
            Loading = true;
            await Task.Delay(CandySoft.Default.WaitSpan);
            var NovelViewData = await NovelFactory.Novel(opt =>
            {
                opt.RequestParam = new Input
                {
                    CacheSpan = CandySoft.Default.Cache,
                    Proxy = StaticResource.Proxy(),
                    ImplType = StaticResource.ImplType(),
                    NovelType = NovelEnum.View,
                    View = new NovelView
                    {
                        Route = input
                    }
                };
            }).RunsAsync();
            Loading = false;
            NovelViewData.ContentResult.Content = $"\t{NovelViewData.ContentResult.Content.Replace("　", "\n\t")}";
            ViewResult = NovelViewData.ContentResult;
        }
        #endregion
    }
}