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
        var result = await FilePicker.Default.PickMultipleAsync();

        if (result.Count() > 0)
        {
            foreach (var item in result)
            {
                if (item.ContentType.Equals("audio/mpeg") && item.FileName.ToLower().EndsWith("mp3"))
                {
                    (this.BindingContext as IndexViewModel).AddLocalMusic(item.FileName, item.FullPath);
                }
            }
        }
    }
}