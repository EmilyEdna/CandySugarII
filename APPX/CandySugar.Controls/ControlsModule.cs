using Sdk.Core;

namespace CandySugar.Controls
{
    public class ControlsModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            SdkOption.EnableLog = true;
            SdkOption.UseRealRoute= true;
            SdkLicense.Register(new SdkLicenseModel
            {
                Account = "EmilyEdna",
                Password = DateTime.Now.ToString("yyyyMMdd")
            });
            DataCenter.RegistFunc();
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<B1, B1ViewModel>();
            containerRegistry.RegisterForNavigation<B2, B2ViewModel>();

            containerRegistry.RegisterForNavigation<C1, C1ViewModel>();
        }
    }
}
