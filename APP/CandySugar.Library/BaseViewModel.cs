using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Library
{
    public class BaseViewModel : BindableBase
    {
        #region 字段
        public bool CanRefresh = true;
        #endregion

        #region 属性
        bool _IsBusy;
        public bool IsBusy
        {
            get => _IsBusy;
            set => SetProperty(ref _IsBusy, value);
        }

        private bool _IsRefreshing;
        public bool IsRefreshing
        {
            get => _IsRefreshing;
            set => SetProperty(ref _IsRefreshing, value);
        }

        int _Page;
        public int Page
        {
            get => _Page;
            set => SetProperty(ref _Page, value);
        }

        double _Total;
        public double Total
        {
            get => _Total;
            set => SetProperty(ref _Total, value);
        }
        #endregion

        #region 方法
        protected virtual void ShowBusy()
        {
            if (CanRefresh)
                IsBusy = true;
        }
        protected virtual void CloseBusy()
        {
            IsBusy = false;
            IsRefreshing = false;
        }
        #endregion
    }
}
