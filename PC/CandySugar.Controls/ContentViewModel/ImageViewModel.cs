using CandySugar.Library;
using CandySugar.Resource.Properties;
using HandyControl.Data;
using HandyControl.Tools.Command;
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
using System.Windows.Input;
using System.Windows.Media.Imaging;
using XExten.Advance.LinqFramework;

namespace CandySugar.Controls.ContentViewModel
{
    public class ImageViewModel : Screen
    {
        public IContainer Container;
        public IWindowManager WindowManager;
        public int Limit = 12;
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
        private BitmapSource _Bitmap;
        public BitmapSource Bitmap
        {
            get => _Bitmap;
            set => SetAndNotify(ref _Bitmap, value);
        }
        #endregion

        #region Override
        protected override void OnViewLoaded()
        {
            InitImage();
        }
        #endregion

        #region Field
        private string KeyWord;
        #endregion

        #region Action
        public ICommand HistoryAction => new RelayCommand((obj) =>
        {
            this.StepOne = true;
            this.StepTwo = false;
        });

        public void SearchAction(string input)
        {
            this.KeyWord = input;
            this.Page = 1;
            InitSearch(input);
        }
        public void PageAction(FunctionEventArgs<int> input)
        {
            this.Page = input.Info;
            InitSearch(this.KeyWord);
        }

        public void ViewAction(ImageElementResult input)
        {
            InitBytes(input.OriginalPng.IsNullOrEmpty() ? input.OriginalJepg : input.OriginalPng);
        }
        #endregion

        #region Method
        private async void InitImage()
        {
            this.Loading = true;
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
                        Page = Page,
                        Limit = Limit,
                        Tag = StaticResource.ImageModule()
                    }
                };
            }).RunsAsync();

            Total = ImageInitData.GlobalResult.Total;
            ImageInitData.GlobalResult.Result.ForEach(async item =>
            {
                item.Runtime = StaticResource.ToImage(await DownBytes(item.Preview)); ;
            });
            this.Loading = false;
            ElementResult = new ObservableCollection<ImageElementResult>(ImageInitData.GlobalResult.Result);
        }
        private async void InitSearch(string input)
        {
            Loading = true;
            await Task.Delay(CandySoft.Default.WaitSpan);
            var ImageQueryData = await ImageFactory.Image(opt =>
            {
                opt.RequestParam = new Input
                {
                    CacheSpan = CandySoft.Default.Cache,
                    Proxy = StaticResource.Proxy(),
                    ImplType = StaticResource.ImplType(),
                    ImageType = ImageEnum.Search,
                    Search = new ImageSearch
                    {
                        Page = Page,
                        Limit = Limit,
                        KeyWord = $"{input} {StaticResource.ImageModule()}"
                    }
                };
            }).RunsAsync();

            Total = ImageQueryData.GlobalResult.Total;
            ImageQueryData.GlobalResult.Result.ForEach(async item =>
            {
                item.Runtime = StaticResource.ToImage(await DownBytes(item.Preview)); ;
            });
            Loading = false;
            ElementResult = new ObservableCollection<ImageElementResult>(ImageQueryData.GlobalResult.Result);
        }
        private async void InitBytes(string input)
        {
            this.StepOne = false;
            this.StepTwo = true;
            this.Loading = true;
            await Task.Delay(CandySoft.Default.WaitSpan);
            var bytes = await DownBytes(input);
            this.Loading = false;
            var width = (int)(CandySoft.Default.ScreenWidth - 200);
            var height = (int)(CandySoft.Default.ScreenHeight - 30);
            Bitmap = StaticResource.ToImage(bytes, width, height);
        }
        #endregion

        #region Util
        private async Task<byte[]> DownBytes(string input)
        {
            var ImageInitData = await ImageFactory.Image(opt =>
            {
                opt.RequestParam = new Input
                {
                    CacheSpan = CandySoft.Default.Cache,
                    Proxy = StaticResource.Proxy(),
                    ImplType = StaticResource.ImplType(),
                    ImageType = ImageEnum.Download,
                    Download = new ImageDownload
                    {
                        Route = input
                    }
                };
            }).RunsAsync();
            return ImageInitData.DownResult.Bytes;
        }
        #endregion
    }
}
