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
            OnViewLoaded();
        }
        #region Property
        private ObservableCollection<NovelInitRecommendResult> _RecResult;
        public ObservableCollection<NovelInitRecommendResult> RecResult
        {
            get => _RecResult;
            set => SetAndNotify(ref _RecResult, value);
        }
        private ObservableCollection<NovelInitCategoryResult> _CateResult;
        public ObservableCollection<NovelInitCategoryResult> CateResult
        {
            get => _CateResult;
            set => SetAndNotify(ref _CateResult, value);
        }
        #endregion

        #region Override
        protected override void OnViewLoaded()
        {
            InitNovel();
        }
        #endregion

        #region Method
        public async void InitNovel()
        {
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
            RecResult = new ObservableCollection<NovelInitRecommendResult>(NovelInitData.RecResults);
            CateResult = new ObservableCollection<NovelInitCategoryResult>(NovelInitData.CateInitResults);
        }
        #endregion
    }
}
