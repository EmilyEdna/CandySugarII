namespace CandySugar.Controls.ViewModels.ImageViewModels.PopupModels
{
    public class ImageLabelViewModel : BaseViewModel
    {
        ICandyService CandyService;
        public ImageLabelViewModel()
        {
            CandyService = CandyContainer.Instance.Resolves<ICandyService>();
        }

        #region 属性
        string _Chinese;
        public string Chinese
        {
            get => _Chinese;
            set => SetProperty(ref _Chinese, value);
        }

        string _English;
        public string English
        {
            get => _English;
            set => SetProperty(ref _English, value);
        }
        #endregion

        #region 命令
        public DelegateCommand SaveAction => new(() =>
        {
            Logic();
        });
        #endregion

        #region 方法
        async void Logic()
        {
            CandyService.AddOrAlterTag(new CandyLabel
            {
                EnLabel = English,
                ZhLabel = Chinese
            });
            await MopupService.Instance.PopAllAsync();
        }
        #endregion
    }
}
