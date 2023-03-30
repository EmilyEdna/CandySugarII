using CandySugar.Com.Options.ComponentGeneric;
using NStandard;
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

namespace CandySugar.Music.View
{
    /// <summary>
    /// IndexView.xaml 的交互逻辑
    /// </summary>
    public partial class IndexView : UserControl
    {
        private int ActiveAnime = 1;
        private IndexViewModel VM;
        private Storyboard AnimeX1;
        private Storyboard AnimeX2;
        private Storyboard AnimeX3;
        public IndexView()
        {
            InitializeComponent();
            AnimeX1 = (Storyboard)FindResource("SingleSongAnimeKey");
            AnimeX2 = (Storyboard)FindResource("SongListAnimeKey");
            AnimeX3 = (Storyboard)FindResource("CollectListAnimeKey");
            AnimeX1.Completed += AnimeEvent;
            AnimeX2.Completed += AnimeEvent;
            AnimeX3.Completed += AnimeEvent;
            Loaded += delegate { VM = (IndexViewModel)this.DataContext; };
            GenericDelegate.InformationAction = new((width, height) =>
            {
                Canvas.SetTop(FloatBtn, height - 160);
                Canvas.SetLeft(FloatBtn, width - 100);
                this.Width = width;
                this.Height = height - 35 <= 0 ? 0 : height - 35;
            });
        }

        private void AnimeEvent(object sender, EventArgs e)
        {
            VM.ChangeCommand(ActiveAnime);
        }

        private void PopMenuEvent(object sender, RoutedEventArgs e)
        {
            PopMenu.Opened += delegate { ((Storyboard)FindResource("Overly")).Begin(); };
            PopMenu.IsOpen = true;
        }

        private void MouseUpChanged(object sender, MouseButtonEventArgs e)
        {
            var ListItem = (sender as ListBoxItem);
            var CK = ListItem.Tag.ToString().AsInt();
            if (CK == 1 && CK != ActiveAnime) Animetion(CK).Begin();
            if (CK == 2 && CK != ActiveAnime) Animetion(CK).Begin();
            if (CK == 3 && CK != ActiveAnime) Animetion(CK).Begin();
        }

        private Storyboard Animetion(int active)
        {
            ActiveAnime = active;
            if (active == 1) return AnimeX1;
            else if (active == 2) return AnimeX2;
            else return AnimeX3;
        }
    }
}
