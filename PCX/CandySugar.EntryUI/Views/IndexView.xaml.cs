using CandySugar.Com.Library.HotKey;
using CandySugar.Com.Library.VisualTree;
using CandySugar.Com.Options.ComponentGeneric;
using CandySugar.Com.Options.NotifyObject;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CandySugar.EntryUI.Views
{
    /// <summary>
    /// IndexView.xaml 的交互逻辑
    /// </summary>
    public partial class IndexView : Window
    {
        private HotKeyAction _HotKey;
        public IndexView()
        {
            InitializeComponent();
            RelyLocation();
            _HotKey = new HotKeyAction();
            Loaded += Window_Loaded;
            StateChanged += Window_Stated;
        }

        private void Window_Stated(object sender, EventArgs e)
        {
            RelyLocation();
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
            //自定义搜索框的位置
            PopBox.CustomPopupPlacementCallback = new((popupSize, targetSize, offset) =>
            {
                Point point = new(0, 0);
                if (WindowState == WindowState.Maximized)
                    point = new Point(SystemParameters.PrimaryScreenWidth / 2.4, SystemParameters.PrimaryScreenHeight / 10);
                if (this.WindowState == WindowState.Normal)
                    point = new Point(Width / 3, Height / 10);
                CustomPopupPlacement placement = new(point, PopupPrimaryAxis.None);
                return new CustomPopupPlacement[] { placement };
            });
            WeakReferenceMessenger.Default.Register<DefaultNotify>(this, (recip, notify) =>
            {
                if (notify.Module == EDefaultNotify.SearchNotify)
                {
                    PopBox.IsOpen = true;
                }
            });
        }

        /// <summary>
        /// 动态转换浮动按钮的位置
        /// </summary>
        public  void RelyLocation()
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.Height = SystemParameters.PrimaryScreenHeight;
                this.Width = SystemParameters.PrimaryScreenWidth;
            }
            if (this.WindowState == WindowState.Normal)
            {
                this.Height = 450;
                this.Width = 800;
            }
            Canvas.SetTop(FloatBtn, this.Height - 100);
            Canvas.SetLeft(FloatBtn, this.Width - 100);
            GenericDelegate.InformationAction?.Invoke(this.Width, this.Height);
        }

        private void PopMenuEvent(object sender, RoutedEventArgs e)
        {
            PopMenu.Opened += delegate { ((Storyboard)FindResource("Overly")).Begin(); };
            PopMenu.IsOpen = true;
        }
    }
}
