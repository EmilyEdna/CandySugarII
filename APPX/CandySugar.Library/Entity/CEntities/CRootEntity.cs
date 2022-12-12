using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Library
{
    public class CRootEntity: BasicEntity
    {
        public string Priview { get; set; }
        public string Original { get; set; }
        [Ignore]
        public List<CElementEntity> Children { get; set; }
    }
}
