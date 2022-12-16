using CandySugar.Library;
using CandySugar.Library.Common;
using CandySugar.Library.Common.Device;
using CandySugar.Library.Common.Screen;

namespace CandySugar.Controls
{
    public class B2ViewModel : ViewModelBase
    {
        public B2ViewModel(BaseServices baseServices) : base(baseServices) { }

        public override void Initialize(INavigationParameters parameters)
        {
            Route = parameters.GetValue<string>("Route");
            Inner = parameters.GetValue<bool>("Inner");
            LoadMauiAsset();
            Hidden();
        }
        #region Property
        public bool Inner { get; set; }
        public string Route { get; set; }
        public string Content { get; set; }
        #endregion

        #region Method
        async void LoadMauiAsset()
        {
            using var stream = await FileSystem.OpenAppPackageFileAsync(Inner?"Dplayer.html":"MaoPlayer.html");
            using var reader = new StreamReader(stream);
            Content = reader.ReadToEnd();
        }
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
