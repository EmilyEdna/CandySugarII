using CandySugar.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Logic
{
    public interface IBService
    {
        Task<BRootEntity> Add(BRootEntity root);
        Task Remove(Guid root);
        Task Alter(BElementEntity root);
        Task<List<BRootEntity>> Query(string key);
    }
}
