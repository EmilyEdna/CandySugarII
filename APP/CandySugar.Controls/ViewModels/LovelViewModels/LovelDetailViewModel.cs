using CandySugar.Controls.Views.LovelViews;
using Sdk.Component.Lovel.sdk;
using Sdk.Component.Lovel.sdk.ViewModel;
using Sdk.Component.Lovel.sdk.ViewModel.Enums;
using Sdk.Component.Lovel.sdk.ViewModel.Request;
using Sdk.Component.Lovel.sdk.ViewModel.Response;

namespace CandySugar.Controls.ViewModels.LovelViewModels
{
    public class LovelDetailViewModel : BaseViewModel
    {
        ICandyService CandyService;
        public LovelDetailViewModel()
        {
            CandyService = CandyContainer.Instance.Resolve<ICandyService>();
        }

        public override void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            EleResult = query["Route"] as LovelCategoryElementResult;
            ViewResult = new ObservableCollection<LovelViewResult>(query["Result"] as List<LovelViewResult>);
        }

        #region 属性
        LovelCategoryElementResult _EleResult;
        public LovelCategoryElementResult EleResult
        {
            get => _EleResult;
            set => SetProperty(ref _EleResult, value);
        }
        ObservableCollection<LovelViewResult> _ViewResult;
        public ObservableCollection<LovelViewResult> ViewResult
        {
            get => _ViewResult;
            set => SetProperty(ref _ViewResult, value);
        }
        #endregion

        #region 方法
        async void InitDown(LovelViewResult input)
        {
            var result = await LovelFactory.Lovel(opt =>
            {
                opt.RequestParam = new Input
                {
                    CacheSpan = CandySoft.Cache,
                    Proxy = StaticResource.Proxy(),
                    ImplType = StaticResource.ImplType(),
                    LovelType = LovelEnum.Download,
                    Down = new LovelDown
                    {
                        BookName = input.BookName,
                        UId = input.ChapterRoute.AsInt()
                    }
                };
            }).RunsAsync();
            var Directory = SyncStatic.CreateDir(Path.Combine(ICrossExtension.Instance.AndriodPath, "CandyDown", "Lovel"));
            var Files = SyncStatic.CreateFile(Path.Combine(Directory, $"{input.BookName}.txt"));
            var WriteResult = SyncStatic.WriteFile(result.DownResult.Bytes, Files);
            if (!WriteResult.IsNullOrEmpty())
                StaticResource.PopToast("下载完成!");
        }
        async void InitContent(LovelViewResult input)
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
                            ChapterRoute = input.ChapterRoute
                        }
                    };
                }).RunsAsync();
                CloseBusy();
                if (result.ContentResult.Content.Equals("因版权问题，文库不再提供该小说的阅读！"))
                {
                    await Shell.Current.DisplayAlert("提示！", "因版权问题，不再提供该小说的阅读", "是");
                    return;
                }
                Logic(input);
                Navigation(new Dictionary<string, object> { { "Result", result.ContentResult }, { "Title", input.ChapterName } });
            }
            catch (Exception ex)
            {
                StaticResource.PopToast(ex.Message);
            }
        }
        async void Navigation(Dictionary<string, object> Param)
        {
            await Shell.Current.GoToAsync(nameof(LovelContentView), Param);
        }
        void Logic(LovelViewResult input)
        {
            var Model = EleResult.ToMapest<CandyLovel>();
            Model.Route = input.ChapterRoute;
            Model.Chapter = input.ChapterName;
            Model.BookType = EleResult.Category;
            CandyService.AddOrAlterLovel(Model);
        }
        #endregion

        #region 命令
        public DelegateCommand<LovelViewResult> ViewAction => new(input =>
        {
            SetRefresh();
            if (input.IsDown)
            {
                Task.Run(() => StaticResource.PopToast("开始下载!"));
                InitDown(input);
            }
            else InitContent(input);
        });
        #endregion
    }
}
