using CandySugar.Controls.ViewModels.HnimeViewModels;

namespace CandySugar.Controls.Views.HnimeViews;

public partial class HnimePlayView : ContentPage
{
	public HnimePlayView()
{
		InitializeComponent();
	}
    HnimePlayViewModel ViewModel;
    private void LoadingEvent(object sender, EventArgs e)
    {
        ViewModel = (HnimePlayViewModel)this.BindingContext;
        HtmlWebViewSource Source = new()
        {
            Html = ViewModel.Content
        };
        Player.Source = Source;
        ExcuteJs();
    }
    async void ExcuteJs()
    {
        while (!Player.IsLoaded)
        {
            await Task.Delay(500);
        }
        if (Player.IsLoaded)
            await Player.EvaluateJavaScriptAsync($"Play('{ViewModel.Route}')");
    }

    private void ExitingEvent(object sender, EventArgs e)
    {
        ViewModel.Show();
    }
}