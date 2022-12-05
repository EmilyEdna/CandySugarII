using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Library
{
    public class BElementEntity : BasicEntity
    {
        public Guid BRootId { get; set; }
        public string CollectName { get; set; }
        public string WatchRoute { get; set; }
        public bool IsWatching { get; set; }
    }
}
