﻿using CandySugar.Controls.ContentViewModel;
using CandySugar.Library;
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
    /// AnimeView.xaml 的交互逻辑
    /// </summary>
    public partial class AnimeView : CandyControl
    {
        public AnimeView()
        {
            InitializeComponent();
        }

        private void ItemSelected(object sender, SelectionChangedEventArgs e)
        {
            BeginStoryboard((Storyboard)this.FindResource("OpenDetail"));
        }

        private void TextClicked(object sender, MouseButtonEventArgs e)
        {
            BeginStoryboard((Storyboard)this.FindResource("CloseDetail"));
        }

        private void LoadEvent(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            {
                var Model = (this.DataContext as AnimeViewModel);
                Model.WebView = this.WebViewCtrl;
                StaticResource.CreateWebView(Model.WebView, "Dplayer");
            }
        }
    }
}
