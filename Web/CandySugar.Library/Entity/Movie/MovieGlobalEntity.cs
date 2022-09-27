using CandySugar.Library.Entity.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Library.Entity.Movie
{
    public class MovieGlobalEntity : BaseEntity
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 封面
        /// </summary>
        public string Cover { get; set; }
        /// <summary>
        /// 上映时间
        /// </summary>
        public string ReleaseTime { get; set; }
        /// <summary>
        /// 路劲
        /// </summary>
        public string Route { get; set; }
        public bool IsSearch { get; set; }
        public int SearchId { get; set; }
        public string InfoRoute { get; set; }
    }
}
