using CandySugar.Library.Entity.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Library.Entity.Manga
{
    public class MangaGlobalEntity : BaseEntity
    {
        /// <summary>
        /// 封面
        /// </summary>
        public string Cover { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Route { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 分类地址
        /// </summary>
        public string CategoryRoute { get; set; }
        public bool IsSearch { get; set; }
    }
}
