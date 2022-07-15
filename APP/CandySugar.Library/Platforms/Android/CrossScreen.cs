using Android.App;
using Android.Views;
using CandySugar.Library.AndroidCommon.Screen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Library.Platforms.Android
{
    public class CrossScreen: ICrossScreen
    {
        private Activity Current => CrossCurrentActivity.Current.Activity;

        public void HiddenStatusBar()
        {
            Current.Window.AddFlags(WindowManagerFlags.Fullscreen);
        }
        public void ShowStatusBar()
        {
            Current.Window.ClearFlags(WindowManagerFlags.Fullscreen);
        }
    }
}
