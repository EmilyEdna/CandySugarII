using CandySugar.Controls.SysViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Controls.SysViewModels
{
    public class ComfirmViewModel : BaseViewModel
    {
        #region 属性
        public string Topic { get; set; }
        string _Msg;
        public string Msg
        {
            get => _Msg;
            set => SetProperty(ref _Msg, value);
        }
        #endregion

        #region 方法
        public async void CallBack(int input)
        {
            if (input == 1)
                MessagingCenter.Send(this, Topic, true);
            else
                MessagingCenter.Send(this, Topic, false);
            await MopupService.Instance.PopAllAsync();
        }
        #endregion
    }
}
