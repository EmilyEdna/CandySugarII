namespace CandySugar.Controls
{
    public class D1ViewModel : ViewModelBase
    {
        readonly IService Service;

        public D1ViewModel(BaseServices baseServices) : base(baseServices)
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

        #region Method

        #endregion

        #region Command
        public DelegateCommand BackCommand => new(() =>
        {

        });
        #endregion
    }
}
