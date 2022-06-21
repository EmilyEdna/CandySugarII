using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Logic.IService
{
    public interface ICandyLog
    {
        Task Add(string input);
        Task Remove();
        Task<List<CandyLog>> Get();
    }
}
