using CandySugar.Library.Entity.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Library.Entity.Lovel
{
    public class LovelViewEntity:BaseEntity
    {
        public string ViewRoute { get; set; }

        public string BookName { get; set; }

        public bool IsDown { get; set; }

        public string ChapterName { get; set; }

        public string ChapterRoute { get; set; }
    }
}
