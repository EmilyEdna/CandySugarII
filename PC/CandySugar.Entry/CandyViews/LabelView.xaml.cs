using CandySugar.Library.Template;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CandySugar.Entry.CandyViews
{
    /// <summary>
    /// LabelView.xaml 的交互逻辑
    /// </summary>
    public partial class LabelView : CandyWindow
    {
        public LabelView()
        {
            InitializeComponent();
        }

        private void HandlEvent(object sender, RoutedEventArgs e)
        {
            var btn = (sender as Button);
            if (btn.CommandParameter.ToString().Equals("1"))
            {
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                this.DialogResult = false;
                this.Close();
            }
        }
    }
}
