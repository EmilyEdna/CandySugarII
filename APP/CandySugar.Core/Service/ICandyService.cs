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
        #region 小说
        void AddOrAlterNovel(CandyNovel input);
        Pagination<CandyNovel> GetNovel(int PageIndex);
        void RemoveNovel(CandyNovel input);
        #endregion
    }
}
