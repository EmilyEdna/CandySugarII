using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Library.Common.Device
{
    public abstract class BaseDeviceOrientation : IDeviceOrientation, IDisposable
    {
        private bool _disposed;

        /// <summary>
        ///     Current device orientation
        /// </summary>
        public abstract DeviceOrientations CurrentOrientation { get; }

        /// <summary>
        ///     Lock orientation in the specified position
        /// </summary>
        /// <param name="orientation">Position for lock.</param>
        public abstract void LockOrientation(DeviceOrientations orientation);

        /// <summary>
        ///     Unlock orientation
        /// </summary>
        public abstract void UnlockOrientation();

        /// <summary>
        ///     Event that fires when orientation changes
        /// </summary>
        public event OrientationHandler.OrientationChangedEventHandler OrientationChanged;

        /// <summary>
        ///     Dispose of class and parent classes
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     When orientation changes
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnOrientationChanged(OrientationChanged e)
        {
            OrientationChanged?.Invoke(this, e);
        }

        /// <summary>
        ///     Dispose up
        /// </summary>
        ~BaseDeviceOrientation()
        {
            Dispose(false);
        }

        /// <summary>
        ///     Dispose method
        /// </summary>
        /// <param name="disposing"></param>
        public virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                _disposed = true;
            }
        }
    }
}
