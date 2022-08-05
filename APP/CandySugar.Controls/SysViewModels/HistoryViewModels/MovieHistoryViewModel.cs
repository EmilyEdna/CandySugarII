using CandySugar.Controls.Views.MovieViews;

namespace CandySugar.Controls.SysViewModels.HistoryViewModels
{
    public class MovieHistoryViewModel : BaseViewModel
    {
        ICandyService CandyService;
        public MovieHistoryViewModel()
        {
            this.Page = 1;
            CandyService = CandyContainer.Instance.Resolve<ICandyService>();
            Task.Run(() => Query());
        }

        #region 属性
        ObservableCollection<CandyMovie> _Root;
        public ObservableCollection<CandyMovie> Root
        {
            get => _Root;
            set => SetProperty(ref _Root, value);
        }
        #endregion

        #region 方法
        void Query()
        {
            var result = CandyService.GetMovie(this.Page);
            Total = result.Total;
            if (Root == null)
                Root = new ObservableCollection<CandyMovie>(result.Result);
            else
                result.Result.ForEach(item =>
                {
                    Root.Add(item);
                });
        }
        async void Navigation(string input)
        {
            await Shell.Current.GoToAsync(nameof(MoviePlayView), new Dictionary<string, object> { { "Key", input } });
        }
        void Remove(CandyMovie input)
        {
            StaticResource.PopComfirm("确认删除", nameof(MovieHistoryViewModel));
            MessagingCenter.Subscribe<ComfirmViewModel, bool>(this, nameof(MovieHistoryViewModel), (sender, args) =>
            {
                if (args == true)
                {
                    CandyService.RemoveMovie(input);
                    var temp = Root.ToList();
                    temp.RemoveAll(t => t.CandyId == input.CandyId);
                    Root = new ObservableCollection<CandyMovie>(temp);
                }
            });
        }
        #endregion

        #region 命令
        public DelegateCommand LoadMoreAction => new(() =>
        {
            this.Page += 1;
            if (this.Page > Total) return;
            Query();
        });

        public DelegateCommand<CandyMovie> ViewAction => new(input =>
        {
            Navigation(input.Route);
        });

        public DelegateCommand<CandyMovie> RemoveAction => new(input => Remove(input));
        #endregion
    }
}
