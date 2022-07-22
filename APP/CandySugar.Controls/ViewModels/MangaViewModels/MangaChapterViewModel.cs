using CandySugar.Controls.Views.MangaViews;
using Sdk.Component.Manga.sdk;
using Sdk.Component.Manga.sdk.ViewModel;
using Sdk.Component.Manga.sdk.ViewModel.Enums;
using Sdk.Component.Manga.sdk.ViewModel.Request;
using Sdk.Component.Manga.sdk.ViewModel.Response;

namespace CandySugar.Controls.ViewModels.MangaViewModels
{
    public class MangaChapterViewModel : BaseViewModel
    {
        ICandyService CandyService;
        public MangaChapterViewModel()
        {
            CandyService = CandyContainer.Instance.Resolve<ICandyService>();
        }
        public override void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            Title = query["Name"].ToString();
            Cover = query["Cover"].ToString();
            DetailResult = new ObservableCollection<MangaChapterDetailResult>(query["Chapter"] as List<MangaChapterDetailResult>);
        }
        #region 属性
        string _Title;
        public string Title
        {
            get => _Title;
            set => SetProperty(ref _Title, value);
        }
        string _Cover;
        public string Cover
        {
            get => _Cover;
            set => SetProperty(ref _Cover, value);
        }
        ObservableCollection<MangaChapterDetailResult> _DetailResult;
        public ObservableCollection<MangaChapterDetailResult> DetailResult
        {
            get => _DetailResult;
            set => SetProperty(ref _DetailResult, value);
        }
        #endregion

        #region 命令
        public DelegateCommand<MangaChapterDetailResult> ViewAction => new(input =>
        {
            SetRefresh();
            InitContent(input);
        });
        #endregion

        #region 方法
        async void InitContent(MangaChapterDetailResult input)
        {
            if (IsBusy) return;
            try
            {
                if (CandySoft.NetState.NetworkAccess != NetworkAccess.Internet)
                {
                    StaticResource.PopToast("请检查网络是否通畅!");
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
                Logic(input);
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
        void Logic(MangaChapterDetailResult input)
        {
            var Model = input.ToMapest<CandyManga>();
            Model.CollectName = input.Title;
            Model.Key = input.TagKey;
            Model.Name = Title;
            Model.Cover = Cover;
            CandyService.AddOrAlterManga(Model);
        }
        #endregion
    }
}
