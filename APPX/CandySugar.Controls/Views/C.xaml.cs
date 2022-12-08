namespace CandySugar.Controls;

public partial class C : CandyUIView
{
    public C()
    {
        InitializeComponent();
    }

    private void OpenEvnet(object sender, TappedEventArgs e)
    {
        Bottom.IsPresented = true;
    }
}