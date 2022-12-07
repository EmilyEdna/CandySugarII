using CandySugar.Library;
using XExten.Advance.CacheFramework;
using Sdk.Component.Plugins;
using XExten.Advance.LinqFramework;

namespace CandySugar.Controls
{
    public class DataCenter
    {

        /// <summary>
        /// Sdk请求方式
        /// </summary>
        /// <returns></returns>
        public static SdkImpl ImplType()
        {
            if (DataBus.QueryModule == 1) return SdkImpl.Multi;
            else if (DataBus.QueryModule == 2) return SdkImpl.Rest;
            else if (DataBus.QueryModule == 3) return SdkImpl.RPC;
            else return SdkImpl.User;
        }
        /// <summary>
        /// 图片方法
        /// </summary>
        /// <returns></returns>
        public static string ImageType()
        {

            if (DataBus.Module == 1) return string.Empty;
            else if (DataBus.Module == 2) return $"rating:safe";
            else if (DataBus.Module == 3) return $"rating:questionable";
            else return $"rating:explicit";
        }
        public static void RegistFunc()
        {
            ImageDep.Funcs = new(async (key, type) =>
            {
                if (type == 1)
                {
                    var result = Caches.RunTimeCacheGet<byte[]>(key.ToMd5());
                    if (result != null) return result;
                    HttpClient client = new();
                    client.DefaultRequestHeaders.Add("Host", "konachan.com");
                    var data = await client.GetByteArrayAsync(key);
                    Caches.RunTimeCacheSet(key.ToMd5(), data, DataBus.Cache);
                    return data;
                }
                if (type == 2)
                {

                }
                return null;
            });
        }
    }
}
