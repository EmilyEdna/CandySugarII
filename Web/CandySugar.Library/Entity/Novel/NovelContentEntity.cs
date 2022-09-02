using CandySugar.Library.Entity.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Library.Entity.Novel
{
    public class NovelContentEntity:BaseEntity
    {
        public string PreviousPage { get; set; }
        
        public string NextPage { get; set; }
        
        public string PreviousChapter { get; set; }

        public string NextChapter { get; set; }

        public string Content { get; set; }

        public string ChapterName { get; set; }
    }
}
