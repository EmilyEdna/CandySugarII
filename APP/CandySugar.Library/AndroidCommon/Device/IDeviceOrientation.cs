using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Library.AndroidCommon.Device
{
    public interface IDeviceOrientation
    {
        /// <summary>
        ///     Gets current device orientation
        /// </summary>
        DeviceOrientations CurrentOrientation { get; }

        /// <summary>
        ///     Event handler when orientation changes
        /// </summary>
        event OrientationHandler.OrientationChangedEventHandler OrientationChanged;

        /// <summary>
        ///     Lock orientation in the specified position
        /// </summary>
        /// <param name="orientation">Position for lock.</param>
        void LockOrientation(DeviceOrientations orientation);

        /// <summary>
        ///     Unlock orientation
        /// </summary>
        void UnlockOrientation();
    }
}
