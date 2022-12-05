using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace CandySugar.Library
{
    public class BRootEntity : BasicEntity
    {
        public string Name { get; set; }   
        public string Cover { get; set; }
        [Ignore]
        public List<BElementEntity> Collection { get; set; }
    }
}
