global using CandySugar.Logic.Entity;
global using CandySugar.Logic.Entity.CandyEntity;
global using CandySugar.Logic.IService;
global using CandySugar.Logic.Serivce;
global using SqlSugar;
global using StyletIoC;
global using System;
global using System.Collections.Generic;
global using System.IO;
global using System.Linq;
global using System.Threading.Tasks;
global using XExten.Advance.StaticFramework;

namespace CandySugar.Logic
{
    public class LogicModule : StyletIoCModule
    {
        protected override void Load()
        {
            Bind<ICandyMusic>().To<CandyMusicImpl>();
            Bind<ICandyNovel>().To<CandyNovelImpl>();
            Bind<ICandyLovel>().To<CandyLovelImpl>();
            Bind<ICandyAnime>().To<CandyAnimeImpl>();
            Bind<ICandyManga>().To<CandyMangaImpl>();
        }
    }
}
