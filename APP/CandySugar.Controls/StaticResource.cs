using CandySugar.Controls.SysViewModels;
using CandySugar.Controls.SysViews;
using CommunityToolkit.Maui.Core;
using Sdk.Component.Plugins;
using Sdk.Core;
using System.Text;

namespace CandySugar.Controls
{
    public class StaticResource
    {
        private static string[] ClassName = { "alert alert-dismissable alert-danger",
            "hd-text-icon",
            "top-nav",
            "well well-filters",
            "navbar navbar-inverse navbar-fixed-top",
            "nav nav-tabs",
            "tab-content m-b-20",
            "pull-left user-container",
            "pull-right big-views hidden-xs",
            "m-t-10 overflow-hidden",
            "col-md-4 col-sm-5",
            "footer-container",
            "col-lg-12",
            "fps60-text-icon",
            "btn btn-primary",
            "vote-box col-xs-7 col-sm-2 col-md-2",
            "pull-right m-t-15",
            "video-banner"};

        /// <summary>
        /// 注册Shell路由
        /// </summary>
        public static void RegistRoute()
        {
            typeof(StaticResource).Assembly.ExportedTypes.Where(t => t.BaseType == typeof(ContentPage))
                .Where(t => !t.Namespace.Equals("CandySugar.Controls.Views"))
                .Where(t => !t.Namespace.Equals("CandySugar.Controls.SysViews"))
                .ForEnumerEach(item =>
                {
                    Routing.RegisterRoute(item.Name, item);
                });
        }
        /// <summary>
        /// 注册视图
        /// </summary>
        /// <returns></returns>
        public static List<Type> RegistView()
        {
            return typeof(StaticResource).Assembly.ExportedTypes.Where(t => t.BaseType == typeof(ContentPage)).ToList();
        }
        /// <summary>
        /// 注册模型
        /// </summary>
        /// <returns></returns>
        public static List<Type> RegistViewModel()
        {
            return typeof(StaticResource).Assembly.ExportedTypes.Where(t => t.BaseType == typeof(BindableBase)).ToList();
        }
        /// <summary>
        /// 代理
        /// </summary>
        /// <returns></returns>
        public static SdkProxy Proxy()
        {
            return new SdkProxy
            {
                IP = CandySoft.IP,
                Port = CandySoft.Port,
                UserName = CandySoft.User,
                PassWord = CandySoft.Pwd
            };
        }
        /// <summary>
        /// Sdk请求方式
        /// </summary>
        /// <returns></returns>
        public static SdkImpl ImplType()
        {
            if (CandySoft.QueryModule == 1) return SdkImpl.Multi;
            else if (CandySoft.QueryModule == 2) return SdkImpl.Rest;
            else if (CandySoft.QueryModule == 3) return SdkImpl.RPC;
            else return SdkImpl.User;
        }
        /// <summary>
        /// 获取壁纸模式
        /// </summary>
        /// <returns></returns>
        public static string ImageModule()
        {
            if (CandySoft.Module == 1) return string.Empty;
            else if (CandySoft.Module == 2) return $"rating:safe";
            else if (CandySoft.Module == 3) return $"rating:questionable";
            else return $"rating:explicit";
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        public static bool Login(string Account, string Pwd)
        {
#if ANDROID
            SdkOption.UseRealRoute = true;
#endif
            return SdkLicense.Register(new SdkLicenseModel
            {
                Account = Account,
                Password = Pwd
            });
        }
        /// <summary>
        /// 弹出确认框
        /// </summary>
        /// <param name="input"></param>
        public static async void PopComfirm(string input, string Topic)
        {
            await MopupService.Instance.PushAsync(new ComfirmView
            {
                BindingContext = new ComfirmViewModel
                {
                    Topic = Topic,
                    Msg = input
                }
            });
        }
        /// <summary>
        /// 提示
        /// </summary>
        /// <param name="input"></param>
        /// <param name="IsLong"></param>
        public static async void PopToast(string input, bool IsLong = false)
        {
            try
            {
                await Toast.Make(input, IsLong ? ToastDuration.Long : ToastDuration.Short).Show();
            }
            catch (Exception)
            {
                //解决Toast在子线程问题
#if ANDROID
            Android.OS.Looper.Prepare();
#endif
                await Toast.Make(input, IsLong ? ToastDuration.Long : ToastDuration.Short).Show();
#if ANDROID
            Android.OS.Looper.Loop();
#endif
            }
        }
        public static void ClearAd(WebView WebView)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in ClassName)
            {
                sb.Append($"$(document.getElementsByClassName('{item}')).remove();");
            }
            sb.Append("$(document.getElementById('ps32-container')).remove();");
            sb.Append("$(document.getElementsByTagName('iframe')).remove();");
            sb.Append("$('div[style*=\"position:absolute;left:18px;display: block;font-size:10px;\"]').remove();");
            sb.Append("$('div[style*=\"position:absolute;right:18px; display: block;font-size:10px;\"]').remove();");
            sb.Append("$('#wrapper').css('padding-bottom','0px');");
            sb.Append("$('body').css('padding-top','0px');");
            sb.Append("$('#video-player').css({'max-width':'1190px','width':'1190px','margin-left':'-30px'});");
            WebView.EvaluateJavaScriptAsync(sb.ToString());
        }
    }
}
