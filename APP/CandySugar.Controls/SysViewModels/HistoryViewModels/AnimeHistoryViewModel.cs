using CandySugar.Controls.Views.AnimeViews;
using CandySugar.Controls.Views.LovelViews;
using Sdk.Component.Anime.sdk;
using Sdk.Component.Anime.sdk.ViewModel;
using Sdk.Component.Anime.sdk.ViewModel.Enums;
using Sdk.Component.Anime.sdk.ViewModel.Request;


namespace CandySugar.Controls.SysViewModels.HistoryViewModels
{
    public class AnimeHistoryViewModel : BaseViewModel
    {
        ICandyService CandyService;
        public AnimeHistoryViewModel()
        {
            this.Page = 1;
            CandyService = CandyContainer.Instance.Resolve<ICandyService>();
            Task.Run(() => Query());
        }

        #region 属性
        ObservableCollection<CandyAnimeRoot> _Root;
        public ObservableCollection<CandyAnimeRoot> Root
        {
            get => _Root;
            set => SetProperty(ref _Root, value);
        }
        #endregion

        #region 方法
        void Query()
        {
            var result = CandyService.GetAnime(this.Page);
            Total = result.Total;
            if (Root == null)
                Root = new ObservableCollection<CandyAnimeRoot>(result.Result);
            else
                result.Result.ForEach(item =>
                {
                    Root.Add(item);
                });
        }
        async void Navigation(string input)
        {
            await Shell.Current.GoToAsync($"{nameof(AnimePlayView)}?Key={input}");
        }
        void Remove(CandyAnimeRoot input)
        {
            StaticResource.PopComfirm("确认删除", nameof(AnimeHistoryViewModel));
            MessagingCenter.Subscribe<ComfirmViewModel, bool>(this, nameof(AnimeHistoryViewModel), (sender, args) =>
            {
                if (args == true)
                {
                    CandyService.RemoveAnime(input);
                    var temp = Root.ToList();
                    temp.RemoveAll(t => t.CandyId == input.CandyId);
                    Root = new ObservableCollection<CandyAnimeRoot>(temp);
                }
            });
        }
        async void InitPlay(CandyAnimeElement input)
        {
            try
            {
                if (CandySoft.NetState.NetworkAccess != NetworkAccess.Internet)
                {
                    StaticResource.PopToast("请检查网络是否通畅!");
                    return;
                }
                await Task.Delay(CandySoft.Wait);
                var result = await AnimeFactory.Anime(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        CacheSpan = CandySoft.Cache,
                        Proxy = StaticResource.Proxy(),
                        ImplType = StaticResource.ImplType(),
                        AnimeType = AnimeEnum.Watch,
                        WatchPlay = new AnimeWatch
                        {
                            Route = input.Route,
                            CollectName = input.Name,
                        }
                    };
                }).RunsAsync();
                Logic(input, result.PlayResult.PlayURL);
                Navigation(result.PlayResult.PlayURL);
            }
            catch (Exception ex)
            {
                StaticResource.PopToast(ex.Message);
            }
        }
        void Logic(CandyAnimeElement input, string PlayRoute)
        {
            var Model = Root.FirstOrDefault(t => t.Name == input.AnimeName);
            Model.CollectName = input.Name;
            Model.Route = PlayRoute;
            CandyService.AddOrAlterAnime(Model);
        }
        #endregion

        #region 命令
        public DelegateCommand LoadMoreAction => new(() =>
        {
            this.Page += 1;
            if (this.Page > Total) return;
            Query();
        });

        public DelegateCommand<CandyAnimeRoot> ViewAction => new(input =>
        {
            Navigation(input.Route);
        });

        public DelegateCommand<CandyAnimeElement> ContinueAtion => new(input =>
        {
            InitPlay(input);
        });

        public DelegateCommand<CandyAnimeRoot> RemoveAction => new(input => Remove(input));
        #endregion
    }
}
