using CandySugar.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace CandySugar.Entry.ViewModels
{
    public class IndexViewModel : BaseViewModel
    {
        public IndexViewModel()
        {
            Author();
        }


        #region 方法
        /// <summary>
        /// 请求权限
        /// </summary>
        async void Author()
        {
            await RequestAsync<StorageWrite>();
            await RequestAsync<StorageRead>();
        }
        #endregion
    }
}
