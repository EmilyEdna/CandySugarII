using CandySugar.Library.AndroidCommon;
using CandySugar.Library.AndroidCommon.Device;
using CandySugar.Library.AndroidCommon.Screen;

namespace CandySugar.Controls.Views.ImageViews;

public partial class ImageDetailView : ContentPage
{
	public ImageDetailView()
	{
		InitializeComponent();
	}

	private void BackEvent(object sender, EventArgs e)
	{
        CrossDeviceOrientation.Current.LockOrientation(DeviceOrientations.Portrait);
        ICrossScreen.ScreenState.ShowStatusBar();
    }
}