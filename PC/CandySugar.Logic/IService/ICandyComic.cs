using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Logic.IService
{
    public interface ICandyComic
    {
        Task Add(CandyComic input);
        Task Remove(CandyComic input);
        Task<List<CandyComic>> Get();
    }
}
