using CandySugar.Controls;
using CandySugar.Entry.ViewModels;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Index = CandySugar.Entry.Views.Index;

namespace CandySugar.Entry
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>().UseMauiCommunityToolkit()
                .UsePrism(prism => prism.ConfigureModuleCatalog(moduleCatalog =>
                {
                    //配置模块目录
                    moduleCatalog.AddModule<ControlsModule>();
                })
                .RegisterTypes(containerRegistry =>
                {
                    containerRegistry.RegisterGlobalNavigationObserver();
                    containerRegistry.RegisterForNavigation<Index>();
                })
                .OnAppStart(navigationService => navigationService.CreateBuilder()
                    .AddSegment<IndexViewModel>()
                    .Navigate(HandleNavigationError)))
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }

        private static void HandleNavigationError(Exception ex)
        {
            Console.WriteLine(ex);
            System.Diagnostics.Debugger.Break();
        }
    }
}