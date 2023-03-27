using CandySugar.Com.Library.VisualTree;
using CandySugar.Com.Options.ComponentGeneric;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Threading.Tasks;
using System.Windows;
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
            GenericDelegate.InformationAction = new((width, height) =>
            {
                Canvas.SetTop(FloatBtn, height - 160);
                Canvas.SetLeft(FloatBtn, width - 100);
                this.Width = width;
                this.Height = height - 35 <= 0 ? 0 : height - 35;
            });
            WeakReferenceMessenger.Default.Register<LightNotify>(this, (recip, notify) =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    CreateDyamicAmime().Begin(this.RightSider);
                });
            });
        }

        private void PopMenuEvent(object sender, System.Windows.RoutedEventArgs e)
        {
            PopMenu.Opened += delegate { ((Storyboard)FindResource("Overly")).Begin(); };
            PopMenu.IsOpen = true;
        }

        private Storyboard CreateDyamicAmime() 
        {
            Storyboard storyboard = new Storyboard();
            DoubleAnimationUsingKeyFrames doubleAnimations = new DoubleAnimationUsingKeyFrames();
            Storyboard.SetTargetName(doubleAnimations, "RightSider");
            Storyboard.SetTargetProperty(doubleAnimations, new PropertyPath(WidthProperty));
            doubleAnimations.KeyFrames.Add(new EasingDoubleKeyFrame(0, TimeSpan.Zero));
            doubleAnimations.KeyFrames.Add(new EasingDoubleKeyFrame(this.Width/3, TimeSpan.FromSeconds(1)));
            storyboard.Children.Add(doubleAnimations);
            return storyboard;
        }
    }
}
