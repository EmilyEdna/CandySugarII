using CandySugar.Library;
using CandySugar.Resource.Properties;
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

namespace CandySugar.Controls.ContentViewModel
{
    public class LovelViewModel : Screen
    {
        public IContainer Container;
        public IWindowManager WindowManager;
        public LovelViewModel(IContainer Container, IWindowManager WindowManager)
        {
            this.WindowManager = WindowManager;
            this.Container = Container;
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
        #endregion

        #region Action
        public void CategoryAction(string input) 
        {
            this.CategoryRoute = input;
            InitCategory(input);
        }

        public void DetailAction(LovelCategoryElementResult input)
        {
            this.DetailRoute = input.DetailAddress;
            InitDetail(this.DetailRoute);
        }
        public void ContentAction(LovelViewResult input)
        { 
                    
        }
        public void PageCateAction(FunctionEventArgs<int> input)
        {
            this.CategoryPage = input.Info;
            CategoryAction(CategoryRoute);
        }
        #endregion

        #region Method
        private async void InitLovel() 
        {
            Loading = true;
            await Task.Delay(CandySoft.Default.WaitSpan);
            var LovelInitData =  LovelFactory.Lovel(opt =>
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
            }).RunsAsync().Result;
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
        #endregion
    }
}
