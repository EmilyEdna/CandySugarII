using CandySugar.Com.Library.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using XExten.Advance.CacheFramework;
using XExten.Advance.LinqFramework;

namespace CandySugar.Com.Library.DownQueue
{
    public class DownloadRequest
    {
        /// <summary>
        /// 队列下载
        /// </summary>
        public static void DownByQueue()
        {
            DownloadQueue.ResultFunc = new(Reuqest);
        }

        private static async Task<byte[]> Reuqest(string route, Enum @enum)
        {
            if (@enum is EDownload EType)
            {
                switch (EType)
                {
                    case EDownload.Light:
                        return await Light(route);
                    default:
                        break;
                }
            }
            return null;
        }

        private static async Task<byte[]> Light(string route)
        {
            var key = route.ToMd5();
            var cache = await Caches.RunTimeCacheGetAsync<byte[]>(key);
            if (cache != null)
                return cache;
            HttpClient client = new HttpClient();
            var res = await client.GetByteArrayAsync(route);
            await Caches.RunTimeCacheSetAsync(key, res);
            return res;
        }
    }
}
