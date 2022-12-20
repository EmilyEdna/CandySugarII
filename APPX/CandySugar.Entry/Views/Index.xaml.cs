using CandySugar.Controls;
using CandySugar.Entry.ViewModels;

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

    private void CloseLeftEvent(object sender, EventArgs e)
    {
        Left.IsPresented = false;
    }
}