using CandySugar.Controls.SysViewModels;

namespace CandySugar.Controls.SysViews;

public partial class OptionView : ContentPage
{
    public OptionView()
    {
        InitializeComponent();
    }
    OptionViewModel ViewModel;
    private void ModuleEvent(object sender, CheckedChangedEventArgs e)
    {
        ViewModel = this.BindingContext as OptionViewModel;
        ViewModel.Module = (sender as RadioButton).Value.ToString().AsInt();
    }

    private void QueryEvent(object sender, CheckedChangedEventArgs e)
    {
        ViewModel = this.BindingContext as OptionViewModel;
        ViewModel.QueryModule = (sender as RadioButton).Value.ToString().AsInt();
    }
}