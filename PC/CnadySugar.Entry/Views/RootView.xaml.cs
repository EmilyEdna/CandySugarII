using CandySugar.Library.Template;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CnadySugar.Entry.Views
{
    public partial class RootView : CandyWindow
    {
        public RootView()
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
    }
}
