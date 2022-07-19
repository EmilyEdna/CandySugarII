using CandySugar.Controls.SysViews.HistoryViews;

namespace CandySugar.Controls.SysViewModels
{
    public class HistoryViewModel : BaseViewModel
    {

        public HistoryViewModel()
        {
            InitContent(1);
        }

        #region 属性
        View _Contents;
        public View Contents
        {
            get => _Contents;
            set => SetProperty(ref _Contents, value);
        }
        #endregion

        #region 命令
        public DelegateCommand<string> HandlerAction => new(input =>
        {
            InitContent(input.AsInt());
        });
        #endregion

        #region 方法
        void InitContent(int input)
        {
            switch (input)
            {
                case 1:
                    Contents = new NovelHistoryView();
                    break;
                case 2:
                    Contents = new LovelHistoryView();
                    break;
                default:
                    break;
            }
        }
        #endregion
    }
}
