using CandySugar.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Foundation
{
    public class AskViewModel : ViewModelBase
    {
        public AskViewModel(BaseServices baseServices) : base(baseServices)
        {
        }

        #region Property
        public string Topic { get; set; }
        #endregion

        #region Command
        public DelegateCommand YesCommand => new(() => {
            MessagingCenter.Send(this, Topic, true);
        });
        #endregion
    }
}
