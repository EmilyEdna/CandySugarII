using CandySugar.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Logic
{
    public interface IService
    {
        #region Log
        Task AddLog(string Info, Exception Stack);
        Task ClearLog();
        Task<List<LogEntity>> QueryLog();
        #endregion

        #region B
        Task<BRootEntity> BAdd(BRootEntity root);
        Task BRemove(Guid root);
        Task<List<BRootEntity>> BQuery();
        #endregion

        #region C
        Task<bool> CAdd(CRootEntity root);
        Task CRemove(Guid root);
        Task<List<CRootEntity>> CQuery();
        #endregion

        #region D
        Task<bool> DAdd(DRootEntity root);
        Task DRemove(Guid root);
        Task<List<DRootEntity>> DQuery();
        #endregion

        #region E
        Task<bool> EAdd(ERootEntity root);
        Task ERemove(Guid root);
        Task<List<ERootEntity>> EQuery();
        #endregion

        #region F
        Task<bool> FAdd(FRootEntity root);
        Task FRemove(Guid root);
        Task<List<FRootEntity>> FQuery();
        #endregion
    }
}
