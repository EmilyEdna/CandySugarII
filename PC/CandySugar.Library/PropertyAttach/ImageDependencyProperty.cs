using CandySugar.Library.ImageTemplate;
using ImTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace CandySugar.Library.PropertyAttach
{
    public static class ImageDependencyProperty
    {
        #region Field

        public static string GetSource(DependencyObject obj)
        {
            return (string)obj.GetValue(SourceProperty);
        }

        public static void SetSource(DependencyObject obj, string value)
        {
            obj.SetValue(SourceProperty, value);
        }

        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.RegisterAttached("Source", typeof(string), typeof(ImageDependencyProperty), new PropertyMetadata(OnSourceComplate));

        private static void OnSourceComplate(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //执行下载队列
            DownloadQueue.StartQueue(e.NewValue.ToString(), (Image)d);
        }
        #endregion

        static ImageDependencyProperty()
        {
            DownloadQueue.OnComplate += new DownloadQueue.ComplateDelegate(OnDownloadComplateEvent);
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
                var uniform =  StaticResource.GetParentObject<UniformGrid>(Image);
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
    }
}
