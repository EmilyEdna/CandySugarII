using CandySugar.Com.Library.BitConvert;
using CandySugar.Com.Library.DownQueue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CandySugar.Com.Controls.AttachControls
{
    public static class ImageAttach
    {

        static ImageAttach()
        {
            DownloadQueue.DownEventAction = new(OnDownload);
        }

        public static string GetSourceAsync(DependencyObject obj)
        {
            return (string)obj.GetValue(SoucreAysncProperty);
        }

        public static void SetSourceAsync(DependencyObject obj, string value)
        {
            obj.SetValue(SoucreAysncProperty, value);
        }

        public static readonly DependencyProperty SoucreAysncProperty =
            DependencyProperty.RegisterAttached("SourceAsync", typeof(string), typeof(ImageAttach), new PropertyMetadata(OnComplate));

        private static void OnComplate(DependencyObject sender, DependencyPropertyChangedEventArgs @event)
        {
            DownloadQueue.Init(@event.NewValue.ToString(),  (Enum)((Image)sender).Tag, (Image)sender);
        }
        private static void OnDownload(FrameworkElement element, byte[] data)
        {
            var images = BitmapHelper.Bytes2Image(data);
            element.Dispatcher.Invoke(() =>
            {
                ((Image)element).Source = images;
            });
        }

    }
}
