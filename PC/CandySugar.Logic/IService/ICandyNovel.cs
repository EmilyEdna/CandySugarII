using CandySugar.Logic.Entity.CandyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Logic.IService
{
    public interface ICandyNovel
    {
        Task AddOrUpdate(CandyNovel input);
        Task Remove(CandyNovel input);
        Task<List<CandyNovel>> Get();
    }
}
