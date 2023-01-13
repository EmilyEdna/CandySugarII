using System.Reflection;
using System;
using System.Windows;

namespace CandySugar.Style
{
    public class ThemeExtension
    {
        public static Uri GetStyleAbsoluteUri(string path)
        {
            return new Uri("pack://application:,,,/CandySugar.Style;component/" + path);
        }

    }
}
