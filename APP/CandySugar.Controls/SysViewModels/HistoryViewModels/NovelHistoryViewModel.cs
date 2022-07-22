using CandySugar.Controls.Views.NovelViews;
using Sdk.Component.Novel.sdk.ViewModel.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Controls.SysViewModels.HistoryViewModels
{
    public class NovelHistoryViewModel : BaseViewModel
    {
        ICandyService CandyService;
        public NovelHistoryViewModel()
        {
            this.Page = 1;
            CandyService = CandyContainer.Instance.Resolve<ICandyService>();
            Task.Run(() => Query());
        }

        #region 属性
        ObservableCollection<CandyNovel> _Novel;
        public ObservableCollection<CandyNovel> Novel
        {
            get => _Novel;
            set => SetProperty(ref _Novel, value);
        }
        #endregion

        #region 方法
        void Query()
        {
            var result = CandyService.GetNovel(this.Page);
            Total = result.Total;
            if (Novel == null)
                Novel = new ObservableCollection<CandyNovel>(result.Result);
            else
                result.Result.ForEach(item =>
                {
                    Novel.Add(item);
                });
        }
        async void Navigation(CandyNovel novel)
        {
            var input = new NovelDetailElementResult
            {
                ChapterName = novel.Chapter,
                ChapterRoute = novel.Route
            };
            await Shell.Current.GoToAsync(nameof(NovelContentView), new Dictionary<string, object> { { "Key", input } });
        }
        #endregion

        #region 命令
        public DelegateCommand LoadMoreAction => new(() =>
        {
            this.Page += 1;
            if (this.Page > Total) return;
            Query();
        });
        public DelegateCommand<CandyNovel> ViewAction => new(input =>
        {
            Navigation(input);
        });
        public DelegateCommand<CandyNovel> RemoveAction => new(input =>
        {
            CandyService.RemoveNovel(input);
            var temp = Novel.ToList();
            temp.RemoveAll(t => t.CandyId == input.CandyId);
            Novel = new ObservableCollection<CandyNovel>(temp);
        });
        #endregion
    }
}
