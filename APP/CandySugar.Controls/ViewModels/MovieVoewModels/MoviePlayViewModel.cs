using CandySugar.Library.AndroidCommon.Device;
using CandySugar.Library.AndroidCommon.Screen;

namespace CandySugar.Controls.ViewModels.MovieVoewModels
{
    public class MoviePlayViewModel : BaseViewModel
    {
        public override void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            Route= query["Key"].ToString();
            LoadMauiAsset();
            Hidden();
        }

        #region 属性
        string _Route;
        public string Route
        {
            get => _Route;
            set => SetProperty(ref _Route, value);
        }
        string _Content;
        public string Content
        {
            get => _Content;
            set => SetProperty(ref _Content, value);
        }
        #endregion

        #region 方法
        async void LoadMauiAsset()
        {
            using var stream = await FileSystem.OpenAppPackageFileAsync("Dplayer.html");
            using var reader = new StreamReader(stream);
            Content = reader.ReadToEnd();
        }
        public void Show()
        {
            CrossDeviceOrientation.Current.LockOrientation(DeviceOrientations.Portrait);
            ICrossHand.Instance.UnRegistEvent();
            ICrossScreen.ScreenState.ShowStatusBar();
        }
        public void Hidden()
        {
            //CrossDeviceOrientation.Current.LockOrientation(DeviceOrientations.Landscape);
            ICrossHand.Instance.RegistEvent();
            ICrossScreen.ScreenState.HiddenStatusBar();
        }
        #endregion
    }
}
