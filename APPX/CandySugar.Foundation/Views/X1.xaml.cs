using XExten.Advance.LinqFramework;
using HtmlAgilityPack;

namespace CandySugar.Foundation;

public partial class X1 : ContentPage
{
    X1ViewModel ViewModel;
    public X1()
    {
        InitializeComponent();
        Player.Navigated += ViewEvent;
        Appearing += (o, e) =>
        {
            ViewModel = (this.BindingContext as X1ViewModel);
        };
    }


    private async void ViewEvent(object sender, WebNavigatedEventArgs e)
    {
        //if (e.Url.Contains("https://vidhub.top/vodplay/"))
        //{
        //    var Doc = await new HtmlWeb().LoadFromWebAsync(e.Url);
        //    var path = $"https://vidhub.top{Doc.DocumentNode.SelectSingleNode("//iframe[@id='player_if']").GetAttributeValue("src", "")}";
        //    Player.Source = path;
        //    await ViewModel.Navgation(path);
        //}
    }

    private void GoEvent(object sender, EventArgs e)
    {
        var Btn = (sender as Button);
        var Param = Btn.CommandParameter.ToString().AsInt();
        if (Param == 1) Player.GoBack();
        else Player.GoForward();
    }
}