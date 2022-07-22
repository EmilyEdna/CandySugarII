using CandySugar.Entry.Views;
using CandySugar.Logic.Common;
using CommunityToolkit.Maui;

namespace CandySugar.Entry
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            return MauiApp.CreateBuilder().ConfigurationService()
                .UsePrismApp<App>(prism =>
                {
                    prism.RegisterTypes(containerRegistry =>
                    {
                        containerRegistry.RegisterForNavigation<LoginView>();
                    }).OnAppStart("NavigationPage/LoginView");
                }).UseMauiCommunityToolkit().ConfigureFonts(fonts =>
                {
                    fonts.AddFont("DroidSans.ttf", "Sans");
                    fonts.AddFont("FontAwesome6Brands.otf", "Brands");
                    fonts.AddFont("FontAwesome6Regular.otf", "Regular");
                    fonts.AddFont("FontAwesome6Solid.otf", "Solid");
                    fonts.AddFont("FontAwesome6Thin.otf", "Thin");
                }).Build();
        }
    }
}
