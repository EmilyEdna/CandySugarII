using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Library
{
    public class OptEntity : BasicEntity
    {
        public int Cache { get; set; }
        public int QueryModule { get; set; }
        public int Module { get; set; }
        public int Delay { get; set; }
    }
}
