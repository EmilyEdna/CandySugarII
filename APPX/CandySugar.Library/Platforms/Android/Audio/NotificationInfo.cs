﻿using Android.Graphics;

namespace CandySugar.Library.Platforms.Android.Audio;
public class NotificationInfo
{
    public Bitmap LargeIcon { get; set; }
    public string ContentTitle { get; set; }
    public string ContentText { get; set; }
    public string SubText { get; set; }

    public NotificationInfo(Bitmap largeIcon, string contentTitle, string contentText, string subText)
    {
        LargeIcon = largeIcon;
        ContentTitle = contentTitle;
        ContentText = contentText;
        SubText = subText;
    }
}