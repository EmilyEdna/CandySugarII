namespace CandySugar.Controls;

public partial class G2 : ContentPage
{
	G2ViewModel ViewModel { get; set; }
	public G2()
	{
		InitializeComponent();
		Appearing += (sender, evnt) =>
		{
            ViewModel = (G2ViewModel)this.BindingContext;
            HtmlWebViewSource Source = new()
            {
                Html = ViewModel.Content
            };
            Player.Source = Source;
            ExcuteJs();
        };
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
}