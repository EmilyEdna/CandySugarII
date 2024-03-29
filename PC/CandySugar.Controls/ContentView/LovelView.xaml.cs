﻿using CandySugar.Library.Template;
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
    /// LovelView.xaml 的交互逻辑
    /// </summary>
    public partial class LovelView : CandyControl
    {
        public LovelView()
        {
            InitializeComponent();
        }

        private void ItemSelected(object sender, SelectionChangedEventArgs e)
        {
            BeginAnime("OpenDetail");
        }

        private void TextClicked(object sender, MouseButtonEventArgs e)
        {
            BeginAnime("CloseDetail");
        }
    }
}
