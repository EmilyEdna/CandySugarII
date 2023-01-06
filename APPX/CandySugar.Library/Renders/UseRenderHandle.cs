using Microsoft.Maui.Handlers;

namespace CandySugar.Library
{
    public static class UseRenderHandle
    {
        public static MauiAppBuilder UseRender(this MauiAppBuilder builder)
        {
            InitWebView();
            return builder;
        }

        static void InitWebView()
        {
            WebViewHandler.Mapper.AppendToMapping("CandyWebView", (handler, view) =>
            {
            });
        }
    }
}
