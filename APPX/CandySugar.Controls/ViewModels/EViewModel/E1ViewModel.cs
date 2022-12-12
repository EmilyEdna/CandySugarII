using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sdk.Component.Novel.sdk;
using Sdk.Component.Novel.sdk.ViewModel;
using Sdk.Component.Novel.sdk.ViewModel.Enums;
using Sdk.Component.Novel.sdk.ViewModel.Request;
using Sdk.Component.Novel.sdk.ViewModel.Response;

namespace CandySugar.Controls
{
    public class E1ViewModel : ViewModelBase
    {
        readonly IService Service;
        public E1ViewModel(BaseServices baseServices) : base(baseServices)
        {
            Service = this.Container.Resolve<IService>();
        }
        public override void Initialize(INavigationParameters parameters)
        {
            Route = parameters.GetValue<string>("Route");
            Task.Run(() => InitDetail(false));
        }

        #region Property
        public string Route { get; set; }
        public bool Sort { get; set; }
        #endregion

        #region Property
        NovelDetailRootResult _Result;
        public NovelDetailRootResult Result
        {
            get => _Result;
            set => SetProperty(ref _Result, value);
        }
        ObservableCollection<NovelDetailElementResult> _ElementResult;
        public ObservableCollection<NovelDetailElementResult> ElementResult
        {
            get => _ElementResult;
            set => SetProperty(ref _ElementResult, value);
        }
        #endregion

        #region Method
        async void InitDetail(bool More)
        {
            try
            {
                SetState(More);
                await Task.Delay(DataBus.Delay);
                var result = await NovelFactory.Novel(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        CacheSpan = DataBus.Cache,
                        ImplType = DataCenter.ImplType(),
                        NovelType = NovelEnum.Detail,
                        Detail = new NovelDetail
                        {
                            Page= Sort?this.Total:this.Page,
                            DetailRoute = this.Route,
                        }
                    };
                }).RunsAsync();
                Result = result.DetailResult;
                if (Sort)
                    result.DetailResult.ElementResults.Reverse();
                if (More)
                    result.DetailResult.ElementResults.ForEach(ElementResult.Add);
                else
                    ElementResult = new ObservableCollection<NovelDetailElementResult>(result.DetailResult.ElementResults);
                SetState();
            }
            catch (Exception ex)
            {
                await Service.AddLog("E1InitDetail异常", ex);
                "E1InitDetail异常".OpenToast();
            }
        }
        #endregion

        #region Command
        public DelegateCommand BackCommand => new(() =>
        {
            Nav.GoBackAsync();
        });
        public DelegateCommand ChangeCommand => new(() =>
        {
            if (Sort) Sort = false; else Sort = true;
            this.Total = Result.Total;
            Task.Run(() => InitDetail(false));
        });
        public DelegateCommand<NovelDetailElementResult> WatchCommand => new(input =>
        {

        });
        public DelegateCommand MoreCommand => new(() => {
            if (Sort)
            {
                this.Page += 1;
                if (this.Page > Result.Total) return;
                Task.Run(() => InitDetail(true));
            }
            else
            {
                Total -= 1;
                if (this.Total >= 1)
                    Task.Run(() => InitDetail(true));
            }
         
        });
        #endregion
    }
}
