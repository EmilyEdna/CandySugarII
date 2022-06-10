using CandySugar.Logic.IService;
using CandySugar.Logic.Serivce;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Logic
{
    public class LogicModule: StyletIoCModule
    {
        protected override void Load()
        {
            Bind<ICandyMusic>().To<CandyMusic>();
        }
    }
}
