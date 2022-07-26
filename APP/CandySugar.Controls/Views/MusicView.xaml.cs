using CandySugar.Controls.ViewModels;

namespace CandySugar.Controls.Views;

public partial class MusicView : ContentPage
{
	public MusicView()
	{
		InitializeComponent();
        MessagingCenter.Subscribe<MusicViewModel,bool>(this, "QueryStart", (sender, args) => {
			if (args)
			{
				SearchHandle.Dispatcher.Dispatch(() =>
				{
					SearchHandle.SetIsFocused(false);
                });
            }
		});
    }
}