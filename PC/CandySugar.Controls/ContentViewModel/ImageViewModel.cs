using CandySugar.Library;
using CandySugar.Logic.Entity.CandyEntity;
using CandySugar.Logic.IService;
using CandySugar.Resource.Properties;
using HandyControl.Controls;
using HandyControl.Data;
using HandyControl.Tools.Command;
using Sdk.Component.Image.sdk;
using Sdk.Component.Image.sdk.ViewModel;
using Sdk.Component.Image.sdk.ViewModel.Enums;
using Sdk.Component.Image.sdk.ViewModel.Request;
using Sdk.Component.Image.sdk.ViewModel.Response;
using Serilog;
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
        public ICandyLabel CandyLabel;
        public ICandyImage CandyImage;
        public int Limit = 12;
        public ImageViewModel(IContainer Container, IWindowManager WindowManager)
        {
            this.WindowManager = WindowManager;
            this.Container = Container;
            this.CandyLabel = Container.Get<ICandyLabel>();
            this.CandyImage = Container.Get<ICandyImage>();
            this.StepOne = true;
            this.StepTwo = false;
            this.Page = 1;
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
        #endregion

        #region 整型
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

        #region 属性
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
        private string _Key;
        public string Key
        {
            get => _Key;
            set => SetAndNotify(ref _Key, value);
        }
        #endregion

        #region 重写
        protected override void OnViewLoaded()
        {
            InitImage();
        }
        #endregion

        #region 字段
        private string KeyWord;
        #endregion

        #region 命令
        public ICommand HistoryAction => new RelayCommand((obj) =>
        {
            this.StepOne = true;
            this.StepTwo = false;
        });

        public async void SearchAction(string input)
        {
            this.KeyWord = await this.CandyLabel.GetKey(input);
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
            this.Key = input.Id.ToString();
            InitBytes(input.OriginalPng.IsNullOrEmpty() ? input.OriginalJepg : input.OriginalPng);
        }

        public void SaveAction(ImageElementResult input)
        {
            Logic(input);
        }
        #endregion

        #region 方法
        private async void InitImage()
        {
            try
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
                this.Loading = false;
                ElementResult = new ObservableCollection<ImageElementResult>(ImageInitData.GlobalResult.Result);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "");
                HandyControl.Controls.Growl.Error("服务异常");
            }
        }
        private async void InitSearch(string input)
        {
            try
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
                Loading = false;
                ElementResult = new ObservableCollection<ImageElementResult>(ImageQueryData.GlobalResult.Result);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "");
                HandyControl.Controls.Growl.Error("服务异常");
            }
        }
        private async void InitBytes(string input)
        {
            try
            {
                this.StepOne = false;
                this.StepTwo = true;
                this.Loading = true;
                await Task.Delay(CandySoft.Default.WaitSpan);
                var bytes = await ImageFactory.Image(opt =>
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
                this.Loading = false;
                var width = (int)(CandySoft.Default.ScreenWidth - 200);
                var height = (int)(CandySoft.Default.ScreenHeight - 30);
                Bitmap = StaticResource.ToImage(bytes.DownResult.Bytes, width, height);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "");
                HandyControl.Controls.Growl.Error("服务异常");
            }
        }
        private async void Logic(ImageElementResult input)
        {
            var tmep = await this.CandyImage.Add(new CandyImage
            {
                Original = input.OriginalPng.IsNullOrEmpty() ? input.OriginalJepg : input.OriginalPng,
                Preview = input.Preview
            });
            if (tmep) Growl.Info("Success");
            else Growl.Warning("已经添加过了！");
        }
        #endregion
    }
}
