using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Controls
{
    public class I1ViewModel : ViewModelBase
    {
        readonly IService Service;
        public I1ViewModel(BaseServices baseServices) : base(baseServices)
        {
            Service = this.Container.Resolve<IService>();
        }

        public override void Initialize(INavigationParameters parameters)
        {
            Route = parameters.GetValue<string>("Route");
        }

        #region Property
        string _Route;
        public string Route
        {
            get => _Route;
            set => SetProperty(ref _Route, value);
        }
        #endregion

        #region Command
        public DelegateCommand BackCommand => new(() =>
        {
            Nav.GoBackAsync();
        });
        #endregion

    }
}
