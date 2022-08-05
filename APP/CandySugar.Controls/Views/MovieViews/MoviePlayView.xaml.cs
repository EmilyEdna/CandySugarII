using CandySugar.Controls.ViewModels.MovieVoewModels;

namespace CandySugar.Controls.Views.MovieViews;

public partial class MoviePlayView : ContentPage
{
    public MoviePlayView()
    {
        InitializeComponent();
    }

    MoviePlayViewModel ViewModel;
    private void LoadingEvent(object sender, EventArgs e)
    {
        ViewModel = (MoviePlayViewModel)this.BindingContext;
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