using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.WallPaper
{
    public class MessageNotify
    {
        /// <summary>
        /// 控件类型 1Chan控件 2Wall控件
        /// </summary>
        public int ControlType { get; set; }
        /// <summary>
        /// 控件参数
        /// </summary>
        public object ControlParam { get; set; }
    }
}
