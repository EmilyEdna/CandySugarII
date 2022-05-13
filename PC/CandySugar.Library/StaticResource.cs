using CandySugar.Resource.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CandySugar.Library
{
    public class StaticResource
    {
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
        /// <param name="Orginal"></param>
        /// <param name="Target"></param>
        public static void ThemeChange(string Orginal, string Target)
        {
            var AppResources = Application.Current.Resources.MergedDictionaries;
            ResourceDictionary Resource = new ResourceDictionary();
            var OrginalTheme = AppResources.FirstOrDefault(t => t.Source != null && t.Source.ToString().Contains($"{Orginal}Theme.xaml"));
            var Index = AppResources.IndexOf(OrginalTheme);
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
    }
}
