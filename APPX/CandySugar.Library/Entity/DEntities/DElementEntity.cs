using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Library
{
    public class DElementEntity : BasicEntity
    {
        public Guid DRootId { get; set; }
        public string Title { get; set; }
        public string Route { get; set; }
    }
}
