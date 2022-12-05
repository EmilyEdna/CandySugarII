using Android.Content.PM;
using Android.Hardware;
using Android.Runtime;
using CandySugar.Library.Common;
using CandySugar.Library.Common.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Object = Java.Lang.Object;

namespace CandySugar.Library.Platforms.Android.CrossHandWare
{
    public class SensorEventListener : Object, ISensorEventListener
    {
        private DeviceOrientations Orientation = DeviceOrientations.Portrait;
        public void OnAccuracyChanged(Sensor sensor, [GeneratedEnum] SensorStatus accuracy)
        {
        }

        public void OnSensorChanged(SensorEvent e)
        {
            if (SensorType.Accelerometer != e.Sensor.Type) return;

            float x = e.Values[0];
            float y = e.Values[1];


            DeviceOrientations NewOrientation;

            if (x < 4.5 && x >= -4.5 && y >= 4.5)
                NewOrientation = DeviceOrientations.Portrait;
            else if (x >= 4.5 && y < 4.5 && y >= -4.5)
                NewOrientation = DeviceOrientations.Landscape;
            else if (x <= -4.5 && y < 4.5 && y >= -4.5)
                NewOrientation = DeviceOrientations.LandscapeFlipped ;
            else
                NewOrientation = DeviceOrientations.PortraitFlipped;
            if (Orientation != NewOrientation)
            {
                CrossDeviceOrientation.Current.LockOrientation(NewOrientation);
                Orientation = NewOrientation;
            }

        }
    }
}
