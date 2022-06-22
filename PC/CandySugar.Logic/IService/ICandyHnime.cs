using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Logic.IService
{
    public interface ICandyHnime
    {
        Task Add(CandyHnime input);
        Task Remove(CandyHnime input);
        Task<List<CandyHnime>> Get();
    }
}
