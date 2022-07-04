using CandySugar.Library;
using CandySugar.Resource.Properties;
using Sdk.Core;
using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CandySugar.Entry.ViewModels
{
    public class LoginViewModel : PropertyChangedBase
    {
        public IContainer Container;
        public IWindowManager WindowManager;
        public LoginViewModel(IContainer Container, IWindowManager WindowManager)
        {
            this.WindowManager = WindowManager;
            this.Container = Container;
        }
        #region Property
        private string _Account;
        public string Account
        {
            get => _Account;
            set => SetAndNotify(ref _Account, value);
        }

        private string _PassWord;
        public string PassWord
        {
            get => _PassWord;
            set => SetAndNotify(ref _PassWord, value);
        }

        private string _AccountTip;
        public string AccountTip
        {
            get => _AccountTip;
            set => SetAndNotify(ref _AccountTip, value);
        }

        private string _PassWordTip;
        public string PassWordTip
        {
            get => _PassWordTip;
            set => SetAndNotify(ref _PassWordTip, value);
        }

        private bool _IsAccountOpen;
        public bool IsAccountOpen
        {
            get => _IsAccountOpen;
            set => SetAndNotify(ref _IsAccountOpen, value);
        }

        private bool _IsPassWordOpen;
        public bool IsPassWordOpen
        {
            get => _IsPassWordOpen;
            set => SetAndNotify(ref _IsPassWordOpen, value);
        }
        #endregion

        public async void LoginAction()
        {
            if (Check())
            {
                WindowManager.ShowWindow(Container.Get<RootViewModel>());
                Application.Current.MainWindow.Close();
            }
            else
            {
                this.PassWordTip = "请检查密码是否正确！";
                this.AccountTip = "请检查账号是否正确！";
                this.IsAccountOpen = true;
                this.IsPassWordOpen = true;
                await Task.Delay(3000);
                await Task.Run(() =>
                {
                    this.IsAccountOpen = false;
                    this.IsPassWordOpen = false;
                });
            }
        }

        private bool Check()
        {
            if (this.Account == null || this.PassWord == null) return false;
            if (this.Account.ToLower().Equals("admin") && this.PassWord.ToLower().Equals("admin"))
            {
                CandySoft.Default.IsAdmin = true;
                return SdkLicense.Register(new SkdLicenseModel
                {
                    Account = "EmilyEdna",
                    Password = DateTime.Now.ToString("yyyyMMdd")
                });
            }
            else if (this.Account.ToLower().Equals("admin") && this.PassWord.ToLower().Equals("123456"))
            {
                CandySoft.Default.IsAdmin = false;
                return SdkLicense.Register(new SkdLicenseModel
                {
                    Account = "EmilyEdna",
                    Password = DateTime.Now.ToString("yyyyMMdd")
                });
            }
            else
                return false;
        }
    }
}
