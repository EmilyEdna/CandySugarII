using CandySugar.Library;
using CandySugar.Library.Template;
using CandySugar.Resource.Properties;
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

        private void MineSize_Clicked(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Visibility = Visibility.Collapsed;
        }

        private void Close_Clicked(object sender, RoutedEventArgs e)
        {
            var parentWindow = Window.GetWindow(this);
            parentWindow.Close();
        }

        private void MaxSize_Clicked(object sender, RoutedEventArgs e)
        {
            var parentWindow = Window.GetWindow(this);

            if (parentWindow.WindowState == WindowState.Normal)
            {
                parentWindow.WindowState = WindowState.Maximized;
                parentWindow.Width = SystemParameters.PrimaryScreenWidth;
                parentWindow.Height = SystemParameters.PrimaryScreenHeight;
                CandySoft.Default.ScreenWidth = SystemParameters.PrimaryScreenWidth;
                CandySoft.Default.ScreenHeight = SystemParameters.PrimaryScreenHeight;
                StaticResource.GridClipContent(parentWindow, SystemParameters.PrimaryScreenWidth, SystemParameters.PrimaryScreenHeight);
            }
            else
            {
                parentWindow.WindowState = WindowState.Normal;
                CandySoft.Default.ScreenWidth = (SystemParameters.PrimaryScreenWidth / 10) * 6;
                CandySoft.Default.ScreenHeight = (SystemParameters.PrimaryScreenHeight / 10) * 7;
                parentWindow.Width = CandySoft.Default.ScreenWidth;
                parentWindow.Height = CandySoft.Default.ScreenHeight;
                StaticResource.GridClipContent(parentWindow, CandySoft.Default.ScreenWidth, CandySoft.Default.ScreenHeight);
            }
        }
    }
}
