using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CandySugar.Com.Controls.ExtenControls
{
    public class CandyBorder:Border
    {
        public int BorderType
        {
            get { return (int)GetValue(BorderTypeProperty); }
            set { SetValue(BorderTypeProperty, value); }
        }
        /// <summary>
        /// [1:Primary] [2:Info] [3:Success] [4:Warn] [5:Error]
        /// </summary>
        public static readonly DependencyProperty BorderTypeProperty =
            DependencyProperty.Register("BorderType", typeof(int), typeof(CandyBorder), new PropertyMetadata(1));
    }
}
