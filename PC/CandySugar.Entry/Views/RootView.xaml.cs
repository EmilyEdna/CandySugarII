using CandySugar.Library;
using CandySugar.Library.Template;
using CandySugar.Resource.Properties;
using MaterialDesignThemes.Wpf;
using System.Linq;
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

        private void TextBlock_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var Pck = StaticResource.FindVisualChild<PackIcon>(sender as TextBlock).FirstOrDefault();
            if (SilderMenu.Width > 0)
            {
                StarAnime("CloseSilder");
                Pck.Kind = PackIconKind.MenuRight;
            }
            else
            {
                StarAnime("OpenSilder");
                Pck.Kind = PackIconKind.MenuLeft;
            }
        }
    }
}
