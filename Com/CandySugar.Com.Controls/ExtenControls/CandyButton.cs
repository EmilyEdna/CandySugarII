using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CandySugar.Com.Controls.ExtenControls
{
    public class CandyButton : Button
    {
        public int ButtonType
        {
            get { return (int)GetValue(ButtonTypeProperty); }
            set { SetValue(ButtonTypeProperty, value); }
        }
        /// <summary>
        /// [1:Primary] [2:Info] [3:Success] [4:Warn] [5:Error]
        /// </summary>
        public static readonly DependencyProperty ButtonTypeProperty =
            DependencyProperty.Register("ButtonType", typeof(int), typeof(CandyButton), new PropertyMetadata(1));
    }
}
