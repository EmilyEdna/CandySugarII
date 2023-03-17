using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CandySugar.Com.Controls.ExtenControls
{
    public class CandyFloatButton : Button
    {
        static CandyFloatButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CandyFloatButton), new FrameworkPropertyMetadata(typeof(CandyFloatButton)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }


    }
}
