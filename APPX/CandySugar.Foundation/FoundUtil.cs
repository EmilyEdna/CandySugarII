using CandySugar.Library;
using Mopups.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Foundation
{
    public class FoundUtil
    {
        public static async void Open(BaseServices baseServices, Func<string> action)
        {
            await MopupService.Instance.PushAsync(new Ask
            {
                BindingContext = new AskViewModel(baseServices)
                {
                    Topic = action.Invoke()
                }
            });
        }
    }
}
