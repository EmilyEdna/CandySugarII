using CandySugar.Library.Template;
using CandySugar.Library.TemplateModel;
using CandySugar.Resource.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Sdk.Component.Plugins;
using System.Drawing;
using System.IO;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using XExten.Advance.StaticFramework;
using XExten.Advance.LinqFramework;
using System.Diagnostics;
using Microsoft.Web.WebView2.Wpf;

namespace CandySugar.Library
{
    public class StaticResource
    {
        public static List<SilderModel> InitMenu()
        {
            List<SilderModel> model = new List<SilderModel>();
            model.Add(new SilderModel
            {
                FuncName = 1,
                IconName = "BookOpenVariant",
                Name = "小说",
                Show = true,
                Query = "XS",
                BackImage = "pack://application:,,,/CandySugar.Resource;component/Assets/Img6.jpg"
            });
            model.Add(new SilderModel
            {
                FuncName = 2,
                IconName = "BookOpenVariant",
                Name = "轻小说",
                Show = true,
                Query = "LXS",
                BackImage = "pack://application:,,,/CandySugar.Resource;component/Assets/Img7.jpg"
            });
            model.Add(new SilderModel
            {
                FuncName = 3,
                IconName = "TelevisionAmbientLight",
                Name = "动漫",
                Show = true,
                Query = "DM",
                BackImage = "pack://application:,,,/CandySugar.Resource;component/Assets/Img8.jpg"
            });
            model.Add(new SilderModel
            {
                FuncName = 4,
                IconName = "TelevisionAmbientLight",
                Name = "H动漫",
                Query = "HDM",
                Show = CandySoft.Default.IsAdmin ? true : false,
                BackImage = "pack://application:,,,/CandySugar.Resource;component/Assets/Img9.jpg"
            });
            model.Add(new SilderModel
            {
                FuncName = 5,
                IconName = "RabbitVariant",
                Name = "漫画",
                Show = true,
                Query = "MH",
                BackImage = "pack://application:,,,/CandySugar.Resource;component/Assets/Img10.jpg"
            });
            model.Add(new SilderModel
            {
                FuncName = 6,
                IconName = "ImageFilterVintage",
                Name = "壁纸",
                Show = true,
                Query = "BZ",
                BackImage = "pack://application:,,,/CandySugar.Resource;component/Assets/Img11.jpg"
            });
            model.Add(new SilderModel
            {
                FuncName = 7,
                IconName = "MusicNoteEighth",
                Name = "音乐",
                Show = true,
                Query = "YY",
                BackImage = "pack://application:,,,/CandySugar.Resource;component/Assets/Img12.jpg"
            });
            model.Add(new SilderModel
            {
                FuncName = 8,
                IconName = "AccountTieWoman",
                Name = "教育",
                Query = "JY",
                Show = CandySoft.Default.IsAdmin ? true : false,
                BackImage = "pack://application:,,,/CandySugar.Resource;component/Assets/Img13.jpg"
            });
            return model;
        }
        /// <summary>
        /// 设置窗体动态图形
        /// </summary>
        /// <param name="window"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public static void GridClipContent(Window window, double width, double height)
        {
            var GridContent = (Grid)window.Template.FindName("GridContent", window);
            GridContent.Clip = new RectangleGeometry(new Rect(0, 0, width, height), 15, 15);
        }
        /// <summary>
        /// 主题切换
        /// </summary>
        /// <param name="Original"></param>
        /// <param name="Target"></param>
        public static void ThemeChange(string Original, string Target)
        {
            var AppResources = Application.Current.Resources.MergedDictionaries;
            ResourceDictionary Resource = new ResourceDictionary();
            var OriginalTheme = AppResources.FirstOrDefault(t => t.Source != null && t.Source.ToString().Contains($"{Original}Theme.xaml"));
            var Index = AppResources.IndexOf(OriginalTheme);
            Resource.Source = new Uri($"pack://application:,,,/CandySugar.Resource;component/Styles/{Target}Theme.xaml");
            AppResources[Index] = Resource;
        }
        /// <summary>
        /// 查找子控件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static List<T> FindVisualChild<T>(DependencyObject obj) where T : DependencyObject
        {
            try
            {
                List<T> TList = new List<T> { };
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                    if (child != null && child is T)
                    {
                        TList.Add((T)child);
                        List<T> childOfChildren = FindVisualChild<T>(child);
                        if (childOfChildren != null)
                        {
                            TList.AddRange(childOfChildren);
                        }
                    }
                    else
                    {
                        List<T> childOfChildren = FindVisualChild<T>(child);
                        if (childOfChildren != null)
                        {
                            TList.AddRange(childOfChildren);
                        }
                    }
                }
                return TList;
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
                return null;
            }
        }
        /// <summary>
        /// 创建内容对象
        /// </summary>
        /// <typeparam name="T">当前控件</typeparam>
        /// <param name="parentMethod">父控件的某个方法</param>
        /// <param name="parentDataContext">父控件的DataContext</param>
        /// <param name="currentDataContext">当前控件的DataContext</param>
        public static void CreateControl<T>(MethodInfo parentMethod, object parentDataContext, object currentDataContext) where T : UserControl
        {
            var instance = (CandyControl)Activator.CreateInstance(typeof(T));
            instance.DataContext = currentDataContext;
            parentMethod.Invoke(parentDataContext, new[] { instance });
        }
        /// <summary>
        /// 创建内容对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="currentDataContext">当前控件的DataContext</param>
        /// <param name="input">查询字段</param>
        /// <param name="action">反射的方法</param>
        public static CandyControl CreateControl<T>(object currentDataContext, string input,string action= "SearchAction")
        {
            var instance = (CandyControl)Activator.CreateInstance(typeof(T));
            instance.DataContext = currentDataContext;

            var MethodInfo = instance.DataContext.GetType().GetMethod(action);
            if (MethodInfo != null)
            {
                MethodInfo.Invoke(instance.DataContext, new[] { input });
            }
            return instance;
        }
        /// <summary>
        /// 返回实现方式
        /// </summary>
        /// <returns></returns>
        public static SdkImpl ImplType()
        {
            if (CandySoft.Default.QueryModule == 1) return SdkImpl.Multi;
            else if (CandySoft.Default.QueryModule == 2) return SdkImpl.Rest;
            else if (CandySoft.Default.QueryModule == 3) return SdkImpl.RPC;
            else return SdkImpl.User;
        }
        /// <summary>
        /// 代理
        /// </summary>
        /// <returns></returns>
        public static SdkProxy Proxy()
        {
            return new SdkProxy
            {
                IP = CandySoft.Default.IP,
                Port = CandySoft.Default.Port,
                UserName = CandySoft.Default.PA,
                PassWord = CandySoft.Default.PP
            };
        }
        /// <summary>
        /// bytes转图片
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static BitmapSource ToImage(byte[] bytes, int width = 180, int height = 240)
        {
            Bitmap bmp = System.Drawing.Image.FromStream(new MemoryStream(bytes)) as Bitmap;
            var ptr = bmp.GetHbitmap();
            var source = Imaging.CreateBitmapSourceFromHBitmap(
                  ptr, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromWidthAndHeight(width, height));
            source.Freeze();
            bmp.Dispose();
            Import.DeleteObject(ptr);
            return source;
        }
        /// <summary>
        /// 把图片按照块状切分
        /// </summary>
        /// <param name="input"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static ImageSource[] SplitImage(BitmapSource input, int width = 50, int height = 50)
        {
            var colCount = input.PixelWidth / width;
            var rowCount = input.PixelHeight / height;
            var results = new ImageSource[rowCount * colCount];
            var stride = width * ((input.Format.BitsPerPixel + 7) / 8);
            var pixelsCount = width * height;
            var tileRect = new Int32Rect(0, 0, width, height);

            for (int row = 0; row < rowCount; row++)
            {
                for (int col = 0; col < colCount; col++)
                {
                    var pixels = new int[pixelsCount];
                    var copyRect = new Int32Rect(col * width, row * height, height, height);
                    input.CopyPixels(copyRect, pixels, stride, 0);
                    var wb = new WriteableBitmap(width, height, input.DpiX, input.DpiY, input.Format, input.Palette);
                    wb.Lock();
                    wb.WritePixels(tileRect, pixels, stride, 0);
                    wb.Unlock();

                    results[row * colCount + col] = wb;
                }
            }

            return results;
        }
        /// <summary>
        /// 过滤特殊字段
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string FileNameFilter(string input)
        {
            string[] Filter = { ":", "\\", "/", "*", "?", "<", ">", "|", "\"" };
            Filter.ForArrayEach<string>(item =>
            {
                if (input.Contains(item))
                {
                    input = input.Replace(item, "_");
                }
            });
            return input;
        }
        /// <summary>
        /// 下载
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="dir"></param>
        /// <param name="fileName"></param>
        /// <param name="extens"></param>
        public static string Download(byte[] bytes, string dir, string fileName, string extens)
        {
            var dirs = SyncStatic.CreateDir(Path.Combine(Environment.CurrentDirectory, "CandyDown", dir, $"{FileNameFilter(fileName)}"));
            var fn = SyncStatic.CreateFile(Path.Combine(dirs, $"{FileNameFilter(fileName)}.{extens}"));
            var route = SyncStatic.WriteFile(bytes, fn);
            Process.Start("explorer.exe", dirs);
            return route;
        }
        /// <summary>
        /// 初始化WebView
        /// </summary>
        /// <param name="WebView"></param>
        /// <param name="Name"></param>
        public static async void CreateWebView(WebView2 WebView, string Name)
        {
            await WebView.EnsureCoreWebView2Async(null);

            WebView.CoreWebView2.Navigate(new Uri($"{Environment.CurrentDirectory}\\Webs\\{Name}.html").AbsoluteUri);

            WebView.CoreWebView2.Settings.AreDefaultContextMenusEnabled = false;
            WebView.CoreWebView2.Settings.AreDevToolsEnabled = false;
        }
        /// <summary>
        /// 获取壁纸模式
        /// </summary>
        /// <returns></returns>
        public static string ImageModule()
        {
            if (CandySoft.Default.Module == 1) return string.Empty;
            else if (CandySoft.Default.Module == 2) return $"{CandySoft.Default.K12}";
            else if (CandySoft.Default.Module == 3) return $"{CandySoft.Default.K15}";
            else return $"{CandySoft.Default.K18}";
        }
    }
}
