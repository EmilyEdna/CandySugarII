using CandySugar.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CandySugar.Controls.ViewModels.LovelViewModels
{
    public class LovelDetailViewModel : BaseViewModel
    {
        public override void ApplyQueryAttributes(IDictionary<string, object> query)
        {
          var x =  HttpUtility.UrlDecode(query["LovelDetailView"].ToString());
        }
    }
}
