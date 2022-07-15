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
    public class OrientationHandler
    {
        public delegate void OrientationChangedEventHandler(object sender, OrientationChanged e);
    }
}
