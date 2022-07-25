using CandySugar.Controls;
using CandySugar.Controls.SysViewModels;
using CandySugar.Library;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            VersionCode = AppInfo.Current.VersionString;
            Author();
            AppVersion();
        }

        #region 属性
        string _VersionCode;
        public string VersionCode
        {
            get => _VersionCode;
            set => SetProperty(ref _VersionCode, value);
        }
        #endregion

        #region 方法
        /// <summary>
        /// 请求权限
        /// </summary>
        async void Author()
        {
            await RequestAsync<StorageWrite>();
            await RequestAsync<StorageRead>();
        }
        /// <summary>
        /// App版本
        /// </summary>
        void AppVersion()
        {
            if (StaticResource.CheckVersion())
            {
                StaticResource.PopComfirm("确认升级", nameof(IndexViewModel));
                MessagingCenter.Subscribe<ComfirmViewModel, bool>(this, nameof(IndexViewModel), (sender, args) =>
                {
                    if (args == true)
                    {
                        StaticResource.InstallApk();
                    }
                });
            }
        }
        #endregion
    }
}
