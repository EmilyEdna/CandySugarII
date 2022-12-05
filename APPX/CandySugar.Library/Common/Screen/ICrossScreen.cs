using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CandySugar.Library.Common.Screen
{
    /// <summary>
    /// 状态栏拓展
    /// </summary>
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
