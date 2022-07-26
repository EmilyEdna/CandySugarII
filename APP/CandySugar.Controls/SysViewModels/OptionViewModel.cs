using XExten.Advance.CacheFramework.RunTimeCache;

namespace CandySugar.Controls.SysViewModels
{
    public class OptionViewModel : BaseViewModel
    {
        ICandyService CandyService;
        public OptionViewModel()
        {
            CandyService = CandyContainer.Instance.Resolve<ICandyService>();
            Init();
        }

        #region 属性
        int _Cache;
        public int Cache
        {
            get => _Cache;
            set => SetProperty(ref _Cache, value);
        }
        string _IP;
        public string IP
        {
            get => _IP;
            set => SetProperty(ref _IP, value);
        }
        int _Port;
        public int Port
        {
            get => _Port;
            set => SetProperty(ref _Port, value);
        }
        string _User;
        public string User
        {
            get => _User;
            set => SetProperty(ref _User, value);
        }
        string _Pwd;
        public string Pwd
        {
            get => _Pwd;
            set => SetProperty(ref _Pwd, value);
        }
        int _QueryModule;
        public int QueryModule
        {
            get => _QueryModule;
            set => SetProperty(ref _QueryModule, value);
        }
        int _Wait;
        public int Wait
        {
            get => _Wait;
            set => SetProperty(ref _Wait, value);
        }
        string _LightAccount;
        public string LightAccount
        {
            get => _LightAccount;
            set => SetProperty(ref _LightAccount, value);
        }
        string _LightPwd;
        public string LightPwd
        {
            get => _LightPwd;
            set => SetProperty(ref _LightPwd, value);
        }
        int _Module;
        public int Module
        {
            get => _Module;
            set => SetProperty(ref _Module, value);
        }
        #endregion

        #region 命令
        public DelegateCommand SaveAction => new(() =>
        {
            Logic();
        });
        public DelegateCommand CleanAction => new(() =>
        {
            MemoryCaches.RemoveAllCache();
        });
        #endregion

        #region 方法
        void Logic()
        {
            CandyService.AddOrAlterOption(new CandyOption
            {
                Cache = this.Cache,
                LightAccount = this.LightAccount,
                IP = this.IP,
                Module = this.Module,
                LightPwd = this.LightPwd,
                Port = this.Port,
                Pwd = this.Pwd,
                QueryModule = this.QueryModule,
                User = this.User,
                Wait = this.Wait
            });
        }
        void Init()
        {
            var Model = CandyService.GetOption();
            if (Model == null) return;
            this.Cache = Model.Cache;
            this.LightAccount = Model.LightAccount;
            this.IP = Model.IP;
            this.Module = Model.Module;
            this.LightPwd = Model.LightPwd;
            this.Port = Model.Port;
            this.Pwd = Model.Pwd;
            this.QueryModule = Model.QueryModule;
            this.User = Model.User;
            this.Wait = Model.Wait;
        }

        #endregion
    }
}
