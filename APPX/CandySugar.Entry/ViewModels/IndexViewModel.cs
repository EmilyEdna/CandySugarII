using CandySugar.Entry.Resources.Styles;
using CandySugar.Foundation;
using CandySugar.Library.Common;
using CandySugar.Logic;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.Linq;
using XExten.Advance.HttpFramework.MultiFactory;
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace CandySugar.Entry.ViewModels
{
    public class IndexViewModel : ViewModelBase
    {
        BaseServices BaseServices;
        IService Service;
        public IndexViewModel(BaseServices baseServices) : base(baseServices)
        {
            BaseServices = baseServices;
            Menu = new ObservableCollection<MenuModel>(MenuModel.GetMenus());
        }
        public override async void OnAppearing()
        {
            Service = BaseServices.Container.Resolve<IService>();
            await Service.ClearLog();
            var Opt = await Service.OptFirst();
            if (Opt != null)
            {
                DataBus.Cache = Opt.Cache;
                DataBus.QueryModule = Opt.QueryModule;
                DataBus.Module = Opt.Module;
                DataBus.Delay = Opt.Delay;
            }
            Author();
        }

        public override void OnDisappearing()
        {
        }

        #region Property
        ObservableCollection<MenuModel> _Menu;
        public ObservableCollection<MenuModel> Menu
        {
            get { return _Menu; }
            set { SetProperty(ref _Menu, value); }
        }

        View _Content;
        public View Content
        {
            get { return _Content; }
            set { SetProperty(ref _Content, value); }
        }
        #endregion

        #region Command
        public DelegateCommand<string> Command => new(key =>
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                DataBus.NetErr.OpenToast();
                return;
            }

            switch (key)
            {
                case "新番":
                    this.Content = new A
                    {
                        BindingContext = new AViewModel(this.BaseServices)
                    };
                    break;
                case "动漫":
                    this.Content = new B
                    {
                        BindingContext = new BViewModel(this.BaseServices)
                    };
                    break;
                case "壁纸":
                    this.Content = new C
                    {
                        BindingContext = new CViewModel(this.BaseServices)
                    };
                    break;
                case "漫画":
                    this.Content = new D
                    {
                        BindingContext = new DViewModel(this.BaseServices)
                    };
                    break;
                case "小说":
                    this.Content = new E
                    {
                        BindingContext = new EViewModel(this.BaseServices)
                    };
                    break;
                case "文学":
                    this.Content = new F
                    {
                        BindingContext = new FViewModel(this.BaseServices)
                    };
                    break;
                case "电影":
                    this.Content = new G
                    {
                        BindingContext = new GViewModel(this.BaseServices)
                    };
                    break;
                case "音乐":
                    this.Content = new H
                    {
                        BindingContext = new HViewModel(this.BaseServices)
                    };
                    break;
                case "运动":
              
                    this.Content = new I
                    {
                        BindingContext = new IViewModel(this.BaseServices)
                    };
                    break;
                case "月亮":
                    this.Content = new J
                    {
                        BindingContext = new JViewModel(this.BaseServices)
                    };
                    break;
                case "太阳":
                    this.Content = new K
                    {
                        BindingContext = new KViewModel(this.BaseServices)
                    };
                    break;
                default:
                    break;
            }
        });
        public DelegateCommand<string> SearchCommand => new(input =>
        {
            if (Content is B BView)
            {
                var model = (BView.BindingContext as BViewModel);
                model.Key = input;
                Task.Run(() => model.QueryInit(false));
            }
            if (Content is C CView)
            {
                var model = (CView.BindingContext as CViewModel);
                model.Key = input;
                Task.Run(() => model.QueryInit(false));
            }
            if (Content is D DView)
            {
                var model = (DView.BindingContext as DViewModel);
                model.Key = input;
                Task.Run(() => model.QueryInit(false));
            }
            if (Content is E EView)
            {
                var model = (EView.BindingContext as EViewModel);
                model.Key = input;
                Task.Run(() => model.QueryInit(false));
            }
            if (Content is F FView)
            {
                var model = (FView.BindingContext as FViewModel);
                model.Key = input;
                Task.Run(() => model.QueryInit(false));
            }
            if (Content is G GView)
            {
                var model = (GView.BindingContext as GViewModel);
                model.Key = input;
                Task.Run(() => model.QueryInit(false));
            }
            if (Content is H HView)
            {
                var model = (HView.BindingContext as HViewModel);
                model.Key = input;
                Task.Run(() => model.QueryInit(false));
            }
            if (Content is I IView)
            {
                var model = (IView.BindingContext as IViewModel);
                model.Key = input;
                Task.Run(() => model.QueryInit(false));
            }
        });
        public DelegateCommand<string> NavCommand => new(input =>
        {
            switch (input)
            {
                case "1":
                    Nav.NavigateAsync(new Uri("X1", UriKind.Relative));
                    break;
                case "2":
                    Nav.NavigateAsync(new Uri("X2", UriKind.Relative));
                    break;
                case "3":
                    Nav.NavigateAsync(new Uri("X3", UriKind.Relative));
                    break;
                case "4":
                    Nav.NavigateAsync(new Uri("X4", UriKind.Relative));
                    break;
                case "5":
                    if (CheckVersion())
                    {
                        FoundUtil.Open(BaseServices, () => "ASK");
                        MessagingCenter.Subscribe<AskViewModel, bool>(this, "ASK", (sender, args) =>
                        {
                            if (args == true)
                                ICrossExtension.Instance.InstallApk();
                        });
                    }
                    break;
                case "6":
                    ICollection<ResourceDictionary> Theme = Application.Current.Resources.MergedDictionaries;
                    if (Theme.Any(t => t.GetType() == typeof(LightTheme)))
                    {
                        var Target = Theme.Where(t => t.GetType() == typeof(LightTheme)).FirstOrDefault();
                        Theme.Remove(Target);
                        Theme.Add(new DarkTheme());
                        return;
                    }
                    if (Theme.Any(t => t.GetType() == typeof(DarkTheme)))
                    {
                        var Target = Theme.Where(t => t.GetType() == typeof(DarkTheme)).FirstOrDefault();
                        Theme.Remove(Target);
                        Theme.Add(new LightTheme());
                        return;
                    }
                    break;
                default:
                    break;
            }
        });
        #endregion

        #region Method
        async void Author()
        {
            await RequestAsync<StorageWrite>();
            await RequestAsync<StorageRead>();
        }
        bool CheckVersion()
        {
            var ServerVersion = IHttpMultiClient.HttpMulti.AddNode(opt =>
            {
                opt.NodePath = "https://gitee.com/EmilyEdna/CandySugar/raw/master/CandySugarOption";
            }).Build().RunStringFirst();
            var AppVersion = ServerVersion.ToModel<JObject>()["App"].ToString();
            if (AppVersion.Equals(AppInfo.Current.VersionString))
            {
                "当前版本已是最新版本".OpenToast();
                return false;
            }
            else return true;
        }

        public async void AddLocalMusic(string fileName, string fullPath)
        {
            var Name = fileName.Split(".").FirstOrDefault();
            await Service.HAdd(new HRootEntity
            {
                Platfrom = "本地",
                Name = Name.Contains("-") ? Name.Split("-").LastOrDefault().Trim() : Name,
                ArtistName = Name.Contains("-") ? Name.Split("-").FirstOrDefault().Trim() : Name,
                Route = fullPath,
                SongId = string.Empty,
                AlbumId = string.Empty,
                AlbumName = Name
            });
        }
        #endregion
    }
}