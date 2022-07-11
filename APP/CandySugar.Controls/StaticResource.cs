using CandySugar.Controls.Views.LovelViews;
using CandySugar.Library;
using Sdk.Component.Plugins;
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
                .ForEnumerEach(item =>
                {
                    Routing.RegisterRoute(item.Name, item);
                });

            //Routing.RegisterRoute("LovelView/LovelDetailView", typeof(LovelDetailView));
            //Routing.RegisterRoute("LovelView/LovelDetailView/LovelContentView", typeof(LovelContentView));
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
    }
}
