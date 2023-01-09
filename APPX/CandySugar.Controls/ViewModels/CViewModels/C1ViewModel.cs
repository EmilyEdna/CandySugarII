namespace CandySugar.Controls
{
    public class C1ViewModel : ViewModelBase
    {
        readonly IService Service;

        public C1ViewModel(BaseServices baseServices) : base(baseServices)
        {
            Service = this.Container.Resolve<IService>();
        }

        public override void Initialize(INavigationParameters parameters)
        {
            Route = parameters.GetValue<string>("Route");
            Hidden();
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
        public void Show()
        {
#if ANDROID
            XExten.Advance.Maui.Direction.IDirection.Instance.LockOrientation(XExten.Advance.Maui.Direction.Platforms.Android.OrientationEnum.Portrait);
            XExten.Advance.Maui.Bar.IBarStatus.Instance.ShowStatusBar();
#endif
            Nav.GoBackAsync();
        }
        public void Hidden()
        {
#if ANDROID
            XExten.Advance.Maui.Direction.IDirection.Instance.LockOrientation(XExten.Advance.Maui.Direction.Platforms.Android.OrientationEnum.Landscape);
            XExten.Advance.Maui.Bar.IBarStatus.Instance.HiddenStatusBar();
#endif
        }
        #endregion

        #region Command
        public DelegateCommand BackCommand => new(() =>
        {
            Show();
        });
        #endregion
    }
}
