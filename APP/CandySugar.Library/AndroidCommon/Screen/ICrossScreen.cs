using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CandySugar.Library.AndroidCommon.Screen
{
    public interface ICrossScreen
    {
        static ICrossScreen ScreenState
        {
            get
            {
#if ANDROID
                return new CandySugar.Library.Platforms.Android.CrossScreen();
#else
                return null;
#endif
            }
        }

        void HiddenStatusBar();
        void ShowStatusBar();
    }
}
