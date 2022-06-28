using CandySugar.Library;
using CandySugar.Library.Template;
using CandySugar.Resource.Properties;
using MaterialDesignThemes.Wpf;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace CandySugar.Entry.Views
{
    public partial class RootView : CandyWindow
    {
        public RootView() : base()
        {
            InitializeComponent();
            this.Version.Text = $"甜糖V_{Assembly.GetExecutingAssembly().GetName().Version}";
        }

        private void CandyHeadTemplate_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void CandyWindow_Loaded(object sender, RoutedEventArgs e)
        {
            StaticResource.GridClipContent(this, CandySoft.Default.ScreenWidth, CandySoft.Default.ScreenHeight);
        }

        private void TaskbarIcon_TrayMouseDoubleClick(object sender, RoutedEventArgs e)
        {
            this.window.Visibility = Visibility.Visible;
        }

        private void ProcessClick(object sender, RoutedEventArgs e)
        {
            Storyboard story = ((Storyboard)this.FindResource("Hidden"));
            if (story != null)
            {
                story.Completed += delegate { Application.Current.Shutdown(0); };
                story.Begin(this);
            };
        }
    }
}
