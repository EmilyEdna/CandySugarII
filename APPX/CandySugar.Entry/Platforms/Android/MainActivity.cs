using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using CandySugar.Library;
using CandySugar.Library.Platforms.Android;
using CandySugar.Library.Platforms.Android.Audio;
using Esprima.Ast;

namespace CandySugar.Entry
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity, IAudioActivity
    {
        MediaPlayerServiceConnection _MediaPlayerServiceConnection;
        public MediaPlayerServiceBinder Binder { get; set ; }

        public event StatusChangedEventHandler StatusChanged;
        public event CoverReloadedEventHandler CoverReloaded;
        public event PlayingEventHandler Playing;
        public event BufferingEventHandler Buffering;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            CrossCurrentActivity.Current.Init(this, savedInstanceState);

            NotificationHelper.CreateNotificationChannel(ApplicationContext);
            if (_MediaPlayerServiceConnection == null)
                InitializeMedia();

            var intent = new Intent(ApplicationContext, typeof(MediaPlayerService));
            ApplicationContext.StartForegroundService(intent);

            AndroidEnvironment.UnhandledExceptionRaiser += AndroidException;
        }
        void InitializeMedia()
        {
            _MediaPlayerServiceConnection = new MediaPlayerServiceConnection(this);
            var mediaPlayerServiceIntent = new Intent(ApplicationContext, typeof(MediaPlayerService));
            BindService(mediaPlayerServiceIntent, _MediaPlayerServiceConnection, Bind.AutoCreate);
        }

        protected override async void OnResume()
        {
            base.OnResume();
            await Task.Delay(500);
        }
        /// <summary>
        /// 全局异常
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AndroidException(object sender, RaiseThrowableEventArgs e)
        {
            "很抱歉,程序出现异常".OpenToast();
            //停一会，让前面的操作做完
            Thread.Sleep(2000);
            e.Handled = true;
        }
    }
}