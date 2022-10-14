using CandySugar.Controls.Views.ComicViews;
using CandySugar.Controls.Views.HnimeViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Controls.SysViewModels.HistoryViewModels
{
    public class ComicHistoryViewModel: BaseViewModel
    {
        ICandyService CandyService;
        public ComicHistoryViewModel()
        {
            this.Page = 1;
            CandyService = CandyContainer.Instance.Resolve<ICandyService>();
            Task.Run(() => Query());
        }

        #region 属性
        ObservableCollection<CandyComic> _Comic;
        public ObservableCollection<CandyComic> Comic
        {
            get => _Comic;
            set => SetProperty(ref _Comic, value);
        }
        #endregion

        #region 方法
        void Query()
        {
            var result = CandyService.GetComic(this.Page);
            Total = result.Total;
            if (Comic == null)
                Comic = new ObservableCollection<CandyComic>(result.Result);
            else
                result.Result.ForEach(item =>
                {
                    Comic.Add(item);
                });
        }
        void Remove(CandyComic input)
        {
            StaticResource.PopComfirm("确认删除", nameof(ComicHistoryViewModel));
            MessagingCenter.Subscribe<ComfirmViewModel, bool>(this, nameof(ComicHistoryViewModel), (sender, args) =>
            {
                if (args == true)
                {
                    CandyService.RemoveComic(input);
                    var temp = Comic.ToList();
                    temp.RemoveAll(t => t.CandyId == input.CandyId);
                    Comic = new ObservableCollection<CandyComic>(temp);
                }
            });
        }

        async void Navigation(List<string> input)
        {
            await Shell.Current.GoToAsync(nameof(ComicWatchView), new Dictionary<string, object> { { "Key", input } });
        }
        #endregion

        #region 命令
        public DelegateCommand LoadMoreAction => new(() =>
        {
            this.Page += 1;
            if (this.Page > Total) return;
            Query();
        });
        public DelegateCommand<CandyComic> ViewAction => new(input => Navigation(input.Route));
        public DelegateCommand<CandyComic> RemoveAction => new(input => Remove(input));
        #endregion
    }
}
