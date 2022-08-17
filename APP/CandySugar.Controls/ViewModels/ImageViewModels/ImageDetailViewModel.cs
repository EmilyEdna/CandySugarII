using CandySugar.Library.AndroidCommon;
using CandySugar.Library.AndroidCommon.Device;
using CandySugar.Library.AndroidCommon.Screen;
using System.Web;

namespace CandySugar.Controls.ViewModels.ImageViewModels
{
    public class ImageDetailViewModel : BaseViewModel
    {

        public override void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            CrossDeviceOrientation.Current.LockOrientation(DeviceOrientations.Landscape);
            ICrossScreen.ScreenState.HiddenStatusBar();
            Route = HttpUtility.UrlDecode(query["Key"].ToString());
        }

        #region 属性
        string _Route;
        public string Route
        {
            get => _Route;
            set => SetProperty(ref _Route, value);
        }
        #endregion
    }
}
