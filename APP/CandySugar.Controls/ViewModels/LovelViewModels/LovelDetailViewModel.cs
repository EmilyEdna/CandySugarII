using CandySugar.Controls.Views.LovelViews;
using Sdk.Component.Lovel.sdk;
using Sdk.Component.Lovel.sdk.ViewModel;
using Sdk.Component.Lovel.sdk.ViewModel.Enums;
using Sdk.Component.Lovel.sdk.ViewModel.Request;
using Sdk.Component.Lovel.sdk.ViewModel.Response;
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace CandySugar.Controls.ViewModels.LovelViewModels
{
    public class LovelDetailViewModel : BaseViewModel
    {
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
        async void InitDown(string input)
        {
            //请求权限
            await RequestAsync<StorageWrite>();
        }
        async void InitContent(LovelViewResult input)
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
                Navigation(new Dictionary<string, object> { { "Result", result.ContentResult }, { "Title", input.ChapterName } });
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("错误！", ex.Message, "是");
            }
        }

        async void Navigation(Dictionary<string,object> Param)
        {
            await Shell.Current.GoToAsync(nameof(LovelContentView), Param);
        }
        #endregion

        #region 命令
        public DelegateCommand<LovelViewResult> ViewAction => new(input =>
        {
            if (input.IsDown) Task.Run(() => InitDown(input.ChapterRoute));
            else InitContent(input);
        });
        #endregion
    }
}
