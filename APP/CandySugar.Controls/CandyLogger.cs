using Sdk.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Controls
{
    public class CandyLogger : ISdkLogger
    {
        ICandyService CandyService;
        public CandyLogger()
        {
            CandyService = CandyContainer.Instance.Resolve<ICandyService>();
        }

        public Task Log(string className, string methodInfo, Exception message)
        {
            CandyService.AddLog(new CandyLog
            {
                Message = className,
                Stack = methodInfo
            });
            return Task.CompletedTask;
        }
    }
}
