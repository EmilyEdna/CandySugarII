using CandySugar.Com.Library;
using CandySugar.Com.Library.VisualTree;
using CandySugar.Com.Options.ComponentGeneric;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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
            Icon.Content = FontIcon.AnglesLeft;
            GenericDelegate.InformationAction = new((width, height) =>
            {
                Canvas.SetTop(FloatBtn, height - 160);
                Canvas.SetLeft(FloatBtn, width - 100);
                this.Width = width;
                this.Height = height - 35 <= 0 ? 0 : height - 35;
                this.RightSider.Width = this.Width / 3;
                this.RightSider.Height = this.Height;
            });
            WeakReferenceMessenger.Default.Register<LightNotify>(this, (recip, notify) =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    CreateOpenDyamicAmime();
                });
            });
        }

        private void PopMenuEvent(object sender, RoutedEventArgs e)
        {
            PopMenu.Opened += delegate { ((Storyboard)FindResource("Overly")).Begin(); };
            PopMenu.IsOpen = true;
        }

        private void SilderEvent(object sender, RoutedEventArgs e)
        {
            Icon.IsEnabled = false;
            if ((int)Icon.Tag == 0) CreateOpenDyamicAmime();
            else CreateCloseDyamicAmime();
        }

        #region DyamicAmime

        private void CreateOpenDyamicAmime()
        {
            Storyboard storyboard = new Storyboard();
            DoubleAnimationUsingKeyFrames doubleAnimations = new DoubleAnimationUsingKeyFrames();
            Storyboard.SetTargetName(doubleAnimations, "RightSider");
            Storyboard.SetTargetProperty(doubleAnimations, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(TranslateTransform.X)"));
            doubleAnimations.KeyFrames.Add(new EasingDoubleKeyFrame(-80, TimeSpan.Zero));
            doubleAnimations.KeyFrames.Add(new EasingDoubleKeyFrame(this.Width / (-3), TimeSpan.FromSeconds(1)));
            storyboard.Children.Add(doubleAnimations);
            storyboard.Completed += delegate
            {
                Icon.Tag = 1;
                Icon.IsEnabled = true;
                Icon.Content = FontIcon.AnglesRight;
            };
            storyboard.Begin(this.RightSider);
        }

        private void CreateCloseDyamicAmime()
        {
            Storyboard storyboard = new Storyboard();
            DoubleAnimationUsingKeyFrames doubleAnimations = new DoubleAnimationUsingKeyFrames();
            Storyboard.SetTargetName(doubleAnimations, "RightSider");
            Storyboard.SetTargetProperty(doubleAnimations, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(TranslateTransform.X)"));
            doubleAnimations.KeyFrames.Add(new EasingDoubleKeyFrame(this.Width / (-3), TimeSpan.Zero));
            doubleAnimations.KeyFrames.Add(new EasingDoubleKeyFrame(-80, TimeSpan.FromSeconds(1)));
            storyboard.Children.Add(doubleAnimations);
            storyboard.Completed += delegate
            {
                Icon.Tag = 0;
                Icon.IsEnabled = true;
                Icon.Content = FontIcon.AnglesLeft;
            };
            storyboard.Begin(this.RightSider);
        }
        #endregion
    }
}
