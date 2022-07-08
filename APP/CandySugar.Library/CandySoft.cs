using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Library
{
    public class CandySoft
    {
        public static bool IsAdmin { get; set; }
        public static int Cache { get; set; } = 5;
        public static IConnectivity NetState => Connectivity.Current;
        public static string IP { get; set; } = string.Empty;
        public static int Port { get; set; } = -1;
        public static string User { get; set; }= string.Empty;
        public static string Pwd { get; set; }= string.Empty;
        public static int QueryModule { get; set; } = 1;
        public static int Wait { get; set; } = 300;
    }
}
