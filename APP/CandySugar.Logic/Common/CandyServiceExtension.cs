using CandySugar.Logic.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Logic.Common
{
    public static class CandyServiceExtension
    {
        public static MauiAppBuilder ConfigurationService(this MauiAppBuilder builder)
        {
            CandyContainer.Instance.Regiest<ICandyService, CandyService>();
            return builder;
        }
    }
}
