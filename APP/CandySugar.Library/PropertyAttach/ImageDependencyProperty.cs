using CandySugar.Library.PropertyQueue;

namespace CandySugar.Library.PropertyAttach
{
    public static class ImageDependencyProperty
    {
        static ImageDependencyProperty()
        {
            DownloadQueue.OnComplate += new DownloadQueue.ComplateDelegate(OnDownloadComplateEvent);
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
            BindableProperty.CreateAttached("Source", typeof(string), typeof(ImageDependencyProperty), null, BindingMode.OneWay, null, OnPropertyChanged);
        #endregion

        #region 方法
        private async static void OnPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            //执行下载队列
            DownloadQueue.StartQueue((Image)bindable,newValue.ToString());
        }



        private static void OnDownloadComplateEvent(Image image, string route, ImageSource bit)
        {
            string source = GetSource(image);
            if (source == route)
            {
                image.Source = bit;
            }
        }
        #endregion
    }
}
