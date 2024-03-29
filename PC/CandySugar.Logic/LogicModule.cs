﻿global using CandySugar.Logic.Entity;
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
global using System.Text;
global using XExten.Advance.StaticFramework;
global using XExten.Advance.LinqFramework;

namespace CandySugar.Logic
{
    public class LogicModule : StyletIoCModule
    {
        protected override void Load()
        {
            Bind<ICandyComic>().To<CandyComicImpl>();
            Bind<ICandyMusic>().To<CandyMusicImpl>();
            Bind<ICandyNovel>().To<CandyNovelImpl>();
            Bind<ICandyLovel>().To<CandyLovelImpl>();
            Bind<ICandyAnime>().To<CandyAnimeImpl>();
            Bind<ICandyManga>().To<CandyMangaImpl>();
            Bind<ICandyLabel>().To<CandyLabelImpl>();
            Bind<ICandyImage>().To<CandyImageImpl>();
            Bind<ICandyHnime>().To<CandyHnimeImpl>();
            Bind<ICandyAxgle>().To<CandyAxgleImpl>();
            Bind<ICandyMovie>().To<CandyMovieImpl>();
            Bind<ICandyLog>().To<CandyLogImpl>();
        }
    }
}
