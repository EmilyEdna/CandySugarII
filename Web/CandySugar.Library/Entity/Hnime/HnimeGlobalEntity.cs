using CandySugar.Library.Entity.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Library.Entity.Hnime
{
    public class HnimeGlobalEntity:BaseEntity
    {
        public string Watch { get; set; }
        public string Cover { get; set; }
        public string Duration { get; set; }
        public string PlayCount { get; set; }
        public string UpdateTime { get; set; }
        public string Property { get; set; }
        public string Title { get; set; }
        public bool IsSearch { get; set; }
        public string InfoRoute { get; set; }
    }
}
