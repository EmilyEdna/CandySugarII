using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using XExten.Advance.CacheFramework;
using XExten.Advance.LinqFramework;

namespace CandySugar.Library.PropertyQueue
{
    public static class DownloadQueue
    {
        public delegate void ComplateDelegate(Image image, string route, ImageSource bit);
        public static event ComplateDelegate OnComplate;
        private static Queue<QueueImageInfo> Stacks;
        private static AutoResetEvent AutoEvent;
        static DownloadQueue()
        {
            Stacks = new Queue<QueueImageInfo>();
            AutoEvent = new AutoResetEvent(true);
            new Thread(new ThreadStart(DownloadImage))
            {
                IsBackground = true
            }.Start();
        }

        private async static void DownloadImage()
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
                        var bytes = await Comic(Info.Route);
                        var source = ImageSource.FromStream(() => bytes);
                        Info.Control.Dispatcher.DispatchAsync(() =>
                        {
                            OnComplate(Info.Control, Info.Route, source);
                        });
                    }
                    catch
                    {
                        StaticUnitl.PopToast("图片加载失败");
                    }
                }
                if (Stacks.Count > 0) continue;
                AutoEvent.WaitOne();
            }
        }

        private static async Task<Stream> Comic(string input)
        {
            var Key = input.ToMd5();
            var data = await Caches.RunTimeCacheGetAsync<byte[]>(Key);
            if (data != null && data.Length > 0)
            {
                var result = new MemoryStream(data);
                result.Dispose();
                result.Close();
                return result;

            }
            else
            {
                HttpClient Client = new HttpClient();
                Client.DefaultRequestHeaders.Add("Host", "i.nhentai.net");
                var bytes = await Client.GetByteArrayAsync(input);
                await Caches.RunTimeCacheSetAsync(Key, bytes, CandySoft.Cache);
                var result = new MemoryStream(bytes);
                result.Dispose();
                result.Close();
                return result;
            }
        }
        public static void StartQueue(Image Control, string Route)
        {
            lock (Stacks)
            {
                Stacks.Enqueue(new QueueImageInfo { Control = Control, Route = Route });
                AutoEvent.Set();
            }
        }
    }
}
