using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Com.Library.Internet
{
    public class InternetWork
    {
        [DllImport("wininet")]
        //判断网络状况的方法,返回值true为连接，false为未连接  
        public extern static bool InternetGetConnectedState(out int conState, int reder);

        /// <summary>
        /// 成功连接网络返回【true】未连接返回【false】
        /// </summary>
        public static bool GetNetworkState=> InternetGetConnectedState(out int i, 0);
    }
}
