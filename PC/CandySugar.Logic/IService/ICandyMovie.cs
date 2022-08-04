using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Logic.IService
{
    public interface ICandyMovie
    {
        Task AddOrUpdate(CandyMovie input);
        Task Remove(CandyMovie input);
        Task<List<CandyMovie>> Get();
    }
}
