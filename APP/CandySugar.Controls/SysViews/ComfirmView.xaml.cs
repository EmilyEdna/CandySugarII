using CandySugar.Controls.SysViewModels;
using Mopups.Pages;

namespace CandySugar.Controls.SysViews;

public partial class ComfirmView : PopupPage
{
    public ComfirmView()
    {
        InitializeComponent();
    }

    private void ConfirmEvent(object sender, EventArgs e)
    {
        var ViewModel = (this.BindingContext as ComfirmViewModel);
        var param = (sender as Button).CommandParameter.ToString().AsInt();
        ViewModel.CallBack(param);
    }
}