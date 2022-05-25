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
                BackImage = "pack://application:,,,/CandySugar.Resource;component/Assets/Img6.jpg"
            });
            model.Add(new SilderModel
            {
                FuncName = 2,
                IconName = "BookOpenVariant",
                Name = "轻小说",
                Show = true,
                BackImage = "pack://application:,,,/CandySugar.Resource;component/Assets/Img7.jpg"
            });
            model.Add(new SilderModel
            {
                FuncName = 3,
                IconName = "TelevisionAmbientLight",
                Name = "动漫",
                Show = true,
                BackImage = "pack://application:,,,/CandySugar.Resource;component/Assets/Img8.jpg"
            });
            model.Add(new SilderModel
            {
                FuncName = 4,
                IconName = "TelevisionAmbientLight",
                Name = "H动漫",
                Show = CandySoft.Default.IsAdmin ? true : false,
                BackImage = "pack://application:,,,/CandySugar.Resource;component/Assets/Img9.jpg"
            });
            model.Add(new SilderModel
            {
                FuncName = 5,
                IconName = "RabbitVariant",
                Name = "漫画",
                Show = true,
                BackImage = "pack://application:,,,/CandySugar.Resource;component/Assets/Img10.jpg"
            });
            model.Add(new SilderModel
            {
                FuncName = 6,
                IconName = "ImageFilterVintage",
                Name = "壁纸",
                Show = true,
                BackImage = "pack://application:,,,/CandySugar.Resource;component/Assets/Img11.jpg"
            });
            model.Add(new SilderModel
            {
                FuncName = 7,
                IconName = "MusicNoteEighth",
                Name = "音乐",
                Show = true,
                BackImage = "pack://application:,,,/CandySugar.Resource;component/Assets/Img12.jpg"
            });
            model.Add(new SilderModel
            {
                FuncName = 8,
                IconName = "AccountTieWoman",
                Name = "教育",
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
        /// <typeparam name="T"></typeparam>
        /// <param name="method"></param>
        /// <param name="parent"></param>
        /// <param name="current"></param>
        public static void CreateControl<T>(MethodInfo method, object parent, object current) where T : UserControl
        {
            var instance = (CandyControl)Activator.CreateInstance(typeof(T));
            instance.DataContext = current;
            method.Invoke(parent, new[] { instance });
        }
    }
}
