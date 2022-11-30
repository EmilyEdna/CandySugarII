using Prism.Ioc;
using Sdk.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }
    }
}
