using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Library
{
    public class MusicPosition
    {
        /// <summary>
        /// 当前进度
        /// </summary>
        public TimeSpan Position { get; set; }
        /// <summary>
        /// 总进度
        /// </summary>
        public TimeSpan Duration { get; set; }
        public double PlayProgress { get; set; }
    }
}
