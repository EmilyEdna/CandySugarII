﻿using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using System;
using System.Threading;
using System.Threading.Tasks;
using Application = Android.App.Application;

namespace CandySugar.Library.Platforms.Android.CrossActivity
{
    /// <summary>
    /// Implementation for Feature
    /// </summary>
    [Preserve(AllMembers = true)]
    public class CurrentActivity : ICurrentActivity
    {

        /// <summary>
        /// Gets or sets the activity.
        /// </summary>
        /// <value>The activity.</value>
        public Activity Activity
        {
            get => lifecycleListener?.Activity;
            set
            {
                if (lifecycleListener == null)
                    Init(value, null);
            }
        }

        /// <summary>
		/// Activity state changed event handler
		/// </summary>
        public event EventHandler<ActivityEventArgs> ActivityStateChanged;


        /// <summary>
        /// Waits for an activity to be ready
        /// </summary>
        /// <returns></returns>
        public async Task<Activity> WaitForActivityAsync(CancellationToken cancelToken = default)
        {
            if (Activity != null)
                return Activity;

            var tcs = new TaskCompletionSource<Activity>();
            var handler = new EventHandler<ActivityEventArgs>((sender, args) =>
            {
                if (args.Event == ActivityEvent.Created || args.Event == ActivityEvent.Resumed)
                    tcs.TrySetResult(args.Activity);
            });

            try
            {
                using (cancelToken.Register(() => tcs.TrySetCanceled()))
                {
                    ActivityStateChanged += handler;
                    return await tcs.Task.ConfigureAwait(false);
                }
            }
            finally
            {
                ActivityStateChanged -= handler;
            }
        }


        internal void RaiseStateChanged(Activity activity, ActivityEvent ev)
            => ActivityStateChanged?.Invoke(this, new ActivityEventArgs(activity, ev));


        ActivityLifecycleContextListener lifecycleListener;

        /// <summary>
        /// Gets the current application context
        /// </summary>
        public Context AppContext =>
            Application.Context;

        /// <summary>
        /// Initialize current activity with application
        /// </summary>
        /// <param name="application">The main application</param>
        public void Init(Application application)
        {
            if (lifecycleListener != null)
                return;

            lifecycleListener = new ActivityLifecycleContextListener();
            application.RegisterActivityLifecycleCallbacks(lifecycleListener);
        }

        /// <summary>
        /// Initialize current activity with activity!
        /// </summary>
        /// <param name="activity">The main activity</param>
        /// <param name="bundle">Bundle for activity </param>
        public void Init(Activity activity, Bundle bundle)
        {
            Init(activity.Application);
            lifecycleListener.Activity = activity;
        }
    }

    [Preserve(AllMembers = true)]
    class ActivityLifecycleContextListener : Java.Lang.Object, Application.IActivityLifecycleCallbacks
    {
        WeakReference<Activity> currentActivity = new WeakReference<Activity>(null);

        public Context Context =>
            Activity ?? Application.Context;

        public Activity Activity
        {
            get => currentActivity.TryGetTarget(out var a) ? a : null;
            set => currentActivity.SetTarget(value);
        }

        CurrentActivity Current =>
            (CurrentActivity)(CrossCurrentActivity.Current);

        public void OnActivityCreated(Activity activity, Bundle savedInstanceState)
        {
            Activity = activity;
            Current.RaiseStateChanged(activity, ActivityEvent.Created);
        }

        public void OnActivityDestroyed(Activity activity)
        {
            Current.RaiseStateChanged(activity, ActivityEvent.Destroyed);
        }

        public void OnActivityPaused(Activity activity)
        {
            Activity = activity;
            Current.RaiseStateChanged(activity, ActivityEvent.Paused);
        }

        public void OnActivityResumed(Activity activity)
        {
            Activity = activity;
            Current.RaiseStateChanged(activity, ActivityEvent.Resumed);
        }

        public void OnActivitySaveInstanceState(Activity activity, Bundle outState)
        {
            Current.RaiseStateChanged(activity, ActivityEvent.SaveInstanceState);
        }

        public void OnActivityStarted(Activity activity)
        {
            Current.RaiseStateChanged(activity, ActivityEvent.Started);
        }

        public void OnActivityStopped(Activity activity)
        {
            Current.RaiseStateChanged(activity, ActivityEvent.Stopped);
        }
    }
}
