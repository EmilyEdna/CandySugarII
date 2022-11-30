using CandySugar.Controls;

namespace CandySugar.Entry.Views;

public partial class Index : CandyUIPage
{
    public Index()
    {
        InitializeComponent();
    }

    private void PopupLeftEvent(object sender, EventArgs e)
    {
        Left.IsPresented = true;
    }
}