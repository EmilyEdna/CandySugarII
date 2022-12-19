using Mopups.Pages;
using Mopups.Services;

namespace CandySugar.Foundation;

public partial class Ask : PopupPage
{
	public Ask()
	{
		InitializeComponent();
	}

    private async void CloseEvent(object sender, EventArgs e)
    {
        await MopupService.Instance.PopAllAsync();
    }

}