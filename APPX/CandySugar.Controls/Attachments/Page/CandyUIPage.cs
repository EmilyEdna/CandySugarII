﻿using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace CandySugar.Controls
{
    [ContentProperty(nameof(Body))]
    public class CandyUIPage : ContentPage
    {
        protected Grid _contentGrid;

        public View Body { get => ContentBorder.Content; set => ContentBorder.Content = value; }
        public ObservableCollection<IPageAttachment> Attachments { get; set; } = new();
        public Border ContentBorder { get; set; } = new Border()
        {
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Fill,
            StrokeThickness = 0,
            Padding=0,
            Margin= 0,
        };
        public CandyUIPage()
        {
            ContentBorder.BackgroundColor = this.BackgroundColor;
            Content = _contentGrid = new Grid
            {
                Children =
                {
                    ContentBorder
                }
            };
            Attachments.CollectionChanged += Attachments_CollectionChanged;
        }
        private void Attachments_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (IPageAttachment attachment in e.NewItems)
                {
                    if (attachment.AttachmentPosition == AttachmentLocation.Front)
                    {
                        _contentGrid.Add(attachment, 0, 0);
                    }
                    else
                    {
                        _contentGrid.Insert(0, attachment);
                    }

                    attachment.OnAttached(this);
                }
            }

            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (IPageAttachment attachment in e.OldItems)
                {
                    _contentGrid.Remove(attachment);
                }
            }
        }
    }
}
