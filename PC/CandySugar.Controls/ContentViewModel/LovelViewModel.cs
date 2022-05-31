using CandySugar.Library;
using CandySugar.Resource.Properties;
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
            OnViewLoaded();
        }

        #region CommomProperty_Bool
        private bool _Loading;
        public bool Loading
        {
            get => _Loading;
            set => SetAndNotify(ref _Loading, value);
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
        private LovelCategoryRootResult _CategoryResult;
        /// <summary>
        /// 分类结果
        /// </summary>
        public LovelCategoryRootResult CategoryResult
        {
            get => _CategoryResult;
            set => SetAndNotify(ref _CategoryResult, value);
        }
        #endregion

        #region Override
        protected override void OnViewLoaded()
        {
            InitLovel();
        }
        #endregion

        #region Action
        public void CategoryAction(string input) 
        {
            InitCategory(input);
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
            var LovelInitData = await LovelFactory.Lovel(opt =>
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
            CategoryTotal = LovelInitData.CategoryResult.Total;
            CategoryResult =LovelInitData.CategoryResult;
        }
        #endregion
    }
}
