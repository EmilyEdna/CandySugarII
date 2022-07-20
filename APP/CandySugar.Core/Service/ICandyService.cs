﻿using CandySugar.Logic.ServiceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Logic.Service
{
    public interface ICandyService
    {
        #region 小说
        void AddOrAlterNovel(CandyNovel input);
        Pagination<CandyNovel> GetNovel(int PageIndex);
        void RemoveNovel(CandyNovel input);
        #endregion

        #region 轻小说
        void AddOrAlterLovel(CandyLovel input);
        Pagination<CandyLovel> GetLovel(int PageIndex);
        void RemoveLovel(CandyLovel input);
        #endregion

        #region 动漫
        void AddOrAlterAnime(CandyAnimeRoot input);
        Pagination<CandyAnimeRoot> GetAnime(int PageIndex);
        void RemoveAnime(CandyAnimeRoot input);
        #endregion
    }
}
