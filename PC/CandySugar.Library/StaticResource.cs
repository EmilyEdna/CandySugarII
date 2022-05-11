using CandySugar.Resource.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CandySugar.Library
{
    public static class StaticResource
    {
        public static Rect RectProperty = new Rect(0,0,CandySoft.Default.ScreenWidth, CandySoft.Default.ScreenHeight);
    }
}
