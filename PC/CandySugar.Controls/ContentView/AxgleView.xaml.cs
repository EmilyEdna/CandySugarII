using CandySugar.Library.Template;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CandySugar.Controls.ContentView
{
    /// <summary>
    /// AxgleView.xaml 的交互逻辑
    /// </summary>
    public partial class AxgleView : CandyControl
    {
        public AxgleView()
        {
            InitializeComponent();
        }
        private void CoverClicked(object sender, MouseButtonEventArgs e)
        {
            var contentView = (dynamic)CoverViews.GetType().GetField("_viewContent", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(CoverViews);
            contentView.Background = new SolidColorBrush(Colors.Transparent);
            contentView.BorderThickness = new Thickness(0);
        }
    }
}
