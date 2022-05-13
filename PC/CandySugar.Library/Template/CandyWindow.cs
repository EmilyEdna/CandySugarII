using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;
using CandySugar.Resource.Properties;

namespace CandySugar.Library.Template
{
    public class CandyWindow : Window
    {
        public CandyWindow()
        {
            this.MaxWidth = SystemParameters.PrimaryScreenWidth;
            this.MaxHeight = SystemParameters.PrimaryScreenHeight;
            CandySoft.Default.ScreenWidth = (this.MaxWidth / 10) * 6;
            CandySoft.Default.ScreenHeight = (this.MaxHeight / 10) * 7;
            this.Width = CandySoft.Default.ScreenWidth;
            this.Height = CandySoft.Default.ScreenHeight;
        }

        protected void StarAnime(string name) 
        {
           BeginStoryboard((Storyboard)this.FindResource(name));         
        }

    }
}
