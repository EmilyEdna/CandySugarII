using CandySugar.Resource.Properties;
using HandyControl.Controls;
using Sdk.Component.Image.sdk;
using Sdk.Component.Image.sdk.ViewModel;
using Sdk.Component.Image.sdk.ViewModel.Enums;
using Sdk.Component.Image.sdk.ViewModel.Request;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

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
                        var bytes = await DownBytes(Info.Route);
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
        private static async Task<byte[]> DownBytes(string input)
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

        public static void StartQueue(string Route, Image Control)
        {
            lock (Stacks)
            {
                Stacks.Enqueue(new QueueImageInfo { Route = Route, ImageControl = Control });
                AutoEvent.Set();
            }
        }
    }
}
