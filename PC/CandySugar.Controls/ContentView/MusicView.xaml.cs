using CandySugar.Controls.ContentViewModel;
using CandySugar.Library.Template;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CandySugar.Controls.ContentView
{
    /// <summary>
    /// MusicView.xaml 的交互逻辑
    /// </summary>
    public partial class MusicView : CandyControl
    {
        public MusicView()
        {
            InitializeComponent();
        }

        private void MouseUpChanged(object sender, MouseButtonEventArgs e)
        {

            if (sender is ListBoxItem boxItem)
            {
                var Content = boxItem.Content.ToString();
                if (Content == "单曲") BeginStoryboard((Storyboard)this.FindResource("OpenSong"));
                else if (Content == "歌单") BeginStoryboard((Storyboard)this.FindResource("OpenSheet"));
                else BeginStoryboard((Storyboard)this.FindResource("OpenPlayList"));
            }
            if (sender is TextBlock block)
            {
                if (block.Text.Equals("查看详情"))
                    BeginStoryboard((Storyboard)this.FindResource("OpenDetail"));
                else
                    BeginStoryboard((Storyboard)this.FindResource("OpenAlbum"));
            }
        }

        private void EachEvent(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListBoxItem boxItem)
            {
                if (boxItem.Name == "单曲循环")
                {
                    列表循环.IsSelected = false;
                    单曲循环.IsSelected = true;
                }
                else
                {
                    列表循环.IsSelected = true;
                    单曲循环.IsSelected = false;
                }
            }
        }

        private int Vol = 0;
        private void VolumeEvent(object sender, RoutedEventArgs e)
        {
            if (Vol == 0)
            {
                BeginStoryboard((Storyboard)this.FindResource("VolOpen"));
                Vol = 1;
            }
            else
            {
                BeginStoryboard((Storyboard)this.FindResource("VolClose"));
                Vol = 0;
            }
        }

        private void VolChangeEvent(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var slider = (sender as Slider);
            VolumeShow.Content = (int)slider.Value + "%";
            var vm = ((MusicViewModel)this.DataContext);
            if (vm != null)
            {
                var Audio = ((MusicViewModel)this.DataContext).AudioFactory;
                if (Audio != null && Audio.PlayOut() != null)
                {
                    Audio.Change((float)(slider.Value / 100f));
                }
            }
        }
    }
}
