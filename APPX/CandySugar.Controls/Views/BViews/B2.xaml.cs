namespace CandySugar.Controls;

public partial class B2 : ContentPage
{
	B2ViewModel ViewModel { get; set; }
	public B2()
	{
		InitializeComponent();
		Disappearing += (sender, evnt) =>
		{
            ViewModel.Show();
        };
		Appearing += (sender, evnt) =>
		{
            ViewModel = (B2ViewModel)this.BindingContext;
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