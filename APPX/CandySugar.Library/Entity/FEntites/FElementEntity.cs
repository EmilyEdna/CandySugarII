using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Library
{
    public class FElementEntity : BasicEntity
    {
        public Guid FRootId { get; set; }
        public bool IsDown { get; set; }
        public string Name { get; set; }
        public string Route { get; set; }
    }
}
