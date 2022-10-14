using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Controls.ViewModels.ComicViewModels
{
    public class ComicWatchViewModel : BaseViewModel
    {
        static List<string> Data;
        public override void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            Data = query.Values.FirstOrDefault() as List<string>;
            this.Page = 0;
            this.Total = Data.Count;
            Route = new ObservableCollection<string>
            {
                Data.FirstOrDefault()
            };
        }
        #region 属性
        ObservableCollection<string> _Route;
        public ObservableCollection<string> Route
        {
            get => _Route;
            set => SetProperty(ref _Route, value);
        }
        #endregion

        #region 命令
        public DelegateCommand LoadMoreAction => new(() =>
        {
            this.Page += 1;
            if (this.Page > this.Total-1) return;
            Route.Add(Data[this.Page]);
        });
        #endregion
    }
}
