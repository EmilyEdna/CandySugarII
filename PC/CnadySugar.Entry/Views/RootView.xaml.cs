using CandySugar.Library;
using CandySugar.Library.Template;
using CandySugar.Resource.Properties;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CnadySugar.Entry.Views
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
            var GridContent = (Grid)this.Template.FindName("GridContent", this);
            GridContent.Clip = new RectangleGeometry(new Rect(0, 0, CandySoft.Default.ScreenWidth, CandySoft.Default.ScreenHeight), 15, 15);
        }
    }
}
