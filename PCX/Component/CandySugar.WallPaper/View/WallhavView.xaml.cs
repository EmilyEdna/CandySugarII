using CandySugar.Com.Options.ComponentGeneric;
using CommunityToolkit.Mvvm.Messaging;
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

namespace CandySugar.WallPaper.View
{
    /// <summary>
    /// WallhavView.xaml 的交互逻辑
    /// </summary>
    public partial class WallhavView : UserControl
    {
        public WallhavView()
        {
            InitializeComponent();
            WeakReferenceMessenger.Default.Register<MessageNotify>(this, (recip, notify) =>
            {
                if (notify.ControlType == 2)
                {
                    var param = (Tuple<double, double>)notify.ControlParam;
                    this.Width = param.Item1;
                    this.Height = param.Item2;
                }
            });
        }

        private void MouseUpChanged(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
