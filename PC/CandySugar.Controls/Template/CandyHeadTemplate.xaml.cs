using CandySugar.Library;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CandySugar.Controls.Template
{
    /// <summary>
    /// CandyHeadTemplate.xaml 的交互逻辑
    /// </summary>
    public partial class CandyHeadTemplate : CandyHead
    {
        public CandyHeadTemplate()
        {
            InitializeComponent();

        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            this.ThemeName.Text = "Dark";
            StaticResource.ThemeChange("Light", "Dark");
        }

        private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            this.ThemeName.Text = "Light";
            StaticResource.ThemeChange("Dark", "Light");
        }
    }
}
