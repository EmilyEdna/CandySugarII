namespace CandySugar.Controls;

public partial class I1 : ContentPage
{
    public I1()
    {
        InitializeComponent();
        Appearing += (obj, e) =>
        {
            Player.Source = (this.BindingContext as I1ViewModel).Route;
        };
    }
    private void GoEvent(object sender, EventArgs e)
    {
        var Btn = (sender as Button);
        var Param = Btn.CommandParameter.ToString().AsInt();
        if (Param == 1) Player.GoBack();
        else Player.GoForward();
    }
}