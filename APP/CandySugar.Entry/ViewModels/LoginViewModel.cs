using CandySugar.Controls;
using CandySugar.Entry.Views;
using CandySugar.Library;

namespace CandySugar.Entry.ViewModels
{
    public class LoginViewModel : BindableBase
    {
        INavigationService NavigationService;
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
            //if (this.Account.ToLower().Equals("admin") && this.Pwd.ToLower().Equals("admin"))
            //{
            //    this.Account = "EmilyEdna";
            //    this.Pwd = DateTime.Now.ToString("yyyyMMdd");
            //    CandySoft.IsAdmin = true;
            //}
            //else
            CandySoft.IsAdmin = true;
            var res = StaticResource.Login("EmilyEdna", DateTime.Now.ToString("yyyyMMdd"));
            if (res)
                Application.Current.MainPage = new IndexView();
        });
        #endregion
    }
}
