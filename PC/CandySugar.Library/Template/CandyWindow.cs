using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CandySugar.Resource.Properties;

namespace CandySugar.Library.Template
{
    public class CandyWindow : Window
    {
        public CandyWindow()
        {
            CandySoft.Default.ScreenWidth = (SystemParameters.PrimaryScreenWidth / 10) * 6;
            CandySoft.Default.ScreenHeight = (SystemParameters.PrimaryScreenHeight / 10) * 7;
            this.Width = CandySoft.Default.ScreenWidth;
            this.Height = CandySoft.Default.ScreenHeight;
        }
    }
}
