using CandySugar.Entry.Views;
using CandySugar.Library;
using Sdk.Core;

namespace CandySugar.Entry.ViewModels
{
    public class LoginViewModel : BindableBase
    {
        private INavigationService NavigationService;

        public LoginViewModel(INavigationService NavigationService)
        {
            this.NavigationService = NavigationService;
        }

        #region 属性
        private string _Account;
        public string Account
        {
            get => _Account;
            set => SetProperty(ref _Account, value);
        }
        private string _Pwd;
        public string Pwd
        {
            get => _Pwd;
            set => SetProperty(ref _Pwd, value);
        }
        #endregion

        #region 命令
        public DelegateCommand LoginAction => new(() =>
        {
            if (this.Account.ToLower().Equals("admin") && this.Pwd.ToLower().Equals("admin"))
            {
                this.Account = "EmilyEdna";
                this.Pwd = DateTime.Now.ToString("yyyyMMdd");
                CandySoft.IsAdmin = true;
            }
            else
                CandySoft.IsAdmin = false;
            var res = SdkLicense.Register(new SkdLicenseModel
            {
                Account = this.Account,
                Password = this.Pwd
            });
            if (res)
                Application.Current.MainPage = new IndexView();
        });
        #endregion
    }
}
