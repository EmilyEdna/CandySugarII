using CandySugar.Controls.ContentViewModel;
using CandySugar.Controls.MenuTemplateViewModel;
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

            Bind<CandyHistoryTemplateViewModel>().ToSelf();
            Bind<CandyImageTemplateViewModel>().ToSelf();
            Bind<CandyLogTemplateViewModel>().ToSelf();

            Bind<NovelViewModel>().ToSelf();
            Bind<LovelViewModel>().ToSelf();
            Bind<AnimeViewModel>().ToSelf();
            Bind<HnimeViewModel>().ToSelf();
            Bind<MangaViewModel>().ToSelf();
            Bind<ImageViewModel>().ToSelf();
            Bind<MusicViewModel>().ToSelf();
            Bind<AxgleViewModel>().ToSelf();
            Bind<MovieViewModel>().ToSelf();
        }
    }
}
