using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace CandySugar.LightNovel.View
{
    /// <summary>
    /// IndexView.xaml 的交互逻辑
    /// </summary>
    public partial class IndexView : UserControl
    {
        public IndexView()
        {
            InitializeComponent();
        }

        private void PopMenuEvent(object sender, System.Windows.RoutedEventArgs e)
        {
            PopMenu.Opened += delegate { ((Storyboard)FindResource("Overly")).Begin(); };
            PopMenu.IsOpen = true;
        }
    }
}
