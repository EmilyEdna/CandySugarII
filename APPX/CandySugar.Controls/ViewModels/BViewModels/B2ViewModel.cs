using CandySugar.Library;
using CandySugar.Library.Common;


namespace CandySugar.Controls
{
    public class B2ViewModel : ViewModelBase
    {
        public B2ViewModel(BaseServices baseServices) : base(baseServices) { }

        public override void Initialize(INavigationParameters parameters)
        {
            Route = parameters.GetValue<string>("Route");
            Inner = parameters.GetValue<bool>("Inner");
            LoadMauiAsset();
            Hidden();
        }
        #region Property
        public bool Inner { get; set; }
        public string Route { get; set; }
        public string Content { get; set; }
        #endregion

        #region Method
        async void LoadMauiAsset()
        {
            using var stream = await FileSystem.OpenAppPackageFileAsync(Inner?"Dplayer.html":"MaoPlayer.html");
            using var reader = new StreamReader(stream);
            Content = reader.ReadToEnd();
        }
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
