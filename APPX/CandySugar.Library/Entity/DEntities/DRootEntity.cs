using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Library
{
    public class DRootEntity: BasicEntity
    {
        public string Cover { get; set; }
        public string Name { get; set; }
        [Ignore]
        public List<DElementEntity> Children { get; set; }
    }
}
