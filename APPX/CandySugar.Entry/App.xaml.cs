using CandySugar.Entry.ViewModels;

namespace CandySugar.Entry
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }
        protected override Window CreateWindow(IActivationState activationState)
        {
            if (Windows.Count >= 1)
                return base.CreateWindow(activationState);
            else
            {
                activationState.Context.Services.GetService<INavigationService>().CreateBuilder().AddSegment<IndexViewModel>().Navigate();
                Thread.Sleep(2000);
                return base.CreateWindow(activationState);
            }
        }
    }
}