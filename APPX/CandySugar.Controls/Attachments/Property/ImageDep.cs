using CandySugar.Library;

namespace CandySugar.Controls
{
    public class ImageDep
    {
        static ImageDep()
        {
            OnComplate += new ComplateDelegate(OnDownloadComplateEvent);
        }

        #region Action
        public static event ComplateDelegate OnComplate;
        public delegate void ComplateDelegate(Image image, string route, ImageSource bit);
        public static ActivityIndicator Indicator;
        public static Func<string, int, Task<byte[]>> Funcs { get; set; }
        #endregion

        #region Dep
        public static string GetSource(BindableObject obj)
        {
            return (string)obj.GetValue(SourceProperty);
        }

        public static void SetSource(BindableObject obj, string value)
        {
            obj.SetValue(SourceProperty, value);
        }
        public static readonly BindableProperty SourceProperty =
            BindableProperty.CreateAttached("Source", typeof(string), typeof(ImageDep), null, BindingMode.TwoWay, null, OnPropertyChanged);

        public static int GetType(BindableObject obj)
        {
            return (int)obj.GetValue(TypeProperty);
        }

        public static void SetType(BindableObject obj, int value)
        {
            obj.SetValue(TypeProperty, value);
        }
        public static readonly BindableProperty TypeProperty =
            BindableProperty.CreateAttached("Type", typeof(int), typeof(ImageDep), 0, BindingMode.TwoWay);
        #endregion

        #region Method
        private static void OnPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            try
            {
                var ctrl = (Image)bindable;
                Indicator = ((ctrl.Parent as Grid).Children.FirstOrDefault() as ActivityIndicator);
                Indicator.IsRunning = true;
                Indicator.IsVisible = true;
                ctrl.IsVisible = false;
                new Thread(new ParameterizedThreadStart(DownloadImage))
                {
                    IsBackground = true
                }.Start(new Dictionary<string, Image> { { newValue.ToString(), ctrl } });
            }
            catch (Exception)
            {
                "图片加载失败".OpenToast();
            }

        }

        private static  async void DownloadImage(object obj)
        {
            try
            {
                if (obj is Dictionary<string, Image> input)
                {
                    var model = input.FirstOrDefault();
                    var bytes = await Funcs?.Invoke(model.Key, GetType(model.Value)); 
                    if (bytes == null || bytes.Length == 0) return; 
                    var source = ImageSource.FromStream(() => new MemoryStream(bytes));
                    input.Values.FirstOrDefault().Dispatcher.Dispatch(() =>
                    {
                        OnComplate(input.Values.FirstOrDefault(), input.Keys.FirstOrDefault(), source);
                    });
                }
            }
            catch (Exception)
            {
                "图片加载失败".OpenToast();
            }
        }
        private static void OnDownloadComplateEvent(Image image, string route, ImageSource bit)
        {
            string source = GetSource(image);
            if (source == route)
            {
                image.Source = bit;
                Indicator.IsRunning = false;
                Indicator.IsVisible = false;
                image.IsVisible = true;
            }
        }
        #endregion
    }
}