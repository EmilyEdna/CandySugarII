using CandySugar.Controls;
using CandySugar.Library;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Entry.ViewModels
{
    public class IndexViewModel : ViewModelBase
    {
        private BaseServices BaseServices;
        public IndexViewModel(BaseServices baseServices) : base(baseServices)
        {
            BaseServices = baseServices;
            Menu = new ObservableCollection<MenuModel>(MenuModel.GetMenus());
        }
        public override void OnAppearing()
        {
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
                case "太阳":
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
                case "运动":
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
            if (Content is B view)
            {
                var model = (view.BindingContext as BViewModel);
                model.Key = input;
                Task.Run(() => model.QueryInit(false));
            }

        });
        #endregion
    }
}
