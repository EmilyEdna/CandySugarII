using CandySugar.Controls.Views.LovelViews;
using Sdk.Component.Lovel.sdk;
using Sdk.Component.Lovel.sdk.ViewModel;
using Sdk.Component.Lovel.sdk.ViewModel.Enums;
using Sdk.Component.Lovel.sdk.ViewModel.Request;

namespace CandySugar.Controls.SysViewModels.HistoryViewModels
{
    public class LovelHistoryViewModel : BaseViewModel
    {
        ICandyService CandyService;
        public LovelHistoryViewModel()
        {
            this.Page = 1;
            CandyService = CandyContainer.Instance.Resolve<ICandyService>();
            Task.Run(() => Query());
        }

        #region 属性
        ObservableCollection<CandyLovel> _Lovel;
        public ObservableCollection<CandyLovel> Lovel
        {
            get => _Lovel;
            set => SetProperty(ref _Lovel, value);
        }
        #endregion

        #region 方法
        void Query()
        {
            var result = CandyService.GetLovel(this.Page);
            Total = result.Total;
            if (Lovel == null)
                Lovel = new ObservableCollection<CandyLovel>(result.Result);
            else
                result.Result.ForEach(item =>
                {
                    Lovel.Add(item);
                });
        }
        async void Navigation(Dictionary<string, object> lovel)
        {
            await Shell.Current.GoToAsync(nameof(LovelContentView), lovel);
        }
        void Remove(CandyLovel input)
        {
            StaticResource.PopComfirm("确认删除", nameof(LovelHistoryViewModel));
            MessagingCenter.Subscribe<ComfirmViewModel, bool>(this, nameof(LovelHistoryViewModel), (sender, args) =>
            {
                if (args == true)
                {
                    CandyService.RemoveLovel(input);
                    var temp = Lovel.ToList();
                    temp.RemoveAll(t => t.CandyId == input.CandyId);
                    Lovel = new ObservableCollection<CandyLovel>(temp);
                }
            });
        }
        async void InitContent(CandyLovel input)
        {
            try
            {
                if (CandySoft.NetState.NetworkAccess != NetworkAccess.Internet)
                {
                    StaticResource.PopToast("请检查网络是否通畅!");
                    return;
                }
                await Task.Delay(CandySoft.Wait);
                var result = await LovelFactory.Lovel(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        CacheSpan = CandySoft.Cache,
                        Proxy = StaticResource.Proxy(),
                        ImplType = StaticResource.ImplType(),
                        LovelType = LovelEnum.Content,
                        Content = new LovelContent
                        {
                            ChapterRoute = input.Route
                        }
                    };
                }).RunsAsync();
                Navigation(new Dictionary<string, object> { { "Result", result.ContentResult }, { "Title", input.Chapter } });
            }
            catch (Exception ex)
            {
                StaticResource.PopToast(ex.Message);
            }
        }

        #endregion

        #region 命令
        public DelegateCommand LoadMoreAction => new(() =>
        {
            this.Page += 1;
            if (this.Page > Total) return;
            Query();
        });
        public DelegateCommand<CandyLovel> ViewAction => new(input =>
        {
            InitContent(input);
        });
        public DelegateCommand<CandyLovel> RemoveAction => new(input => Remove(input));
        #endregion
    }
}
