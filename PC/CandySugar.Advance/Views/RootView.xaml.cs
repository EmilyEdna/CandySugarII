using CandySugar.Advance.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CandySugar.Advance.Views
{
    public partial class RootView : HandyControl.Controls.Window
    {
        public RootView()
        {
            InitializeComponent();
        }

        private void OnLoad(object sender, RoutedEventArgs e)
        {
            (this.DataContext as RootViewModel).Upgrade();
        }
    }
}
