using CandySugar.Controls.ViewModels;

namespace CandySugar.Controls.Views;

public partial class MusicView : ContentPage
{
    public MusicView()
    {
        InitializeComponent();
        //ModuleBtn.Source = GetImageSource();
        MessagingCenter.Subscribe<MusicViewModel, bool>(this, "QueryStart", (sender, args) =>
        {
            if (args)
            {
                SearchHandle.Dispatcher.Dispatch(() =>
                {
                    SearchHandle.SetIsFocused(false);
                });
            }
        });
    }

    FontImageSource GetImageSource()
    {
        if (Module)
        {
            Module = false;
            return new FontImageSource
            {
                Color = Colors.Black,
                FontFamily = "Thin",
                Glyph = FontIcon.ArrowsRepeat,
                Size = 15,
            };
        }
        else
        {
            Module = true;
            return new FontImageSource
            {
                Color = Colors.Black,
                FontFamily = "Thin",
                Glyph = FontIcon.ArrowsRepeat1,
                Size = 15,
            };

        }
    }

    bool Module = true;
    private void TapEvent(object sender, EventArgs e)
    {
        //ModuleBtn.Source = GetImageSource();
    }
}