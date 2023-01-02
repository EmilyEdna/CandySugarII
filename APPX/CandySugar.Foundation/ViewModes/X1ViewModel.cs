using CandySugar.Library;
using CandySugar.Logic;
using DryIoc.ImTools;

namespace CandySugar.Foundation
{
    public class X1ViewModel : ViewModelBase
    {
        readonly IService Service;
        public X1ViewModel(BaseServices baseServices) : base(baseServices)
        {
            Service = this.Container.Resolve<IService>();
        }

        #region Command
        public DelegateCommand BackCommand => new(() =>
        {
            Nav.GoBackAsync();
        });
        #endregion

        #region 
        public async Task Navgation(string Path)
        {
            await Nav.NavigateAsync(new Uri("B2", UriKind.Relative), new NavigationParameters { { "Route", Path }, { "Inner", false } });
        }
        #endregion
    }
}
