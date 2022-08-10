using CandySugar.Library.Template;
using CandySugar.Library.TemplateEffects;
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
using System.Windows.Shapes;

namespace CandySugar.Entry.Views
{
    /// <summary>
    /// LoginView.xaml 的交互逻辑
    /// </summary>
    public partial class LoginView : CandyWindow
    {
        WaterEffect waterEffect;
        public LoginView()
        {
            InitializeComponent();
            waterEffect = new WaterEffect(100, 100);
            login.Effect = waterEffect;
            waterEffect.StartTime();
        }

        private void CloseEvent(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void MoveEvent(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void MouseEvent(object sender, MouseButtonEventArgs e)
        {
            Point point = Mouse.GetPosition(e.Source as FrameworkElement);//WPF方法
            waterEffect.Drop((float)(point.X / login.ActualWidth), (float)(point.Y / login.ActualHeight));//只在图片的中心触发波纹效果
        }
    }
}
