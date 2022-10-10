using CandySugar.Resource.Properties;
using HandyControl.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using XExten.Advance.CacheFramework;
using XExten.Advance.LinqFramework;

namespace CandySugar.Library.PropertyAttach
{
    public static class ImageThreadDependencyProperty
    {
        #region 委托
        public static event ComplateDelegate OnComplate;
        public delegate void ComplateDelegate(Image image, string route, BitmapSource bit);
        #endregion

        static ImageThreadDependencyProperty()
        {
            OnComplate += new ComplateDelegate(OnDownloadComplateEvent);
        }

        #region 字段
        public static string GetSource(DependencyObject obj)
        {
            return (string)obj.GetValue(SourceProperty);
        }

        public static void SetSource(DependencyObject obj, string value)
        {
            obj.SetValue(SourceProperty, value);
        }

        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.RegisterAttached("Source", typeof(string), typeof(ImageThreadDependencyProperty), new PropertyMetadata(OnSourceComplate));
        #endregion

        #region 方法
        private static void OnSourceComplate(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                var img = (Image)d;
                var data = new Tuple<string, Image>(img.Tag.ToString(), img);
                var route = e.NewValue == null ? ((Image)d).Source.GetValue(SourceProperty).ToString() : e.NewValue.ToString();
                new Thread(new ParameterizedThreadStart(DownloadImage))
                {
                    IsBackground = true
                }.Start(new Dictionary<string, Tuple<string, Image>> { { route, data } });
            }
            catch (Exception)
            {
                Growl.Info("未获取到路由");
            }

        }

        private async static void DownloadImage(object obj)
        {
            if (obj is Dictionary<string, Tuple<string, Image>> input)
            {
                try
                {
                    var bytes = await Comic(input.Keys.FirstOrDefault(), input.Values.FirstOrDefault().Item1.ToString());
                    var source = StaticResource.ToImage(bytes);
                    input.Values.FirstOrDefault().Item2.Dispatcher.BeginInvoke(new Action<Image, string, BitmapSource>((Img, Route, bit) =>
                    {
                        if (OnComplate != null)
                        {
                            OnComplate(Img, Route, bit);
                        }
                    }), new object[] { input.Values.FirstOrDefault().Item2, input.Keys.FirstOrDefault(), source });
                }
                catch
                {
                    Growl.Info("加载缓慢！请挂梯子食用！");
                }
            }
        }

        private static async Task<byte[]> Comic(string input, string tag)
        {
            var Key = input.ToMd5();
            var data = Caches.RunTimeCacheGet<byte[]>(Key);
            if (data != null && data.Length > 0)
                return data;
            else
            {
                HttpClient Client = new HttpClient();
                Client.DefaultRequestHeaders.Add("Host", $"{(tag.ToUpper() == "BIG" ? "i" : "n")}.nhentai.net");
                var result = await Client.GetByteArrayAsync(input);
                await Caches.RunTimeCacheSetAsync(Key, result, CandySoft.Default.Cache);
                return result;
            }
        }

        private static void OnDownloadComplateEvent(Image Image, string Route, BitmapSource Bit)
        {
            string source = GetSource(Image);
            if (source == Route)
            {
                Image.Source = Bit;
                Storyboard storyboard = new Storyboard();
                DoubleAnimation doubleAnimation = new DoubleAnimation(0.0, 1.0, new Duration(TimeSpan.FromMilliseconds(500.0)));
                Storyboard.SetTarget(doubleAnimation, Image);
                Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath("Opacity", new object[0]));
                storyboard.Children.Add(doubleAnimation);
                storyboard.Begin();
                var uniform = StaticResource.GetParentObject<UniformGrid>(Image);
                if (uniform == null) return;
                foreach (var ctrl in uniform.Children)
                {
                    if (ctrl is Image image && ctrl != null)
                    {
                        if (image.Name.Equals("Loading"))
                        {
                            image.Visibility = Visibility.Collapsed;
                        }
                        break;
                    }
                }
            }
        }
        #endregion
    }
}
