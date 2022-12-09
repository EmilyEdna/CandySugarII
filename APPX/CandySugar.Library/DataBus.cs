using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Library
{
    public class DataBus
    {
        public const string NetErr = "请检查网络是否通畅";
        public static int Cache { get; set; } = 5;
        public static int QueryModule { get; set; } = 2;
        public static int Module { get; set; } = 1;
    }
}
