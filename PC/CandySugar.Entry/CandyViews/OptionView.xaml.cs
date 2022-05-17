﻿using CandySugar.Library.Template;
using CandySugar.Resource.Properties;
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
using XExten.Advance.LinqFramework;

namespace CandySugar.Entry.CandyViews
{
    /// <summary>
    /// OptionView.xaml 的交互逻辑
    /// </summary>
    public partial class OptionView : CandyWindow
    {
        public OptionView()
        {
            InitializeComponent();
        }

        private void Grid_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void GiftOpenEvent(object sender, MouseButtonEventArgs e)
        {
            GiftContent.Visibility = Visibility.Visible;
            StarAnime("GiftOpen");
        }

        private void GiftCloseEvent(object sender, RoutedEventArgs e)
        {
            StarAnime("GiftClose");
        }

        private void HandleEvent(object sender, RoutedEventArgs e)
        {
            var type = ((Button)sender).CommandParameter.ToString().AsInt();
            if (type == 1) this.Close();
        }

        private void SettingWin_Loaded(object sender, RoutedEventArgs e)
        {
            StarAnime("OpenWindow");
            if (CandySoft.Default.PlayBox) DPlayer.IsChecked = true;
            else VLC.IsChecked = true;
        }

        private void PlayBoxChecked(object sender, RoutedEventArgs e)
        {
            CandySoft.Default.PlayBox = ((RadioButton)sender).CommandParameter.ToString().AsInt() == 1;
        }
    }
}
