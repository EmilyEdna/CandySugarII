using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Com.Options.ComponentGeneric
{
    /// <summary>
    /// 组件泛用委托
    /// </summary>
    public class GenericDelegate
    {
        /// <summary>
        /// 长宽通知委托
        /// </summary>
        public static Action<double, double> InformationAction { get; set; }
    }
}
