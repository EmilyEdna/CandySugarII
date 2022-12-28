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

    private async void PickerEvent(object sender, EventArgs e)
    {
        var result = await FilePicker.Default.PickAsync();
        if (result.ContentType.Equals("audio/mpeg")&& result.FileName.ToLower().EndsWith("mp3"))
        {
            (this.BindingContext as IndexViewModel).AddLocalMusic(result.FileName,result.FullPath);
        }
    }
}