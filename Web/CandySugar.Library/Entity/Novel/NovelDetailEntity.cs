using CandySugar.Library.Entity.Base;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Library.Entity.Novel
{
    public class NovelDetailEntity : BaseEntity
    {
        public int Total { get; set; }

        public string Cover { get; set; }

        public string BookName { get; set; }

        public string Author { get; set; }

        public DateTime LastUpdateTime { get; set; }

        public string Description { get; set; }

        public string Status { get; set; }

        public string BookType { get; set; }

        public string ShortRoute { get; set; }

        public string InfoRoute { get; set; }

        [SugarColumn(IsIgnore =true)]
        public List<NovelChapterEntity> Chapter { get; set; }
    }
    public class NovelChapterEntity : BaseEntity
    {
        public string ChapterName { get; set; }
        public string ChapterRoute { get; set; }
        public Guid KeyId { get; set; }
        public void Create(Guid input) 
        {
            KeyId = input;
            base.Create(true);
        }
    }
}
