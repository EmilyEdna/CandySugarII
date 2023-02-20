using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace CandySugar.Com.Style
{
    /// <summary>
    /// Theme.xaml 的交互逻辑
    /// </summary>
    public partial class Theme : ResourceDictionary
    {
        public virtual void MoveEvent(object sender, MouseButtonEventArgs e)
        {

            if (e.LeftButton == MouseButtonState.Pressed)
                ((Window)((Border)sender).TemplatedParent).DragMove();
        }
        public virtual void MinEvent(object sender, RoutedEventArgs e)
        {
            Window win = (Window)((Button)sender).TemplatedParent;
            win.WindowState = WindowState.Minimized;
        }
        public virtual void MaxEvent(object sender, RoutedEventArgs e)
        {
            Window win = (Window)((Button)sender).TemplatedParent;
            if (win.WindowState == WindowState.Maximized)
                win.WindowState = WindowState.Normal;
            else
                win.WindowState = WindowState.Maximized;
        }
        public virtual void CloseEvent(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(0);
        }
    }
}
