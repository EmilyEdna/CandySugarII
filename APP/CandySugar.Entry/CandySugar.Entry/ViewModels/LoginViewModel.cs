using Sdk.Core;

namespace CandySugar.Entry.ViewModels
{
    public class LoginViewModel : BindableBase
    {
        private ISemanticScreenReader ScreenReader;

        public LoginViewModel(ISemanticScreenReader ScreenReader)
        {
            this.ScreenReader = ScreenReader;
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
            var res = SdkLicense.Register(new SkdLicenseModel
            {
                Account = this.Account,
                Password = this.Pwd
            });
        });
        #endregion
    }
}
