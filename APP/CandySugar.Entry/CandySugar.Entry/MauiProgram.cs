using CandySugar.Entry.Views;
using Prism.Ioc;

namespace CandySugar.Entry
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            return MauiApp.CreateBuilder().UsePrismApp<App>(prism =>
            {
                prism.RegisterTypes(containerRegistry =>
                {
                    containerRegistry.RegisterForNavigation<LoginView>().RegisterInstance(SemanticScreenReader.Default);
                }).OnAppStart("NavigationPage/LoginView");
            }).ConfigureFonts(fonts =>
            {
                fonts.AddFont("DroidSans.ttf", "Sans");
            }).Build();
        }
    }
}
