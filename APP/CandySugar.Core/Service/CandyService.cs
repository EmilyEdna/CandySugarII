using CandySugar.Logic.ServiceModel;
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
            var Data = base.Read<CandyNovel>();
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
            var Data = base.Read<CandyLovel>();
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
    }
}
