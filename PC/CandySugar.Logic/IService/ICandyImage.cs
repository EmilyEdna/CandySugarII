using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Logic.IService
{
    public interface ICandyImage
    {
        Task<bool> Add(CandyImage input);
        Task<Tuple<List<CandyImage>, int>> Get(int input);
        Task Remove(CandyImage input);
    }
}
