using CandySugar.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CandySugar.Controls.ViewModels.AnimeViewModels
{
    public class AnimePlayViewModel:BaseViewModel
    {
        public override void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            HttpUtility.UrlDecode(query["Key"].ToString());
        }
    }
}
