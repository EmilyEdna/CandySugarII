using RestSharp;
using Sdk.Core;

namespace CandySugar.Controls
{
    public class ControlsModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            SdkOption.EnableLog = true;
            SdkOption.UseRealRoute = true;
            SdkLicense.Register(new SdkLicenseModel
            {
                Account = "EmilyEdna",
                Password = DateTime.Now.ToString("yyyyMMdd")
            });
            DataCenter.RegistFunc();
            HttpEvent.HttpActionEvent = new Action<HttpClient, Exception>((client, ex) =>
            {
                containerProvider.Resolve<IService>().AddLog("HttpClient请求异常",ex);
            });
            HttpEvent.RestActionEvent = new Action<RestClient, Exception>((client, ex) =>
            {
                containerProvider.Resolve<IService>().AddLog("RestClient请求异常", ex);
            });
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<B1, B1ViewModel>();
            containerRegistry.RegisterForNavigation<B2, B2ViewModel>();

            containerRegistry.RegisterForNavigation<C1, C1ViewModel>();

            containerRegistry.RegisterForNavigation<D1, D1ViewModel>();
            containerRegistry.RegisterForNavigation<D2, D2ViewModel>();

            containerRegistry.RegisterForNavigation<E1, E1ViewModel>();
            containerRegistry.RegisterForNavigation<E2, E2ViewModel>();

            containerRegistry.RegisterForNavigation<F1, F1ViewModel>();
            containerRegistry.RegisterForNavigation<F2, F2ViewModel>();

            containerRegistry.RegisterForNavigation<G1, G1ViewModel>();
            containerRegistry.RegisterForNavigation<G2, G2ViewModel>();
        }
    }
}
