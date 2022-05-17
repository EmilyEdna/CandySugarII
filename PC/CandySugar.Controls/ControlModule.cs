using CandySugar.Controls.TemplateViewModel;
using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Controls
{
    public class ControlModule : StyletIoCModule
    {
        protected override void Load()
        {
            Bind<CandyHeadTemplateViewModel>().ToSelf();
            Bind<CandySilderTemplateViewModel>().ToSelf();
        }
    }
}
