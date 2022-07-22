using Sdk.Component.Manga.sdk.ViewModel.Response;
using Sdk.Component.Manga.sdk;
using Sdk.Component.Manga.sdk.ViewModel;
using Sdk.Component.Manga.sdk.ViewModel.Enums;
using Sdk.Component.Manga.sdk.ViewModel.Request;
using CandySugar.Controls.Views.MangaViews;

namespace CandySugar.Controls.SysViewModels.HistoryViewModels
{
    public class MangaHistoryViewModel : BaseViewModel
    {
        ICandyService CandyService;
        public MangaHistoryViewModel()
        {
            this.Page = 1;
            CandyService = CandyContainer.Instance.Resolve<ICandyService>();
            Task.Run(() => Query());
        }

        #region 属性
        ObservableCollection<CandyManga> _Manga;
        public ObservableCollection<CandyManga> Manga
        {
            get => _Manga;
            set => SetProperty(ref _Manga, value);
        }
        #endregion

        #region 方法
        void Query()
        {
            var result = CandyService.GetManga(this.Page);
            Total = result.Total;
            if (Manga == null)
                Manga = new ObservableCollection<CandyManga>(result.Result);
            else
                result.Result.ForEach(item =>
                {
                    Manga.Add(item);
                });
        }
        async void InitContent(CandyManga input)
        {
            if (IsBusy) return;
            try
            {
                if (CandySoft.NetState.NetworkAccess != NetworkAccess.Internet)
                {
                    await Shell.Current.DisplayAlert("网络异常！", "请检查网络是否通畅！", "是");
                    return;
                }
                ShowBusy();
                await Task.Delay(CandySoft.Wait / 2);
                var result = await MangaFactory.Manga(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        CacheSpan = CandySoft.Cache,
                        Proxy = StaticResource.Proxy(),
                        ImplType = StaticResource.ImplType(),
                        MangaType = MangaEnum.Content,
                        Content = new MangaContent
                        {
                            Route = input.Route
                        }
                    };
                }).RunsAsync();
                await Task.Delay(CandySoft.Wait / 2);
                var bytes = await MangaFactory.Manga(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        CacheSpan = CandySoft.Cache,
                        Proxy = StaticResource.Proxy(),
                        ImplType = StaticResource.ImplType(),
                        MangaType = MangaEnum.Download,
                        Down = new MangaBytes
                        {
                            Route = result.ContentResult.Route,
                            CacheKey = result.ContentResult.CacheKey
                        }
                    };
                }).RunsAsync();
                CloseBusy();
                Navigation(bytes.DwonResult.Bytes);
            }
            catch (Exception ex)
            {
                StaticResource.PopToast(ex.Message);
            }
        }
        async void Navigation(List<byte[]> input)
        {
            await Shell.Current.GoToAsync(nameof(MangaWatchView), new Dictionary<string, object> { { "Result", input } });
        }
        void Remove(CandyManga input)
        {
            StaticResource.PopComfirm("确认删除", nameof(MangaHistoryViewModel));
            MessagingCenter.Subscribe<ComfirmViewModel, bool>(this, nameof(MangaHistoryViewModel), (sender, args) =>
            {
                if (args == true)
                {
                    CandyService.RemoveManga(input);
                    var temp = Manga.ToList();
                    temp.RemoveAll(t => t.CandyId == input.CandyId);
                    Manga = new ObservableCollection<CandyManga>(temp);
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
        public DelegateCommand<CandyManga> ViewAction => new(input =>
        {
            SetRefresh();
            InitContent(input);
        });
        public DelegateCommand<CandyManga> RemoveAction => new(input => Remove(input));
        #endregion
    }
}
