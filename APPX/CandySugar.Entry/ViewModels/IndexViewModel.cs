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

        public IndexViewModel(BaseServices baseServices) : base(baseServices)
        {
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
        #endregion
    }
}
