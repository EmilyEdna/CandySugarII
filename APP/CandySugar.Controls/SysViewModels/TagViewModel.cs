using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Controls.SysViewModels
{
    public class TagViewModel : BaseViewModel
    {
        ICandyService CandyService;
        public TagViewModel()
        {
            CandyService = CandyContainer.Instance.Resolves<ICandyService>();
            Query();
        }

        #region 属性
        ObservableCollection<CandyLabel> _Root;
        public ObservableCollection<CandyLabel> Root
        {
            get => _Root;
            set => SetProperty(ref _Root, value);
        }
        #endregion

        #region 方法
        void Query()
        {
            var result = CandyService.GetTag(this.Page);
            Total = result.Total;
            if (Root == null)
                Root = new ObservableCollection<CandyLabel>(result.Result);
            else
                result.Result.ForEach(item =>
                {
                    Root.Add(item);
                });
        }
        #endregion

        #region 命令
        public DelegateCommand<CandyLabel> RemoveAction => new(input =>
        {
            CandyService.RemoveTag(input);
            var temp = Root.ToList();
            temp.RemoveAll(t => t.CandyId == input.CandyId);
            Root = new ObservableCollection<CandyLabel>(temp);
        });
        public DelegateCommand LoadMoreAction => new(() =>
        {
            this.Page += 1;
            if (this.Page > Total) return;
            Query();
        });
        #endregion
    }
}
