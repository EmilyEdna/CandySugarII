using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using CandySugar.Library.Platforms.Android;
using CandySugar.Logic.Common;
using CandySugar.Logic.Entity.CandyEntity;
using CandySugar.Logic.Service;

namespace CandySugar.Entry
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            CrossCurrentActivity.Current.Activity = this;
            base.OnCreate(savedInstanceState);
            AndroidEnvironment.UnhandledExceptionRaiser += AndroidException;
        }

        /// <summary>
        /// 全局异常
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AndroidException(object sender, RaiseThrowableEventArgs e)
        {
            CandyContainer.Instance.Resolve<ICandyService>().AddLog(new CandyLog
            {
                 Message = e.Exception.Message,
                 Stack= e.Exception.StackTrace
            });

            //提示
            Task.Run(() =>
            {
                Looper.Prepare();
                //可以换成更友好的提示
                Toast.MakeText(this, "很抱歉,程序出现异常.", ToastLength.Long).Show();
                Looper.Loop();
            });

            //停一会，让前面的操作做完
            Thread.Sleep(2000);

            e.Handled = true;
        }
    }
}
