using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Library.TempControl
{
    public class CandyCheckBox : CheckBox
    {
        public string Text
        {
            get { return (string)this.GetValue(TextProperty); }
            set { this.SetValue(TextProperty, value); }
        }

        public static readonly BindableProperty TextProperty =
         BindableProperty.Create(nameof(Text), typeof(string), typeof(CandyCheckBox), default(string), BindingMode.Default);
    }
}
