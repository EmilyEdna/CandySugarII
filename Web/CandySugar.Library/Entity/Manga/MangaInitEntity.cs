using CandySugar.Library.Entity.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Library.Entity.Manga
{
    public class MangaInitEntity : BaseEntity
    {
        /// <summary>
        /// 类别
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 分类短链接
        /// </summary>
        public string Route { get; set; }
    }
}
