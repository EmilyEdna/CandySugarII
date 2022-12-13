using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Foundation
{
    public class BasicModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<X1, X1ViewModel>();
            containerRegistry.RegisterForNavigation<X2, X2ViewModel>();
            containerRegistry.RegisterForNavigation<X3, X3ViewModel>();
        }
    }
}
