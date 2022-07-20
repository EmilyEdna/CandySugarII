using CandySugar.Controls.ViewModels.AnimeViewModels;

namespace CandySugar.Controls.Views.AnimeViews;

public partial class AnimePlayView : ContentPage
{
    public AnimePlayView()
    {
        InitializeComponent();
    }

    AnimePlayViewModel ViewModel;
    private void LoadingEvent(object sender, EventArgs e)
    {
        ViewModel = (AnimePlayViewModel)this.BindingContext;
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