using CandySugar.Library.AndroidCommon.Device;
using CandySugar.Library.AndroidCommon.Screen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CandySugar.Controls.ViewModels.AxgleViewModels
{
    public class AxglePlayViewModel : BaseViewModel
    {
        public override void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            //CrossDeviceOrientation.Current.LockOrientation(DeviceOrientations.Landscape);
            ICrossHand.Instance.RegistEvent();
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
