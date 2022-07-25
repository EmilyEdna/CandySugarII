using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Controls.SysViewModels
{
    public class LogViewModel : BaseViewModel
    {
        ICandyService CandyService;
        public LogViewModel()
        {
            this.Page = 1;
            CandyService = CandyContainer.Instance.Resolve<ICandyService>();
            Query();
        }

        #region 属性
        ObservableCollection<CandyLog> _Root;
        public ObservableCollection<CandyLog> Root
        {
            get => _Root;
            set => SetProperty(ref _Root, value);
        }
        #endregion

        #region 命令
        public DelegateCommand LoadMoreAction => new(() =>
        {
            this.Page += 1;
            if (this.Page > Total) return;
            Query();
        });

        public DelegateCommand RefreshAction => new(() => {
            this.Page = 1;
            Root = new ObservableCollection<CandyLog>();
            this.Query();
        });
        #endregion

        #region 方法
        void Query()
        {
            var result = CandyService.GetLog(this.Page);
            Total = result.Total;
            if (Root == null)
                Root = new ObservableCollection<CandyLog>(result.Result);
            else
                result.Result.ForEach(item =>
                {
                    Root.Add(item);
                });
        }
        #endregion
    }
}
