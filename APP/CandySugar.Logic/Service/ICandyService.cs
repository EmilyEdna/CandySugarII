using CandySugar.Logic.ServiceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Logic.Service
{
    public interface ICandyService
    {
        #region 设置
        void AddOrAlterOption(CandyOption input);
        CandyOption GetOption();
        #endregion

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

        #region Acg
        void AddOrAlterHnime(CandyHnime input);
        Pagination<CandyHnime> GetHnime(int PageIndex);
        void RemoveHnime(CandyHnime input);
        #endregion

        #region Comic
        void AddOrAlterComic(CandyComic input);
        Pagination<CandyComic> GetComic(int PageIndex);
        void RemoveComic(CandyComic input);
        #endregion

        #region 漫画
        void AddOrAlterManga(CandyManga input);
        Pagination<CandyManga> GetManga(int PageIndex);
        void RemoveManga(CandyManga input);
        #endregion

        #region 标签
        void AddOrAlterTag(CandyLabel input);
        Pagination<CandyLabel> GetTag(int PageIndex);
        List<CandyLabel> SearcheTag(string input);
        void RemoveTag(CandyLabel input);
        #endregion

        #region 壁纸
        void AddOrAlterImage(CandyImage input);
        Pagination<CandyImage> GetImage(int PageIndex);
        void RemoveImage(CandyImage input);
        #endregion

        #region 教育
        void AddOrAlterAxgle(CandyAxgle input);
        Pagination<CandyAxgle> GetAxgle(int PageIndex);
        void RemoveAxgle(CandyAxgle input);
        #endregion

        #region 日志
        void AddLog(CandyLog input);
        Pagination<CandyLog> GetLog(int PageIndex);
        void RemoveLog(CandyLog input);
        void ClearLog();
        #endregion

        #region 音乐
        void AddMusic(CandyMusic input);
        List<CandyMusic> GetMusic();
        void RemoveMusic(CandyMusic input);
        void ClearMusic();
        #endregion

        #region 电影
        void AddOrAlterMovie(CandyMovie input);
        Pagination<CandyMovie> GetMovie(int PageIndex);
        void RemoveMovie(CandyMovie input);
        #endregion
    }
}
