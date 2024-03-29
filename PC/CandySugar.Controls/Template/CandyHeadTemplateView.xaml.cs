﻿using CandySugar.Library;
using CandySugar.Library.Template;
using CandySugar.Resource.Properties;
using DryIoc;
using System.Windows;
using System.Windows.Media.Animation;

namespace CandySugar.Controls.Template
{
    /// <summary>
    /// CandyHeadTemplate.xaml 的交互逻辑
    /// </summary>
    public partial class CandyHeadTemplateView : CandyControl
    {
        public CandyHeadTemplateView()
        {
            InitializeComponent();

        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            this.ThemeName.Text = "Dark";
            StaticResource.ThemeChange("Light", "Dark");
        }

        private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            this.ThemeName.Text = "Light";
            StaticResource.ThemeChange("Dark", "Light");
        }

        private void MineSize_Clicked(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Visibility = Visibility.Collapsed;
        }

        private void Close_Clicked(object sender, RoutedEventArgs e)
        {
            var parentWindow = Window.GetWindow(this);
            Storyboard story = ((Storyboard)parentWindow.FindResource("Hidden"));
            if (story != null)
            {
                story.Completed += delegate { parentWindow.Close(); };
                story.Begin(parentWindow);
            };
        }

        private void MaxSize_Clicked(object sender, RoutedEventArgs e)
        {
            var parentWindow = Window.GetWindow(this);
            if (parentWindow.WindowState == WindowState.Normal)
            {
                parentWindow.WindowState = WindowState.Maximized;
                parentWindow.Width = SystemParameters.PrimaryScreenWidth;
                parentWindow.Height = SystemParameters.PrimaryScreenHeight;
                CandySoft.Default.ScreenWidth = SystemParameters.PrimaryScreenWidth;
                CandySoft.Default.ScreenHeight = SystemParameters.PrimaryScreenHeight;
                StaticResource.GridClipContent(parentWindow, SystemParameters.PrimaryScreenWidth, SystemParameters.PrimaryScreenHeight);
            }
            else
            {
                parentWindow.WindowState = WindowState.Normal;
                CandySoft.Default.ScreenWidth = (SystemParameters.PrimaryScreenWidth / 10) * 6;
                CandySoft.Default.ScreenHeight = (SystemParameters.PrimaryScreenHeight / 10) * 7;
                parentWindow.Width = (SystemParameters.PrimaryScreenWidth / 10) * 6;
                parentWindow.Height = (SystemParameters.PrimaryScreenHeight / 10) * 7;
                StaticResource.GridClipContent(parentWindow, CandySoft.Default.ScreenWidth, CandySoft.Default.ScreenHeight);
            }
        }

        private bool IsOpen = true;
        private void HandleEvent(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var win = (Window.GetWindow(this) as CandyWindow);
            if (IsOpen)
            {
                IsOpen = false;
                win.StarAnime("CloseSilder");
            }
            else
            {
                IsOpen = true;
                win.StarAnime("OpenSilder");
            }
        }
    }
}
