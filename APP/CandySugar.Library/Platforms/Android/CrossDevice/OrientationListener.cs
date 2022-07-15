using Android.Content;
using Android.Hardware;
using Android.Runtime;
using Android.Views;
using CandySugar.Library.AndroidCommon;
using CandySugar.Library.AndroidCommon.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application = Android.App.Application;

namespace CandySugar.Library.Platforms.Android.CrossDevice
{
    public class OrientationListener : OrientationEventListener
    {
        private readonly Action<OrientationChanged> _onOrientationChanged;

        private DeviceOrientations _cachedOrientation;
        public OrientationListener(Action<OrientationChanged> onOrientationChanged)
           : base(Application.Context, SensorDelay.Normal)
        {
            _onOrientationChanged = onOrientationChanged;
        }

        public OrientationListener(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public OrientationListener(Context context)
            : base(context)
        {
        }

        public OrientationListener(Context context, SensorDelay rate)
            : base(context, rate)
        {
        }
        public override void OnOrientationChanged(int orientation)
        {
            var currentOrientation = CrossDeviceOrientation.Current.CurrentOrientation;

            if (currentOrientation != _cachedOrientation)
            {
                _cachedOrientation = currentOrientation;

                _onOrientationChanged(new OrientationChanged
                {
                    Orientation = currentOrientation
                });
            }
        }
    }
}
