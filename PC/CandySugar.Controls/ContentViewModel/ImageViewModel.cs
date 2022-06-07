using CandySugar.Library;
using CandySugar.Resource.Properties;
using Sdk.Component.Image.sdk;
using Sdk.Component.Image.sdk.ViewModel;
using Sdk.Component.Image.sdk.ViewModel.Enums;
using Sdk.Component.Image.sdk.ViewModel.Request;
using Sdk.Component.Image.sdk.ViewModel.Response;
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
    public class ImageViewModel : Screen
    {
        public IContainer Container;
        public IWindowManager WindowManager;
        public int Limit = 5;
        public ImageViewModel(IContainer Container, IWindowManager WindowManager)
        {
            this.WindowManager = WindowManager;
            this.Container = Container;
            this.StepOne = true;
            this.StepTwo = false;
            this.Page = 1;
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
        private int _Total;
        public int Total
        {
            get => _Total;
            set => SetAndNotify(ref _Total, value);
        }
        private int _Page;
        public int Page
        {
            get => _Page;
            set => SetAndNotify(ref _Page, value);
        }
        #endregion

        #region Property
        private ObservableCollection<ImageElementResult> _ElementResult;
        public ObservableCollection<ImageElementResult> ElementResult
        {
            get => _ElementResult;
            set => SetAndNotify(ref _ElementResult, value);
        }
        #endregion

        #region Override
        protected override void OnViewLoaded()
        {
            InitImage();
        }
        #endregion

        #region Action
        public void SearchAction(string input)
        {

        }
        #endregion

        #region Method

        private async void InitImage()
        {
            Loading = true;
            await Task.Delay(CandySoft.Default.WaitSpan);
            var ImageInitData = await ImageFactory.Image(opt =>
            {
                opt.RequestParam = new Input
                {
                    CacheSpan = CandySoft.Default.Cache,
                    Proxy = StaticResource.Proxy(),
                    ImplType = StaticResource.ImplType(),
                    ImageType = ImageEnum.Init,
                    Init = new ImageInit
                    {
                        Page=Page,
                        Limit= Limit,
                        Tag= StaticResource.ImageModule()
                    }
                };
            }).RunsAsync();
            Loading = false;
            Total = ImageInitData.GlobalResult.Total;
            ElementResult = new ObservableCollection<ImageElementResult>(ImageInitData.GlobalResult.Result);
        }
        #endregion
    }
}
