using CandySugar.Com.Library.HotKey;
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
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CandySugar.WPFTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private HotKeyAction _HotKey;
        public MainWindow()
        {
            InitializeComponent();
            _HotKey = new HotKeyAction();
            Loaded += Window_Loaded;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _HotKey.RegistHotKey();
        }
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            _HotKey.SetHwnd(this);
        }
        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);
            _HotKey.InitHotKey();
        }
    }
}
