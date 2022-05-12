using CandySugar.Resource.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CandySugar.Library
{
    public class StaticResource
    {
        public static Rect RectProperty = new Rect(0, 0, CandySoft.Default.ScreenWidth, CandySoft.Default.ScreenHeight);

        public static void ThemeChange(string Orginal, string Target)
        {
            var AppResources = Application.Current.Resources.MergedDictionaries;
            ResourceDictionary Resource = new ResourceDictionary();
            var OrginalTheme = AppResources.FirstOrDefault(t => t.Source != null && t.Source.ToString().Contains($"{Orginal}Theme.xaml"));
            var Index = AppResources.IndexOf(OrginalTheme);
            Resource.Source = new Uri($"pack://application:,,,/CandySugar.Resource;component/Styles/{Target}Theme.xaml");
            AppResources[Index] = Resource;
        }
    }
}
