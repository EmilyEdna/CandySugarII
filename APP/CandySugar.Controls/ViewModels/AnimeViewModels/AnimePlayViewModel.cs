using CandySugar.Library.AndroidCommon;
using CandySugar.Library.AndroidCommon.Device;
using CandySugar.Library.AndroidCommon.Screen;
using System.Web;

namespace CandySugar.Controls.ViewModels.AnimeViewModels
{
    public class AnimePlayViewModel : BaseViewModel
    {
        public override void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            Route = HttpUtility.UrlDecode(query["Key"].ToString());
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
