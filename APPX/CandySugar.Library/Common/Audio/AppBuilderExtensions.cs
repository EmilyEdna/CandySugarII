
namespace CandySugar.Library.Common.Audio;
public static class AppBuilderExtensions
{
    public static MauiAppBuilder UseNativeMedia(this MauiAppBuilder builder)
    {
#if WINDOWS
        builder.Services.AddSingleton<INativeAudioService, Platforms.Windows.NativeAudioService>();
#elif ANDROID
        builder.Services.AddSingleton<INativeAudioService, Platforms.Android.Audio.NativeAudioService>();
#elif IOS
        builder.Services.AddSingleton<INativeAudioService, Platforms.iOS.NativeAudioService>();
#endif
        return builder;
    }

    public static void UseNativeMedia(this IContainerRegistry builder)
    {
#if WINDOWS
        builder.RegisterSingleton<INativeAudioService, Platforms.Windows.NativeAudioService>();
#elif ANDROID
        builder.RegisterSingleton<INativeAudioService, Platforms.Android.Audio.NativeAudioService>();
#elif IOS
        builder.RegisterSingleton<INativeAudioService, Platforms.iOS.NativeAudioService>();
#endif
    }
}