using CandySugar.Controls;

namespace CandySugar.Entry.Views;

public partial class IndexView : Shell
{
    public IndexView()
    {
        InitializeComponent();
        StaticResource.RegistRoute();
    }
}
