using CandySugar.Controls.Views.AxgleViews;
using CandySugar.Controls.Views.LovelViews;
using Sdk.Component.Lovel.sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Controls.SysViewModels.HistoryViewModels
{
    public class AxgleHistoryViewModel : BaseViewModel
    {
        ICandyService CandyService;
        public AxgleHistoryViewModel()
        {
            this.Page = 1;
            CandyService = CandyContainer.Instance.Resolve<ICandyService>();
            Task.Run(() => Query());
        }

        #region 属性
        ObservableCollection<CandyAxgle> _Root;
        public ObservableCollection<CandyAxgle> Root
        {
            get => _Root;
            set => SetProperty(ref _Root, value);
        }
        #endregion

        #region 方法
        void Query()
        {
            var result = CandyService.GetAxgle(this.Page);
            Total = result.Total;
            if (Root == null)
                Root = new ObservableCollection<CandyAxgle>(result.Result);
            else
                result.Result.ForEach(item =>
                {
                    Root.Add(item);
                });
        }
        async void Navigation(string input)
        {
            await Shell.Current.GoToAsync($"{nameof(AxglePlayView)}?Key={input}");
        }
        void Remove(CandyAxgle input)
        {
            StaticResource.PopComfirm("确认删除", nameof(AxgleHistoryViewModel));
            MessagingCenter.Subscribe<ComfirmViewModel, bool>(this, nameof(AxgleHistoryViewModel), (sender, args) =>
            {
                if (args == true)
                {
                    CandyService.RemoveAxgle(input);
                    var temp = Root.ToList();
                    temp.RemoveAll(t => t.CandyId == input.CandyId);
                    Root = new ObservableCollection<CandyAxgle>(temp);
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
        public DelegateCommand<string> ViewAction => new(input =>
        {
            Navigation(input);
        });
        public DelegateCommand<CandyAxgle> RemoveAction => new(input => Remove(input));
        #endregion
    }
}
