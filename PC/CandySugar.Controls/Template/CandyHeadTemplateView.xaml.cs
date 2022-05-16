using CandySugar.Library;
using CandySugar.Library.Template;
using CandySugar.Resource.Properties;
using System.Windows;
using System.Windows.Media.Animation;

namespace CandySugar.Controls.Template
{
    /// <summary>
    /// CandyHeadTemplate.xaml 的交互逻辑
    /// </summary>
    public partial class CandyHeadTemplateView : CandyControl
    {
        public CandyHeadTemplateView()
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
            Storyboard story = ((Storyboard)parentWindow.FindResource("Hidden"));
            if (story != null)
            {
                story.Completed += delegate { parentWindow.Close(); };
                story.Begin(parentWindow);
            };
        }

        private void MaxSize_Clicked(object sender, RoutedEventArgs e)
        {
            var parentWindow = Window.GetWindow(this);
            if (parentWindow.WindowState == WindowState.Normal)
            {
                parentWindow.WindowState = WindowState.Maximized;
                StaticResource.GridClipContent(parentWindow, SystemParameters.PrimaryScreenWidth, SystemParameters.PrimaryScreenHeight);
            }
            else
            {
                parentWindow.WindowState = WindowState.Normal;
                StaticResource.GridClipContent(parentWindow, CandySoft.Default.ScreenWidth, CandySoft.Default.ScreenHeight);
            }
        }
    }
}
