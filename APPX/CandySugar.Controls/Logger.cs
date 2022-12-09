using Sdk.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Controls
{
    public class Logger : ISdkLogger
    {
        public Task Log(string className, string methodInfo, Exception ex)
        {
            return new Service().AddLog($"类名:【{className}】,方法:【{methodInfo}】", ex);
        }
    }
}
