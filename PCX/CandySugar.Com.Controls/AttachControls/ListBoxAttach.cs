using CandySugar.Com.Library.VisualTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CandySugar.Com.Controls.AttachControls
{
    public static class ListBoxAttach
    {

        public static void SetCommand(DependencyObject target, ICommand value)
        {
            target.SetValue(CommandProperty, value);
        }

        public static ICommand GetPreviewMouseLeftButtonDown(DependencyObject target)
        {
            return (ICommand)target.GetValue(CommandProperty);
        }

        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(ListBoxAttach), new FrameworkPropertyMetadata(default(ICommand), new PropertyChangedCallback(CommandChanged)));

        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            ListBox element = target as ListBox;

            if (element != null)
            {
                if ((e.NewValue != null) && (e.OldValue == null))
                {
                    element.Loaded += InitLoad;
                }
                else if ((e.NewValue == null) && (e.OldValue != null))
                {
                    element.Loaded -= InitLoad;
                }
            }
        }

        private static void InitLoad(object sender, RoutedEventArgs e)
        {
            ListBox element = sender as ListBox;
            if (element != null)
            {
                ScrollViewer scrollViewer = element.FindChildren<ScrollViewer>().FirstOrDefault();
                if (scrollViewer != null)
                {
                    scrollViewer.ScrollChanged += ScrollChanged;
                }
            }
        }

        private static void ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            ListBox element = (sender as ScrollViewer).FindParent<ListBox>();
            if (element != null)
            {
                ICommand command = (ICommand)element.GetValue(CommandProperty);
                command.Execute(e);
            }
        }
    }
}
