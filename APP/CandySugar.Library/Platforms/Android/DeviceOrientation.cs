using Android.Content;
using Android.Content.PM;
using Android.Content.Res;
using Android.Hardware;
using Android.Runtime;
using Android.Views;
using CandySugar.Library.AndroidCommon;
using CandySugar.Library.AndroidCommon.Device;
using OrientationListener = CandySugar.Library.Platforms.Android.CrossDevice.OrientationListener;

namespace CandySugar.Library.Platforms.Android
{
    public class DeviceOrientation : BaseDeviceOrientation
    {
        private readonly OrientationListener _listener;
        private bool _disposed;
        private bool _isListenerEnabled = true;

        protected bool IsListenerEnabled
        {
            set
            {
                if (_listener == null) return;

                if (value == _isListenerEnabled) return;

                if (value)
                {
                    _listener.Enable();
                }
                else
                {
                    _listener.Disable();
                }

                _isListenerEnabled = value;
            }
        }

        public DeviceOrientation()
        {
            _listener = new OrientationListener(OnOrientationChanged);

            if (_listener.CanDetectOrientation())
            {
                _listener.Enable();
            }
        }

        public override DeviceOrientations CurrentOrientation
        {
            get
            {
                var activity = CrossCurrentActivity.Current.Activity;
                var rotation = activity.WindowManager.DefaultDisplay.Rotation;

                return Convert(rotation);
            }
        }

        public override void LockOrientation(DeviceOrientations orientation)
        {
            var activity = CrossCurrentActivity.Current.Activity;

            activity.RequestedOrientation = Convert(orientation);
        }

        public override void UnlockOrientation()
        {
            var activity = CrossCurrentActivity.Current.Activity;

            activity.RequestedOrientation = Convert(DeviceOrientations.Undefined);
        }

        public override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing && _listener != null)
                {
                    _listener.Disable();
                    _listener.Dispose();
                }

                _disposed = true;
            }

            base.Dispose(disposing);
        }

        public static void NotifyOrientationChange(Orientation newOrientation, bool isForms = true)
        {
            var instance = (DeviceOrientation)CrossDeviceOrientation.Current;

            if (instance == null)
                throw new InvalidCastException("Cast from IDeviceOrientation to Android.DeviceOrientationImplementation");

            instance.IsListenerEnabled = !isForms;

            instance.OnOrientationChanged(new OrientationChanged
            {
                Orientation = CrossDeviceOrientation.Current.CurrentOrientation
            });
        }

        private ScreenOrientation Convert(DeviceOrientations orientation)
        {
            switch (orientation)
            {
                case DeviceOrientations.Portrait:
                    return ScreenOrientation.Portrait;
                case DeviceOrientations.PortraitFlipped:
                    return ScreenOrientation.ReversePortrait;
                case DeviceOrientations.Landscape:
                    return ScreenOrientation.Landscape;
                case DeviceOrientations.LandscapeFlipped:
                    return ScreenOrientation.ReverseLandscape;
                default:
                    return ScreenOrientation.Unspecified;
            }
        }

        public DeviceOrientations Convert(SurfaceOrientation orientation)
        {
            switch (orientation)
            {
                case SurfaceOrientation.Rotation0:
                    return DeviceOrientations.Portrait;
                case SurfaceOrientation.Rotation180:
                    return DeviceOrientations.PortraitFlipped;
                case SurfaceOrientation.Rotation90:
                    return DeviceOrientations.Landscape;
                case SurfaceOrientation.Rotation270:
                    return DeviceOrientations.LandscapeFlipped;
                default:
                    return DeviceOrientations.Undefined;
            }
        }
    }

}
