using CandySugar.Library;
using CandySugar.Logic;
using DryIoc.ImTools;

namespace CandySugar.Foundation
{
    public class X2ViewModel : ViewModelBase
    {
        readonly IService Service;
        public X2ViewModel(BaseServices baseServices) : base(baseServices)
        {
            Service = this.Container.Resolve<IService>();
            Query();
        }

        #region Property
        OptEntity _Opt;
        public OptEntity Opt
        {
            get => _Opt;
            set => SetProperty(ref _Opt, value);
        }
        #endregion

        #region Command
        public DelegateCommand BackCommand => new(() =>
        {
            Nav.GoBackAsync();
        });
        public DelegateCommand SaveCommand => new(() =>
        {
            Edit();
        });
        #endregion

        #region Method
        async void Query()
        {
            Opt = await Service.OptFirst();
            if (Opt == null)
            {
                Opt = new OptEntity
                {
                    Cache = DataBus.Cache,
                    Delay = DataBus.Delay,
                    Module = DataBus.Module,
                    QueryModule = DataBus.QueryModule,
                };
            }
        }
        async void Edit()
        {
            await Service.OptAlter(Opt);
        }
        #endregion
    }
}
