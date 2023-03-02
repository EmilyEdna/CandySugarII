using CandySugar.LightNovel.ViewModel;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.LightNovel
{
    public class LightNovelModule : StyletIoCModule
    {
        protected override void Load()
        {
            Bind<IndexViewModel>().ToSelf();
        }
    }
}
