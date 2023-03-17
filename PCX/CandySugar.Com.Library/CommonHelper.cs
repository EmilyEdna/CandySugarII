using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Com.Library
{
    public class CommonHelper
    {
        /// <summary>
        /// 程序目录
        /// </summary>
        public static string AppPath => AppDomain.CurrentDomain.BaseDirectory;
        /// <summary>
        /// 配置目录
        /// </summary>
        public static string OptionPath => "Component";
        /// <summary>
        /// 配置文件
        /// </summary>
        public static List<string> OptionFile = new List<string> 
        {
            "Component.json"
        };
    }
}
