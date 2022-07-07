﻿using CandySugar.Entry.Views;
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
                    containerRegistry.RegisterForNavigation<LoginView>();
                }).OnAppStart("NavigationPage/LoginView");
            }).ConfigureFonts(fonts =>
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
