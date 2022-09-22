using Microsoft.Extensions.Logging;
using Sdk.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Library
{
    public class SdkLog : ISdkLogger
    {
        public Task Log(string className, string methodInfo, Exception message)
        {
            Furion.Logging.Log.CreateLogger("SDK日志").LogError(message.Message, new { ClassName= className,Method= methodInfo });
            return Task.CompletedTask;
        }
    }
}
