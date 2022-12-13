using CandySugar.Library;
using CandySugar.Logic;

namespace CandySugar.Foundation
{
    public class X2ViewModel : ViewModelBase
    {
        readonly IService Service;
        public X2ViewModel(BaseServices baseServices) : base(baseServices)
        {
            Service = this.Container.Resolve<IService>();
        }
        #region Command
        public DelegateCommand BackCommand => new(() =>
        {
            Nav.GoBackAsync();
        });
        public DelegateCommand<string> WatchCommand => new(input => { 
        
        
        });
        #endregion
    }
}
