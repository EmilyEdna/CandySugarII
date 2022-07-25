using CandySugar.Controls;
using CandySugar.Entry.Views;
using CandySugar.Library;
using CandySugar.Logic.Common;
using CandySugar.Logic.Service;
using XExten.Advance.LinqFramework;

namespace CandySugar.Entry.ViewModels
{
    public class LoginViewModel : BindableBase
    {
        INavigationService NavigationService;
        ICandyService CandyService;
        public LoginViewModel(INavigationService NavigationService)
        {
            CandyService = CandyContainer.Instance.Resolve<ICandyService>();
            CandySoft.ScreenWidth = DeviceDisplay.Current.MainDisplayInfo.Width / 3;
            CandySoft.ScreenHeight = DeviceDisplay.Current.MainDisplayInfo.Height / 3;
            this.NavigationService = NavigationService;
            Init();
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
            if (this.Account.IsNullOrEmpty() || this.Pwd.IsNullOrEmpty())
            {
                StaticResource.PopToast("请填写正确的账户和密码！", true);
                return;
            }

            if (this.Account.ToLower().Equals("admin") && this.Pwd.ToLower().Equals("123456"))
            {
                this.Account = "EmilyEdna";
                this.Pwd = DateTime.Now.ToString("yyyyMMdd");
                CandySoft.IsAdmin = true;
            }
            else
                CandySoft.IsAdmin = false;
            var res = StaticResource.Login(this.Account, this.Pwd);
            if (res)
                Application.Current.MainPage = new IndexView();
            else
                StaticResource.PopToast("请填写正确的账户和密码！", true);
        });
        #endregion
        #region 方法
        void Init()
        {
            var Model = CandyService.GetOption();
            if (Model == null) return;
            CandySoft.Cache = Model.Cache == 0 ? CandySoft.Cache : Model.Cache;
            CandySoft.LightAccount = Model.LightAccount;
            CandySoft.IP = Model.IP.IsNullOrEmpty() ? CandySoft.IP : Model.IP;
            CandySoft.Module = Model.Module == 0 ? CandySoft.Module : Model.Module;
            CandySoft.LightPwd = Model.LightPwd;
            CandySoft.Port = Model.Port == 0 ? CandySoft.Port : Model.Port;
            CandySoft.Pwd = Model.Pwd.IsNullOrEmpty() ? CandySoft.Pwd : Model.Pwd;
            CandySoft.QueryModule = Model.QueryModule == 0 ? CandySoft.QueryModule : Model.QueryModule;
            CandySoft.User = Model.User.IsNullOrEmpty() ? CandySoft.User : Model.User;
            CandySoft.Wait = Model.Wait == 0 ? CandySoft.Wait : Model.Wait;
        }
        #endregion
    }
}
