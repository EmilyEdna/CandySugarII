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
        ICrossHand.Instance.UnRegistEvent();
        ICrossScreen.ScreenState.ShowStatusBar();
    }

    private void ConfirmEvent(object sender, EventArgs e)
    {
        var param = (sender as ImageButton).CommandParameter.ToString().AsInt();
        if (param == 2)
            Player.Reload();
        else
            StaticResource.ClearAd(Player);
    }
}