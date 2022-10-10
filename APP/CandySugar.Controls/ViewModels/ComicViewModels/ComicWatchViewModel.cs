using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Controls.ViewModels.ComicViewModels
{
    public class ComicWatchViewModel : BaseViewModel
    {
        public override void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            var data = query.Values.FirstOrDefault() as List<string>;
            Img = new ObservableCollection<string>(data);
        }
        #region 属性
        ObservableCollection<string> _Img;
        public ObservableCollection<string> Img
        {
            get => _Img;
            set => SetProperty(ref _Img, value);
        }
        #endregion
    }
}
