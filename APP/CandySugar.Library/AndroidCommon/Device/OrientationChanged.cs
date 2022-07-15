using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Library.AndroidCommon.Device
{
    public class OrientationChanged: EventArgs
    {
        /// <summary>
        ///     Gets device orientation
        /// </summary>
        public DeviceOrientations Orientation { get; set; }
    }
}
