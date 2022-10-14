using Microsoft.Maui;
using System.IO;
using XExten.Advance.CacheFramework;
using XExten.Advance.LinqFramework;
using XExten.Advance.StaticFramework;

namespace CandySugar.Library.PropertyAttach
{
    public static class ImageDependencyProperty
    {
        #region 委托
        public static event ComplateDelegate OnComplate;
        public delegate void ComplateDelegate(Image image, string route, ImageSource bit);
        public static ActivityIndicator Indicator;
        #endregion
        static ImageDependencyProperty()
        {
            OnComplate += new ComplateDelegate(OnDownloadComplateEvent);
        }

        #region 字段

        public static string GetSource(BindableObject obj)
        {
            return (string)obj.GetValue(SourceProperty);
        }

        public static void SetSource(BindableObject obj, string value)
        {
            obj.SetValue(SourceProperty, value);
        }

        public static readonly BindableProperty SourceProperty =
            BindableProperty.CreateAttached("Source", typeof(string), typeof(ImageDependencyProperty), null, BindingMode.TwoWay, null, OnPropertyChanged);
        #endregion

        #region 方法
        private async static void OnPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            try
            {
                var ctrl = (Image)bindable;
                Indicator = ((ctrl.Parent as Grid).Children.LastOrDefault() as ActivityIndicator);
                Indicator.IsRunning = true;
                new Thread(new ParameterizedThreadStart(DownloadImage))
                {
                    IsBackground = true
                }.Start(new Dictionary<string, Image> { { newValue.ToString(), ctrl } });
            }
            catch (Exception)
            {
                StaticUnitl.PopToast("图片加载失败");
            }

        }
        private async static void DownloadImage(object obj)
        {
            try
            {
                if (obj is Dictionary<string, Image> input)
                {
                    var bytes = await Comic(input.Keys.FirstOrDefault());
                    if (bytes == null || bytes.Length == 0) return;
                    var source = ImageSource.FromStream(() => new MemoryStream(bytes));
                    input.Values.FirstOrDefault().Dispatcher.DispatchAsync(() =>
                    {
                        OnComplate(input.Values.FirstOrDefault(), input.Keys.FirstOrDefault(), source);
                    });
                }
            }
            catch (Exception)
            {
                StaticUnitl.PopToast("图片加载失败");
            }
        }

        private static async Task<byte[]> Comic(string input)
        {
            try
            {
                var Key = input.ToMd5();
                var data = await Caches.RunTimeCacheGetAsync<byte[]>(Key);
                if (data != null && data.Length > 0) return data;
                else
                {
                    HttpClient Client = new HttpClient();
                    Client.DefaultRequestHeaders.Add("Host", "i.nhentai.net");
                    var bytes = await Client.GetByteArrayAsync(input);
                    await Caches.RunTimeCacheSetAsync(Key, bytes, CandySoft.Cache);
                    return bytes;
                }
            }
            catch (Exception)
            {
                StaticUnitl.PopToast("图片加载失败");
                return null;
            }
        }

        private static void OnDownloadComplateEvent(Image image, string route, ImageSource bit)
        {
            string source = GetSource(image);
            if (source == route)
            {
                image.Source = bit;
                Indicator.IsRunning = false;
            }
        }
        #endregion
    }
}
