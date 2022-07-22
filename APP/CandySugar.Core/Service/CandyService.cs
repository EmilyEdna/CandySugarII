﻿using CandySugar.Logic.ServiceModel;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Logic.Service
{
    public class CandyService : DbJson, ICandyService
    {
        public CandyService() : base()
        {

        }

        #region 设置
        public void AddOrAlterOption(CandyOption input)
        {
            var Data = base.Read<CandyOption>();
            if (Data.Count > 0)
                Data.ForEach(item =>
                {
                    base.Delete(item);
                });
            base.InsertSingle(input);
        }
        public CandyOption GetOption()
        {
            return base.Read<CandyOption>().FirstOrDefault();
        }
        #endregion

        #region 小说
        public void AddOrAlterNovel(CandyNovel input)
        {
            var Data = base.Read<CandyNovel>();
            var CheckData = Data.FirstOrDefault(t => t.BookName == input.BookName && t.Author == input.Author);
            if (CheckData != null)
                base.Delete(CheckData);
            base.InsertSingle(input);
        }
        public Pagination<CandyNovel> GetNovel(int PageIndex)
        {
            var Data = base.Read<CandyNovel>().OrderByDescending(t => t.Span);
            return new Pagination<CandyNovel>
            {
                Result = Data.Skip((PageIndex - 1) * 10).Take(10).ToList(),
                Total = Math.Ceiling(Data.Count() / 10d)
            };
        }
        public void RemoveNovel(CandyNovel input)
        {
            base.Delete(input);
        }
        #endregion

        #region 轻小说
        public void AddOrAlterLovel(CandyLovel input)
        {
            var Data = base.Read<CandyLovel>();
            var CheckData = Data.FirstOrDefault(t => t.BookName == input.BookName && t.Author == input.Author);
            if (CheckData != null)
                base.Delete(CheckData);
            base.InsertSingle(input);
        }
        public Pagination<CandyLovel> GetLovel(int PageIndex)
        {
            var Data = base.Read<CandyLovel>().OrderByDescending(t => t.Span);
            return new Pagination<CandyLovel>
            {
                Result = Data.Skip((PageIndex - 1) * 10).Take(10).ToList(),
                Total = Math.Ceiling(Data.Count() / 10d)
            };
        }
        public void RemoveLovel(CandyLovel input)
        {
            base.Delete(input);
        }
        #endregion

        #region 动漫
        public void AddOrAlterAnime(CandyAnimeRoot input)
        {
            var Data = base.Read<CandyAnimeRoot>();
            var CheckData = Data.FirstOrDefault(t => t.Name == input.Name);
            if (CheckData != null)
                base.Delete(CheckData);
            base.InsertSingle(input);
        }
        public Pagination<CandyAnimeRoot> GetAnime(int PageIndex)
        {
            var Data = base.Read<CandyAnimeRoot>().OrderByDescending(t => t.Span);
            return new Pagination<CandyAnimeRoot>
            {
                Result = Data.Skip((PageIndex - 1) * 10).Take(10).ToList(),
                Total = Math.Ceiling(Data.Count() / 10d)
            };
        }
        public void RemoveAnime(CandyAnimeRoot input)
        {
            base.Delete(input);
        }
        #endregion

        #region ACG
        public void AddOrAlterHnime(CandyHnime input)
        {
            var Data = base.Read<CandyHnime>();
            var CheckData = Data.FirstOrDefault(t => t.Name == input.Name);
            if (CheckData != null)
                base.Delete(CheckData);
            base.InsertSingle(input);
        }
        public Pagination<CandyHnime> GetHnime(int PageIndex)
        {
            var Data = base.Read<CandyHnime>().OrderByDescending(t => t.Span);
            return new Pagination<CandyHnime>
            {
                Result = Data.Skip((PageIndex - 1) * 10).Take(10).ToList(),
                Total = Math.Ceiling(Data.Count() / 10d)
            };
        }
        public void RemoveHnime(CandyHnime input)
        {
            base.Delete(input);
        }
        #endregion

        #region 漫画
        public void AddOrAlterManga(CandyManga input)
        {
            var Data = base.Read<CandyManga>();
            var CheckData = Data.FirstOrDefault(t => t.Key == input.Key);
            if (CheckData != null)
                base.Delete(CheckData);
            base.InsertSingle(input);

        }
        public Pagination<CandyManga> GetManga(int PageIndex)
        {
            var Data = base.Read<CandyManga>().OrderByDescending(t => t.Span);
            return new Pagination<CandyManga>
            {
                Result = Data.Skip((PageIndex - 1) * 10).Take(10).ToList(),
                Total = Math.Ceiling(Data.Count() / 10d)
            };
        }
        public void RemoveManga(CandyManga input)
        {
            base.Delete(input);
        }
        #endregion

        #region 标签
        public void AddOrAlterTag(CandyLabel input)
        {
            var Data = base.Read<CandyLabel>();
            var CheckData = Data.FirstOrDefault(t => t.ZhLabel == input.ZhLabel && t.EnLabel == input.EnLabel);
            if (CheckData != null)
                base.Delete(CheckData);
            base.InsertSingle(input);
        }
        public Pagination<CandyLabel> GetTag(int PageIndex)
        {
            var Data = base.Read<CandyLabel>().OrderByDescending(t => t.Span);
            return new Pagination<CandyLabel>
            {
                Result = Data.Skip((PageIndex - 1) * 10).Take(10).ToList(),
                Total = Math.Ceiling(Data.Count() / 10d)
            };
        }
        public List<CandyLabel> SearcheTag(string input)
        {
            return base.Read<CandyLabel>().Where(t => t.ZhLabel.Contains(input) || t.EnLabel.Contains(input)).OrderByDescending(t => t.Span).ToList();
        }
        public void RemoveTag(CandyLabel input)
        {
            base.Delete(input);
        }
        #endregion

        #region 壁纸
        public void AddOrAlterImage(CandyImage input)
        {
            var Data = base.Read<CandyImage>();
            var CheckData = Data.FirstOrDefault(t => t.Preview == input.Preview && t.Original == input.Original);
            if (CheckData != null)
                base.Delete(CheckData);
            base.InsertSingle(input);
        }
        public Pagination<CandyImage> GetImage(int PageIndex)
        {
            var Data = base.Read<CandyImage>().OrderByDescending(t => t.Span);
            return new Pagination<CandyImage>
            {
                Result = Data.Skip((PageIndex - 1) * 10).Take(10).ToList(),
                Total = Math.Ceiling(Data.Count() / 10d)
            };
        }
        public void RemoveImage(CandyImage input)
        {
            base.Delete(input);
        }
        #endregion
    }
}