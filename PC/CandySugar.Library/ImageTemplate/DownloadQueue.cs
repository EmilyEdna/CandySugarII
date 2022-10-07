using CandySugar.Resource.Properties;
using HandyControl.Controls;
using Sdk.Component.Axgle.sdk;
using Sdk.Component.Comic.sdk;
using Sdk.Component.Image.sdk;
using Sdk.Component.Image.sdk.ViewModel;
using Sdk.Component.Image.sdk.ViewModel.Enums;
using Sdk.Component.Image.sdk.ViewModel.Request;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Ax = Sdk.Component.Axgle.sdk.ViewModel;
using Co = Sdk.Component.Comic.sdk.ViewModel;

namespace CandySugar.Library.ImageTemplate
{
    public static class DownloadQueue
    {
        public delegate void ComplateDelegate(Image image, string route, BitmapSource bit);
        public static event ComplateDelegate OnComplate;
        private static Queue<QueueImageInfo> Stacks;
        private static AutoResetEvent AutoEvent;

        static DownloadQueue()
        {
            Stacks = new Queue<QueueImageInfo>();
            AutoEvent = new AutoResetEvent(true);
            (new Thread(new ThreadStart(DownloadImage))
            {
                IsBackground = true
            }).Start();
        }

        private static async void DownloadImage()
        {
            while (true)
            {
                QueueImageInfo Info = null;
                lock (Stacks)
                {
                    if (Stacks.Count > 0)
                    {
                        Info = Stacks.Dequeue();
                    }
                }
                if (Info != null)
                {
                    try
                    {
                        var bytes = await DownBytes(Info.Route, Info.Type);
                        var source = StaticResource.ToImage(bytes);
                        Info.ImageControl.Dispatcher.BeginInvoke(new Action<QueueImageInfo, BitmapSource>((img, bit) =>
                        {
                            if (OnComplate != null)
                            {
                                OnComplate(Info.ImageControl, Info.Route, bit);
                            }
                        }), new object[] { Info, source });
                    }
                    catch
                    {
                        Growl.Info("加载缓慢！请挂梯子食用！");
                        continue;
                    }
                }
                if (Stacks.Count > 0) continue;
                AutoEvent.WaitOne();
            }
        }
        /// <summary>
        /// Konachan
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static async Task<byte[]> DownBytes(string input, int type)
        {
            if (type == 1) return await Axgle(input);
            else if (type == 2) return await Konachan(input);
            else return await Comic(input);
        }
        #region Func
        private static async Task<byte[]> Konachan(string input)
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
        private static async Task<byte[]> Axgle(string input)
        {
            var AxgleCoverData = await AxgleFactory.Axgle(opt =>
             {
                 opt.RequestParam = new Ax.Input
                 {
                     CacheSpan = CandySoft.Default.Cache,
                     Proxy = StaticResource.Proxy(),
                     ImplType = StaticResource.ImplType(),
                     AxgleType = Ax.Enums.AxgleEnum.Cover,
                     Cover = new Ax.Request.AxgleCover
                     {
                         KeyWord = input
                     }
                 };
             }).RunsAsync();
            return AxgleCoverData.CoverResult.Bytes;
        }
        private static async Task<byte[]> Comic(string input)
        {
            var ComicData = await ComicFactory.Comic(opt =>
            {
                opt.RequestParam = new Co.Input
                {
                    CacheSpan = CandySoft.Default.Cache,
                    Proxy = StaticResource.Proxy(),
                    ImplType = StaticResource.ImplType(),
                    ComicType = Co.Enums.ComicEnum.Down,
                    ImageRoute = input
                };
            }).RunsAsync();
            return ComicData.ImageBytes;
        }
        #endregion
        public static void StartQueue(string Route, string Type, Image Control)
        {
            lock (Stacks)
            {
                int Category = Type.ToUpper().Equals("AXGLE") ? 1 : (Type.ToUpper().Equals("KONACHAN") ? 2 : 3);
                Stacks.Enqueue(new QueueImageInfo { Route = Route, Type = Category, ImageControl = Control });
                AutoEvent.Set();
            }
        }
    }
}
