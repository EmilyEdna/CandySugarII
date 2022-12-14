using CandySugar.Library.Common;
using Sdk.Component.Lovel.sdk;
using Sdk.Component.Lovel.sdk.ViewModel;
using Sdk.Component.Lovel.sdk.ViewModel.Enums;
using Sdk.Component.Lovel.sdk.ViewModel.Request;
using Sdk.Component.Lovel.sdk.ViewModel.Response;
using System.Collections.ObjectModel;

namespace CandySugar.Controls;

internal class F1ViewModel : ViewModelBase
{
    readonly IService Service;
    public F1ViewModel(BaseServices baseServices) : base(baseServices)
    {
        Service = this.Container.Resolve<IService>();
    }

    public override void Initialize(INavigationParameters parameters)
    {
        Route = parameters.GetValue<string>("Route");
        Cover = parameters.GetValue<string>("Cover");
        Task.Run(DetailInit);
    }

    #region Property
    public string Route { get; set; }
    public string Cover { get; set; }
    #endregion

    #region Property
    ObservableCollection<LovelViewResult> _Result;
    public ObservableCollection<LovelViewResult> Result
    {
        get => _Result;
        set => SetProperty(ref _Result, value);
    }
    #endregion

    #region Method
    async void DetailInit()
    {
        try
        {
            Activity = true;
            await Task.Delay(DataBus.Delay);
            var detail = await LovelFactory.Lovel(opt =>
            {
                opt.RequestParam = new Input
                {
                    CacheSpan = DataBus.Cache,
                    ImplType = DataCenter.ImplType(),
                    LovelType = LovelEnum.Detail,
                    Detail = new LovelDetail
                    {
                        Route = Route
                    }
                };
            }).RunsAsync();
            var view = await LovelFactory.Lovel(opt =>
             {
                 opt.RequestParam = new Input
                 {
                     CacheSpan = DataBus.Cache,
                     ImplType = DataCenter.ImplType(),
                     LovelType = LovelEnum.View,
                     View = new LovelView
                     {
                         Route = detail.DetailResult.Route
                     }
                 };
             }).RunsAsync();
            Result = new ObservableCollection<LovelViewResult>(view.ViewResult);
            SetState();
        }
        catch (Exception ex)
        {
            await Service.AddLog("F1DetailInit异常", ex);
            "F1DetailInit异常".OpenToast();
        }
    }
    async void InitDown(LovelViewResult input)
    {
        var result = await LovelFactory.Lovel(opt =>
        {
            opt.RequestParam = new Input
            {
                CacheSpan = DataBus.Cache,
                ImplType = DataCenter.ImplType(),
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
            "下载完成".OpenToast();
    }
    async void Add()
    {
        await Service.FAdd(new FRootEntity
        {
            Cover = Cover,
            Name = Result.FirstOrDefault().BookName,
            Route=Route,
            Children = Result.Select(t => new FElementEntity
            {
                Name = t.ChapterName,
                IsDown = t.IsDown,
                Route = t.ChapterRoute
            }).ToList()
        });
    }
    #endregion

    #region Command
    public DelegateCommand<LovelViewResult> WatchCommand => new(input =>
    {
        if (input.IsDown) Task.Run(() => InitDown(input));
        else
        {
            Add();
            Nav.NavigateAsync(new Uri("F2", UriKind.Relative), new NavigationParameters { { "Title", input.ChapterName }, { "Route", input.ChapterRoute } });
        }
    });
    public DelegateCommand BackCommand => new(() =>
    {
        Nav.GoBackAsync();
    });
    #endregion
}
