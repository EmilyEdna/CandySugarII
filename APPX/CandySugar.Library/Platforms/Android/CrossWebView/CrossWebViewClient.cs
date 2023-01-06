using Android.OS;
using Android.Webkit;

namespace CandySugar.Library.Platforms.Android.CrossWebView
{
    public class CrossWebViewClient : WebViewClient
    {
        public override void OnPageFinished(global::Android.Webkit.WebView view, string url)
        {
            view.LoadUrl("javascript:window.java_obj.ShowSource(document.getElementsByTagName('html')[0].innerHTML);");
            base.OnPageFinished(view, url);
        }
    }

    public class InJavaScriptLocalObj : Java.Lang.Object
    {
        [JavascriptInterface]
        public void ShowSource(string html)
        {
            var x = html;
        }
    }
}
