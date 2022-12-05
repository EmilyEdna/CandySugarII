using CandySugar.Library.Common.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#if ANDROID
using CandySugar.Library.Platforms.Android;
#endif

namespace CandySugar.Library.Common
{
    /// <summary>
    /// 屏幕方向拓展
    /// </summary>
    public class CrossDeviceOrientation
    {
        private static readonly Lazy<IDeviceOrientation> Implementation =
            new Lazy<IDeviceOrientation>(() =>
            {
#if ANDROID
                return new DeviceOrientation();
#else
                return null;
#endif
            }, LazyThreadSafetyMode.PublicationOnly);

        /// <summary>
        ///     Current settings to use
        /// </summary>
        public static IDeviceOrientation Current => Implementation.Value;
    }
}
