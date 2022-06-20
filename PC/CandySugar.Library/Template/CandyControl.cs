using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace CandySugar.Library.Template
{
    public class CandyControl: UserControl
    {
        public void BeginAnime(string Xkey) 
        {
            BeginStoryboard((Storyboard)this.FindResource(Xkey));
        }
        
    }
}
