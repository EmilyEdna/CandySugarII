using CandySugar.Library.Common;
using Sdk.Component.Lovel.sdk;
using Sdk.Component.Lovel.sdk.ViewModel;
using Sdk.Component.Lovel.sdk.ViewModel.Enums;
using Sdk.Component.Lovel.sdk.ViewModel.Request;
using Sdk.Component.Lovel.sdk.ViewModel.Response;
using System.Collections.ObjectModel;

namespace CandySugar.Controls;

internal class F2ViewModel : ViewModelBase
{
    readonly IService Service;
    public F2ViewModel(BaseServices baseServices) : base(baseServices)
    {
        Service = this.Container.Resolve<IService>();
    }

    public override void Initialize(INavigationParameters parameters)
    {
        Route = parameters.GetValue<string>("Route");
        Title = parameters.GetValue<string>("Title");
        Task.Run(InitContent);
    }
    #region Property
    public string Route { get; set; }
    #endregion

    #region Property
    ObservableCollection<string> _Views;
    public ObservableCollection<string> Views
    {
        get => _Views;
        set => SetProperty(ref _Views, value);
    }
    ObservableCollection<string> _Images;
    public ObservableCollection<string> Images
    {
        get => _Images;
        set => SetProperty(ref _Images, value);
    }
    bool _IsImage;
    public bool IsImage
    {
        get => _IsImage;
        set => SetProperty(ref _IsImage, value);
    }
    bool _IsContent;
    public bool IsContent
    {
        get => _IsContent;
        set => SetProperty(ref _IsContent, value);
    }
    string _Ttile;
    public string Title
    {
        get => _Ttile;
        set => SetProperty(ref _Ttile, value);
    }
    #endregion

    #region Method
    async void InitContent()
    {
        try
        {
            Activity = true;
            await Task.Delay(100);
            var result = await LovelFactory.Lovel(opt =>
            {
                opt.RequestParam = new Input
                {
                    CacheSpan = DataBus.Cache,
                    ImplType = DataCenter.ImplType(),
                    LovelType = LovelEnum.Content,
                    Content = new LovelContent
                    {
                        ChapterRoute = Route
                    }
                };
            }).RunsAsync();
            if (result.ContentResult.Content.Equals("因版权问题，文库不再提供该小说的阅读！"))
            {
                SetState();
                "因版权问题，不再提供该小说的阅读".OpenToast();
                await Task.Delay(1000);
                Back();
            }
            else
            {
                var Res = result.ContentResult;
                if (!Res.Content.IsNullOrEmpty())
                {
                    Views = new ObservableCollection<string>(Res.Content.Split("\r\n", StringSplitOptions.RemoveEmptyEntries).Select(t => $"\r\n\t\t\t\t{t}"));
                    IsImage = false;
                    IsContent = !IsImage;
                }
                else
                {
                    Images = new ObservableCollection<string>(Res.Image);
                    IsImage = true;
                    IsContent = !IsImage;
                }
                SetState();
            }
        }
        catch (Exception ex)
        {
            await Service.AddLog("F2InitContent异常", ex);
            "F2InitContent异常".OpenToast();
        }
    }
    async void Back()
    {
        Nav.GoBackAsync();
    }
    #endregion

    #region Command
    public DelegateCommand BackCommand => new(() =>
    {
        Back();
    });
    #endregion
}
