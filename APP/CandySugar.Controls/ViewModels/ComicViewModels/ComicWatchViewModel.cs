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
            this.PageIndex = this.Page + 1;
            this.Total = Data.Count;
            Route = Data.FirstOrDefault();
        }
        #region 属性
        string _Route;
        public string Route
        {
            get => _Route;
            set => SetProperty(ref _Route, value);
        }
        int _PageIndex;
        public int PageIndex
        {
            get => _PageIndex;
            set => SetProperty(ref _PageIndex, value);
        }
        #endregion

        #region 命令
        public DelegateCommand NextAction => new(() =>
        {
            this.Page += 1;
            if (this.Page > this.Total - 1) return;
            this.PageIndex += 1;
            Route = Data[this.Page];
        });
        public DelegateCommand PreAction => new(() =>
        {
            if (this.Page <= 0) return;
            this.Page -= 1;
            this.PageIndex -= 1;
            Route = Data[this.Page];
        });
        #endregion
    }
}
