using CandySugar.Com.Library;
using CandySugar.Com.Options.NotifyObject;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using XExten.Advance.LinqFramework;
using XExten.Advance.StaticFramework;

namespace CandySugar.Com.Style
{
    /// <summary>
    /// Theme.xaml 的交互逻辑
    /// </summary>
    public partial class Theme : ResourceDictionary
    {
        private Stopwatch Watch;
        /// <summary>
        /// 背景轮询队列
        /// </summary>
        private ConcurrentQueue<string> BackQueue;
        public Theme()
        {
            Watch = new();
            BackQueue = new();
            CompositionTarget.Rendering += AnimetionEvent;
            Watch.Start();
        }

        /// <summary>
        /// 利用关键帧动态切换背景图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void AnimetionEvent(object sender, EventArgs args)
        {

            if (Watch.Elapsed.Subtract(TimeSpan.Zero).TotalSeconds <= 30d)
                return;
            SyncStatic.CreateDir(CommonHelper.BackgourdResource);
            var files = Directory.GetFiles(CommonHelper.BackgourdResource);
            if (files.Length <= 0) return;
            if (BackQueue.IsEmpty)
                files.ForArrayEach<string>(BackQueue.Enqueue);
            ((Dispatcher)sender).Invoke(() =>
            {
                var style = this["CandyDefaultWindowStyle"] as System.Windows.Style;
                var template = ((Setter)style.Setters.LastOrDefault()).Value as ControlTemplate;
                var win = Application.Current.MainWindow;
                if (win.Name.Equals("CandyWindow"))
                {
                    Grid grid = template.FindName("ImageBackgroud", win) as Grid;
                    if (grid != null)
                    {
                        BackQueue.TryDequeue(out string file);
                        grid.BeginAnimation(Grid.OpacityProperty, new DoubleAnimation(0, 1, TimeSpan.FromSeconds(3)));
                        grid.Background = new ImageBrush(new BitmapImage(new Uri(file)));
                        Watch.Restart();
                    }
                }
            });
        }

        /// <summary>
        /// 窗体拖拽事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void MoveEvent(object sender, MouseButtonEventArgs e)
        {

            if (e.LeftButton == MouseButtonState.Pressed)
                ((Window)((Border)sender).TemplatedParent).DragMove();
        }
        /// <summary>
        /// 最小化事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void MinEvent(object sender, RoutedEventArgs e)
        {
            Window win = (Window)((Button)sender).TemplatedParent;
            win.WindowState = WindowState.Minimized;
        }
        /// <summary>
        /// 最大化事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void MaxEvent(object sender, RoutedEventArgs e)
        {
            Window win = (Window)((Button)sender).TemplatedParent;
            if (win.WindowState == WindowState.Maximized)
                win.WindowState = WindowState.Normal;
            else
                win.WindowState = WindowState.Maximized;
        }
        /// <summary>
        /// 窗体关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void CloseEvent(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(0);
        }
        /// <summary>
        /// 搜索事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void SearchEvent(object sender, RoutedEventArgs e)
        {
            WeakReferenceMessenger.Default.Send(new DefaultNotify { Module = EDefaultNotify.SearchNotify });
        }
    }
}
