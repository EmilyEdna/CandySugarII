﻿using CandySugar.Library.Common;
using CandySugar.Library.Common.Device;
using CandySugar.Library.Common.Screen;

namespace CandySugar.Controls
{
    public class C1ViewModel : ViewModelBase
    {
        readonly IService Service;

        public C1ViewModel(BaseServices baseServices) : base(baseServices)
        {
            Service = this.Container.Resolve<IService>();
        }

        public override void Initialize(INavigationParameters parameters)
        {
            Route = parameters.GetValue<string>("Route");
            Hidden();
        }

        #region Property
        string _Route;
        public string Route
        {
            get => _Route;
            set => SetProperty(ref _Route, value);
        }
        #endregion

        #region Method
        public void Show()
        {
            CrossDeviceOrientation.Current.LockOrientation(DeviceOrientations.Portrait);
            ICrossScreen.ScreenState.ShowStatusBar();
            Nav.GoBackAsync();
        }
        public void Hidden()
        {
            CrossDeviceOrientation.Current.LockOrientation(DeviceOrientations.Landscape);
            ICrossScreen.ScreenState.HiddenStatusBar();
        }
        #endregion

        #region Command
        public DelegateCommand BackCommand => new(() =>
        {
            Show();
        });
        #endregion
    }
}