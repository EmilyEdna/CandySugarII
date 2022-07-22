using CandySugar.Controls.Views.ImageViews;

namespace CandySugar.Controls.SysViewModels.HistoryViewModels
{
    public class ImageHistoryViewModel: BaseViewModel
    {
        ICandyService CandyService;
        public ImageHistoryViewModel()
        {
            this.Page = 1;
            CandyService = CandyContainer.Instance.Resolve<ICandyService>();
            Task.Run(() => Query());
        }

        #region 属性
        ObservableCollection<CandyImage> _Root;
        public ObservableCollection<CandyImage> Root
        {
            get => _Root;
            set => SetProperty(ref _Root, value);
        }
        #endregion

        #region 方法
        void Query()
        {
            var result = CandyService.GetImage(this.Page);
            Total = result.Total;
            if (Root == null)
                Root = new ObservableCollection<CandyImage>(result.Result);
            else
                result.Result.ForEach(item =>
                {
                    Root.Add(item);
                });
        }
        async void Navigation(string input)
        {
            await Shell.Current.GoToAsync($"{nameof(ImageDetailView)}?Key={input}");
        }
        #endregion

        #region 命令
        public DelegateCommand LoadMoreAction => new(() =>
        {
            this.Page += 1;
            if (this.Page > Total) return;
            Query();
        });

        public DelegateCommand<CandyImage> ViewAction => new(input =>
        {
            Navigation(input.Original);
        });

        public DelegateCommand<CandyImage> DownAction => new(input =>
        {

        });

        public DelegateCommand<CandyImage> RemoveAction => new(input =>
        {
            CandyService.RemoveImage(input);
            var temp = Root.ToList();
            temp.RemoveAll(t => t.CandyId == input.CandyId);
            Root = new ObservableCollection<CandyImage>(temp);
        });
        #endregion
    }
}
