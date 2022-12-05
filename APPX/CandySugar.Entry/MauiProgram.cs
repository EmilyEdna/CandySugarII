﻿using CandySugar.Controls;
using CandySugar.Entry.ViewModels;
using CandySugar.Library;
using CandySugar.Logic;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using UraniumUI;
using Index = CandySugar.Entry.Views.Index;

namespace CandySugar.Entry
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder.UseMauiApp<App>().UseMauiCommunityToolkit().UseUraniumUI()
                .UsePrism(prism => prism.ConfigureModuleCatalog(moduleCatalog =>
                {
                    //配置模块目录
                    moduleCatalog.AddModule<ControlsModule>();
                    moduleCatalog.AddModule<LibraryModule>();
                    moduleCatalog.AddModule<LogicModule>();
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
                    fonts.AddFont("DroidSans.ttf", "Sans");
                    fonts.AddFont("FontAwesome6Brands.otf", "Brands");
                    fonts.AddFont("FontAwesome6Regular.otf", "Regular");
                    fonts.AddFont("FontAwesome6Solid.otf", "Solid");
                    fonts.AddFont("FontAwesome6Thin.otf", "Thin");
                    fonts.AddMaterialIconFonts();
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