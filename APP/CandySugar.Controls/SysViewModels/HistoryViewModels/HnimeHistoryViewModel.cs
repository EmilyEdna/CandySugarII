using CandySugar.Controls.Views.HnimeViews;
using Sdk.Component.Anime.sdk;

namespace CandySugar.Controls.SysViewModels.HistoryViewModels
{
    public class HnimeHistoryViewModel: BaseViewModel
    {
        ICandyService CandyService;
        public HnimeHistoryViewModel()
        {
            this.Page = 1;
            CandyService = CandyContainer.Instance.Resolve<ICandyService>();
            Query();
        }

        #region 属性
        ObservableCollection<CandyHnime> _Root;
        public ObservableCollection<CandyHnime> Root
        {
            get => _Root;
            set => SetProperty(ref _Root, value);
        }
        #endregion

        #region 方法
        void Query()
        {
            var result = CandyService.GetHnime(this.Page);
            Total = result.Total;
            if (Root == null)
                Root = new ObservableCollection<CandyHnime>(result.Result);
            else
                result.Result.ForEach(item =>
                {
                    Root.Add(item);
                });
        }
        async void Navigation(string input)
        {
            await Shell.Current.GoToAsync($"{nameof(HnimePlayView)}?Key={input}");
        }
        #endregion

        #region 命令
        public DelegateCommand LoadMoreAction => new(() =>
        {
            this.Page += 1;
            if (this.Page > Total) return;
            Query();
        });

        public DelegateCommand<CandyHnime> ViewAction => new(input =>
        {
            Navigation(input.Route);
        });

     

        public DelegateCommand<CandyHnime> RemoveAction => new(input =>
        {
            CandyService.RemoveHnime(input);
            var temp = Root.ToList();
            temp.RemoveAll(t => t.CandyId == input.CandyId);
            Root = new ObservableCollection<CandyHnime>(temp);
        });
        #endregion
    }
}
