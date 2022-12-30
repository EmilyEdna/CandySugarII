using CandySugar.Library;
using Sdk.Component.Novel.sdk.ViewModel;
using Sdk.Component.Novel.sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using Sdk.Component.Novel.sdk.ViewModel.Enums;
using Sdk.Component.Novel.sdk.ViewModel.Response;
using System.Collections.ObjectModel;
using Sdk.Component.Novel.sdk.ViewModel.Request;
using Sdk.Component.Lovel.sdk.ViewModel.Response;

namespace CandySugar.Controls
{
    public class EViewModel : ViewModelBase
    {
        readonly IService Service;
        public EViewModel(BaseServices baseServices) : base(baseServices)
        {
            Service = this.Container.Resolve<IService>();
        }
        public override void OnLoad()
        {
            Task.Run(Init);
        }

        #region Property
        public string Route { get; set; }
        public string Key { get; set; }
        #endregion

        #region  Property
        ObservableCollection<NovelInitCategoryResult> _InitResult;
        public ObservableCollection<NovelInitCategoryResult> InitResult
        {
            get => _InitResult;
            set => SetProperty(ref _InitResult, value);
        }
        ObservableCollection<NovelCategoryElementResult> _GroupResult;
        public ObservableCollection<NovelCategoryElementResult> GroupResult
        {
            get => _GroupResult;
            set => SetProperty(ref _GroupResult, value);
        }
        #endregion

        #region Method
        async void Init()
        {
            try
            {
                SetActivity();
                await Task.Delay(DataBus.Delay);
                var result = await NovelFactory.Novel(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        CacheSpan = DataBus.Cache,
                        ImplType = DataCenter.ImplType(),
                        NovelType = NovelEnum.Init
                    };
                }).RunsAsync();
                InitResult = new ObservableCollection<NovelInitCategoryResult>(result.CateInitResults);
                SetState();
            }
            catch (Exception ex)
            {
                await Service.AddLog("EInit异常", ex);
                "EInit异常".OpenToast();
            }
        }
        async void GroupInit(bool More)
        {
            try
            {
                SetActivity();
                await Task.Delay(DataBus.Delay);
                var result = await NovelFactory.Novel(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        CacheSpan = DataBus.Cache,
                        ImplType = DataCenter.ImplType(),
                        NovelType = NovelEnum.Category,
                        Category = new NovelCategory
                        {
                            Page = this.Page,
                            CategoryRoute = this.Route,
                        }
                    };
                }).RunsAsync();
                Total = result.CategoryResult.Total;
                if (More)
                    result.CategoryResult.ElementResults.ForEach(GroupResult.Add);
                else
                    GroupResult = new ObservableCollection<NovelCategoryElementResult>(result.CategoryResult.ElementResults);
                SetState();
            }
            catch (Exception ex)
            {
                await Service.AddLog("EGroupInit异常", ex);
                "EGroupInit异常".OpenToast();
            }
        }
        public async void QueryInit(bool More)
        {
            try
            {
                SetActivity();
                await Task.Delay(DataBus.Delay);
                var result = await NovelFactory.Novel(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        CacheSpan = DataBus.Cache,
                        ImplType = DataCenter.ImplType(),
                        NovelType = NovelEnum.Search,
                        Search = new NovelSearch
                        {
                            KeyWord = Key
                        }
                    };
                }).RunsAsync();
                GroupResult = new ObservableCollection<NovelCategoryElementResult>(result.SearchResults.ToMapest<List<NovelCategoryElementResult>>());
                SetState();
            }
            catch (Exception ex)
            {
                await Service.AddLog("EQueryInit异常", ex);
                "EQueryInit异常".OpenToast();
            }
        }
        #endregion

        #region Command
        public DelegateCommand<string> GroupCammand => new(input =>
        {
            this.Page = 1;
            this.Route = input;
            Task.Run(() => GroupInit(false));
        });
        public DelegateCommand RefreshCommand => new(() =>
        {
            this.Page = 1;
            Task.Run(() => GroupInit(false));
        });
        public DelegateCommand MoreCommand => new(() =>
        {
            this.Page += 1;
            if (this.Page > this.Total) return;
            Task.Run(() => GroupInit(true));
        });
        public DelegateCommand<string> DetailCommand => new(input =>
        {
            Nav.NavigateAsync(new Uri("E1", UriKind.Relative), new NavigationParameters { { "Route", input } });
        });
        #endregion
    }
}
