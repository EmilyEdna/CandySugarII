﻿using System;
using Android.App;

namespace CandySugar.Library.Platforms.Android.CrossActivity
{
    public class ActivityEventArgs : EventArgs
    {
        internal ActivityEventArgs(Activity activity, ActivityEvent ev)
        {
            Event = ev;
            Activity = activity;
        }

        public ActivityEvent Event { get; }
        public Activity Activity { get; }
    }
}
