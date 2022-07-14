using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Library.AndroidCommon.Device
{

    /// <summary>
    ///     Arguments to pass to event handlers
    /// </summary>
    public class OrientationChangedEventArgs : EventArgs
    {
        /// <summary>
        ///     Gets device orientation
        /// </summary>
        public DeviceOrientations Orientation { get; set; }
    }

    /// <summary>
    ///     Orientation changed event handlers
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void OrientationChangedEventHandler(object sender, OrientationChangedEventArgs e);
}
