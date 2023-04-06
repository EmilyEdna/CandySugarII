namespace CandySugar.WallPaper.ViewModels
{
    public class MainViewModel : PropertyChangedBase
    {
        private List<string> Builder;
        private List<string> RealLocal;
        public MainViewModel()
        {
            SdkLicense.Register(new SdkLicenseModel
            {
                Account = "EmilyEdna",
                Password = DateTime.Now.ToString("yyyyMMdd")
            });
            ComponentControl = Module.IocModule.Resolve<WallhavView>();
            MenuIndex = new()
            {
                new MenuInfo { Key = 1, Value = "wallhaven" },
                new MenuInfo { Key = 2, Value = "konachan" }
            };
            GenericDelegate.HandleAction = new(obj =>
            {
                Builder = ((List<string>)obj);
                if (Builder.Count >= 1)
                {
                    if (!MenuIndex.Any(t => t.Key == 3))
                        MenuIndex.Add(new MenuInfo { Key = 3, Value = "制作相册" });
                }
                if (Builder.Count <= 0)
                    MenuIndex.RemoveAt(2);
            });
        }

        #region Property
        private Control _ComponentControl;
        public Control ComponentControl
        {
            get => _ComponentControl;
            set => SetAndNotify(ref _ComponentControl, value);
        }
        private ObservableCollection<MenuInfo> _MenuIndex;
        /// <summary>
        /// 平台菜单
        /// </summary>
        public ObservableCollection<MenuInfo> MenuIndex
        {
            get => _MenuIndex;
            set => SetAndNotify(ref _MenuIndex, value);
        }
        #endregion

        #region Command
        public void ActiveCommand(int key)
        {
            if (key == 3)
                BuilderVideoPicture();
        }
        #endregion

        #region Method
        private void BuilderVideoPicture()
        {
            if (Builder != null)
            {
                RealLocal = new List<string>();
                //判断本地文件是否存在
                Builder.ForEach(item =>
                {
                    var fileName = DownUtil.FilePath(item, FileTypes.Png, "WallPaper");
                    if (File.Exists(fileName)) RealLocal.Add(fileName);
                });
                //没有被删除真实存在的文件
                if (RealLocal.Count > 0)
                {
                    //异步制作MP4
                    Task.Run(async () =>
                    {
                        var catalog = Path.Combine(CommonHelper.DownloadPath, "WallPaper");
                        var res = await RealLocal.ImageToVideo(catalog);
                        if (res) Application.Current.Dispatcher.Invoke(() =>
                        {
                            new ScreenDownNofityView(CommonHelper.DownloadFinishInformation, catalog).Show();
                        });
                    });
                }
            }
        }
        #endregion
    }
}
