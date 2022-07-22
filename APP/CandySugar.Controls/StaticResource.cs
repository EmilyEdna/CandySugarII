using CandySugar.Controls.SysViewModels;
using CandySugar.Controls.SysViews;
using CandySugar.Controls.Views.LovelViews;
using CandySugar.Library;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Sdk.Component.Plugins;
using Sdk.Core;
using XExten.Advance.LinqFramework;

namespace CandySugar.Controls
{
    public class StaticResource
    {
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
        public static async void PopToast(string input)
        {
#if ANDROID
            Android.OS.Looper.Prepare();
#endif
            var toast = Toast.Make(input);
            await toast.Show();
#if ANDROID
            Android.OS.Looper.Loop();
#endif

        }
    }
}
