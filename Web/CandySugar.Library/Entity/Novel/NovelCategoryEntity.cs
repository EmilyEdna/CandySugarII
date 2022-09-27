using CandySugar.Library.Entity.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Library.Entity.Novel
{
    public class NovelCategoryEntity : BaseEntity
    {
        public string DetailRoute { get; set; }

        public string BookName { get; set; }

        public string Author { get; set; }

        public string UpdateDate { get; set; }

        public string CategoryType { get; set; }
    }
}
