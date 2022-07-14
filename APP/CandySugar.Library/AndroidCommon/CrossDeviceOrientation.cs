using CandySugar.Library.AndroidCommon.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#if ANDROID
using CandySugar.Library.Platforms.Android;
#endif

namespace CandySugar.Library.AndroidCommon
{
    public class CrossDeviceOrientation
    {
        private static readonly Lazy<IDeviceOrientation> Implementation =
            new Lazy<IDeviceOrientation>(() => CreateDeviceOrientation(), LazyThreadSafetyMode.PublicationOnly);

        /// <summary>
        ///     Gets if the plugin is supported on the current platform.
        /// </summary>
        public static bool IsSupported => Implementation.Value != null;

        /// <summary>
        ///     Current settings to use
        /// </summary>
        public static IDeviceOrientation Current
        {
            get
            {
                var ret = Implementation.Value;
                if (ret == null)
                    throw NotImplementedInReferenceAssembly();
                return ret;
            }
        }

        private static IDeviceOrientation CreateDeviceOrientation()
        {
#if ANDROID
            return new DeviceOrientation();
#else
            return null;
#endif
        }

        internal static Exception NotImplementedInReferenceAssembly()
        {
            return new NotImplementedException(
                "This functionality is not implemented in the portable version of this assembly.  You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");
        }
    }
}
