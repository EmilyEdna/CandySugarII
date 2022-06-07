using CandySugar.Resource.Properties;
using Sdk.Component.Image.sdk;
using Sdk.Component.Image.sdk.ViewModel;
using Sdk.Component.Image.sdk.ViewModel.Enums;
using Sdk.Component.Image.sdk.ViewModel.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace CandySugar.Library.PropertyAttach
{
    public class ImageDependencyProperty
    {
        #region Attached DependencyProperty
        public static string GetRoute(DependencyObject obj)
        {
            return (string)obj.GetValue(RouteProperty);
        }
        public static void SetRoute(DependencyObject obj, string value)
        {
            obj.SetValue(RouteProperty, value);
        }

        public static readonly DependencyProperty RouteProperty =
            DependencyProperty.RegisterAttached("Route", typeof(string), typeof(ImageDependencyProperty), new PropertyMetadata(async (obj, e) =>
            {
                if (obj is Image img)
                {
                    img.SetBinding(Image.SourceProperty, new Binding
                    {
                        Source = await ImageBind(e.NewValue.ToString())
                    });
                }
            }));

        private async static Task<BitmapSource> ImageBind(string input)
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
            return StaticResource.ToImage(ImageInitData.DownResult.Bytes);
        }

        #endregion
    }
}
