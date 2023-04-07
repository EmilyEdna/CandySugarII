namespace CandySugar.WallPaper.ViewModels
{
    public class MainViewModel : PropertyChangedBase
    {
        private List<WallhavSearchElementResult> WallhavBuilder;
        private List<string> RealLocal;
        private List<MenuInfo> Default = new List<MenuInfo> {
            new MenuInfo { Key = 3, Value = "下载选中" },
            new MenuInfo { Key = 4, Value = "删除选中" },
            new MenuInfo { Key = 5, Value = "制作相册" }
        };
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
                if (obj is List<WallhavSearchElementResult> wallhav)
                {
                    WallhavBuilder = wallhav;
                    if (WallhavBuilder.Count >= 1)
                    {
                        if (!MenuIndex.Any(t => t.Key == 3 || t.Key == 4 || t.Key == 5))
                            Default.ForEach(item => MenuIndex.Add(item));
                    }
                }
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
                DownSelectPicture();
            if (key == 4)
                RemoveSelectPicture();
            if (key == 5)
                BuilderVideoPicture();
        }
        #endregion

        #region Method
        private void BuilderVideoPicture()
        {
            if (WallhavBuilder != null)
            {
                RealLocal = new List<string>();
                //判断本地文件是否存在
                WallhavBuilder.ForEach(item =>
                {
                    var fileName = DownUtil.FilePath(item.Id, FileTypes.Png, "WallPaper");
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
        private void DownSelectPicture()
        {
            if (WallhavBuilder != null && WallhavBuilder.Count > 0)
            {
                Task.Run(() =>
                {
                    WallhavBuilder.ForEach(async item =>
                    {
                        var fileBytes = await (new HttpClient().GetByteArrayAsync(item.Original));
                        fileBytes.FileCreate(item.Id, FileTypes.Png, "WallPaper", (catalog, fileName) =>
                        {
                            new ScreenDownNofityView(CommonHelper.DownloadFinishInformation, catalog).Show();
                        });
                    });
                });
                WallhavBuilder.DeleteAndCreate("Wallhaven", FileTypes.Dat, "WallPaper");
            }
        }
        private void RemoveSelectPicture()
        {
            if (WallhavBuilder != null && WallhavBuilder.Count > 0)
            {
                WallhavBuilder.ForEach(item =>
                {
                    SyncStatic.DeleteFile(DownUtil.FilePath(item.Id, FileTypes.Png, "WallPaper"));
                    if (ComponentControl.DataContext is WallhavViewModel ViewModel)
                    {
                        ViewModel.CollectResult.Remove(item);
                    }
                });
                if (WallhavBuilder.Count <= 0)
                    Default.ForEach(item => MenuIndex.Remove(item));
                WallhavBuilder.DeleteAndCreate("Wallhaven", FileTypes.Dat, "WallPaper");
            }
        }
        #endregion
    }
}
