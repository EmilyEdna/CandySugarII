using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Library
{
    public class FRootEntity: BasicEntity
    {
        public string Name { get; set; }
        public string Cover { get; set; }
        [Ignore]
        public List<FElementEntity> Children { get; set; }
    }
}
