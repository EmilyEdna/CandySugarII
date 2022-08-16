using Android.Hardware;
using Android.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CandySugar.Library.Platforms.Android.CrossHandWare;
using CandySugar.Library.AndroidCommon;

namespace CandySugar.Library.Platforms.Android
{
    public class CrossHand : ICrossHand
    {
        public static SensorManager Manager { get; set; }
        public static SensorEventListener Listener = new SensorEventListener();
        public void RegistEvent()
        {
            Manager ??= (SensorManager)CrossCurrentActivity.Current.Activity.GetSystemService(Context.SensorService);
            var sensor = Manager.GetDefaultSensor(SensorType.Accelerometer);
            Manager.RegisterListener(Listener, sensor, SensorDelay.Normal);
        }
        public void UnRegistEvent()
        {
            if (Manager != null)
                Manager.UnregisterListener(Listener);
        }
    }
}
