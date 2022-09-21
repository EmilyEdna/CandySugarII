using CandySugar.Library.Entity.Lovel;
using CandySugar.Library.Entity.Novel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Library.Logic.IService
{
    public interface ILovelService
    {
        Task<List<LovelInitEntity>> Init();
    }
}
