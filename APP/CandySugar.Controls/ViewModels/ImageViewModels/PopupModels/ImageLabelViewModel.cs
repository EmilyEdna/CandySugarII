namespace CandySugar.Controls.ViewModels.ImageViewModels.PopupModels
{
    public class ImageLabelViewModel : BaseViewModel
    {
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


        });
        #endregion
    }
}
