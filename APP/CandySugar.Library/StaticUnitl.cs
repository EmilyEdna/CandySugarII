using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Library
{
    public class StaticUnitl
    {
        /// <summary>
        /// 提示
        /// </summary>
        /// <param name="input"></param>
        /// <param name="IsLong"></param>
        public static async void PopToast(string input, bool IsLong = false)
        {
            try
            {
                await Toast.Make(input, IsLong ? ToastDuration.Long : ToastDuration.Short).Show();
            }
            catch (Exception)
            {
                //解决Toast在子线程问题
#if ANDROID
            Android.OS.Looper.Prepare();
#endif
                await Toast.Make(input, IsLong ? ToastDuration.Long : ToastDuration.Short).Show();
#if ANDROID
            Android.OS.Looper.Loop();
#endif
            }
        }
    }
}
