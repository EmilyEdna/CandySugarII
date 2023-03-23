using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Com.Library.Audios
{
    public class AudioLive
    {
        /// <summary>
        /// 实时时常 例如04:20
        /// </summary>
        public string LiveSpan { get; set; }
        /// <summary>
        /// 实时秒
        /// </summary>
        public double LiveSeconds { get; set; }
        /// <summary>
        /// 实时通道数据
        /// </summary>
        public List<double> LiveData { get; set; }
    }
}
