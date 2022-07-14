using NStandard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Library
{
    public class BaseViewModel : BindableBase, IQueryAttributable
    {
        #region 字段
        public bool CanRefresh = true;
        public bool Lock = false;
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
        string _KeyWord;
        public string KeyWord
        {
            get => _KeyWord;
            set
            {
                SetProperty(ref _KeyWord, value);
            }
        }
        #endregion

        #region 方法
        protected virtual void ShowBusy()
        {
            Lock = true;
            if (CanRefresh)
                IsBusy = true;
        }
        protected virtual void CloseBusy()
        {
            Lock = false;
            IsBusy = false;
            IsRefreshing = false;
        }

        protected virtual void SetRefresh(bool input=true) 
        {
            CanRefresh = input;
        }

        /// <summary>
        /// 获取URL的参数
        /// </summary>
        /// <param name="query"></param>
        public virtual void ApplyQueryAttributes(IDictionary<string, object> query)
        {
        }
        #endregion
    }
}
