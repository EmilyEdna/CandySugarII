using CandySugar.Library.AndroidCommon.Device;
using CandySugar.Library.AndroidCommon.Screen;

namespace CandySugar.Controls.Views.AxgleViews;

public partial class AxglePlayView : ContentPage
{
	public AxglePlayView()
	{
		InitializeComponent();
	}

	private void BackEvent(object sender, EventArgs e)
	{
        CrossDeviceOrientation.Current.LockOrientation(DeviceOrientations.Portrait);
        ICrossScreen.ScreenState.ShowStatusBar();
    }
}