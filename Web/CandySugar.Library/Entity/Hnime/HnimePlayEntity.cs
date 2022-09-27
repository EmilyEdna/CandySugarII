using CandySugar.Library.Entity.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Library.Entity.Hnime
{
    public class HnimePlayEntity : BaseEntity
    {
        public string Title { get; set; }
        public string Cover { get; set; }
        public string PlayRoute { get; set; }
        public bool IsPlaying { get; set; }
        public string WatchRoute { get; set; }
        public string PlayCount { get; set; }
    }
}
