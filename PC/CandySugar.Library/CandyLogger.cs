using Sdk.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XExten.Advance.LinqFramework;

namespace CandySugar.Library
{
    public class CandyLogger : ISdkLogger
    {
        public Task Log(string className, string methodInfo, Exception message)
        {
            Serilog.Log.Logger.Error(message, $"CandySugar：【{className}】【{methodInfo}】，时间：【{DateTime.Now.ToFmtDate(3, "yyyy年MM月dd日 HH时mm分ss秒")}】");
            return Task.CompletedTask;
        }
    }
}
