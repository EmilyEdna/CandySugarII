using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Library.ViewModel.SysDto
{
    public class UserAttachDto
    {
        /// <summary>
        /// 请求类型 1:Multi 2:Reset 3:RPC
        /// </summary>
        public int RequestType { get; set; }
        /// <summary>
        /// 缓存时常 分钟
        /// </summary>
        public int Cache { get; set; }
        /// <summary>
        /// 数据源 0:SBDM 1:YSJDM
        /// </summary>
        public int AnimeSource { get; set; }
    }
}
