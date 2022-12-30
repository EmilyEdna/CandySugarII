using CandySugar.Library;
using Sdk.Component.Lovel.sdk.ViewModel;
using Sdk.Component.Lovel.sdk;
using Sdk.Component.Lovel.sdk.ViewModel.Response;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sdk.Component.Lovel.sdk.ViewModel.Enums;
using Sdk.Component.Lovel.sdk.ViewModel.Request;
using static SQLite.SQLite3;
using Microsoft.Maui;

namespace CandySugar.Controls
{
    public class FViewModel : ViewModelBase
    {
        readonly IService Service;
        public FViewModel(BaseServices baseServices) : base(baseServices)
        {
            Service = this.Container.Resolve<IService>();
        }
        /// <summary>
        /// 1 分类 2 查询
        /// </summary>
        public static int Module = 0;
        public override void OnLoad()
        {
            Task.Run(Init);
        }

        #region Property
        public string Route { get; set; }
        public string Key { get; set; }
        #endregion

        #region Property
        ObservableCollection<LovelInitResult> _InitResult;
        public ObservableCollection<LovelInitResult> InitResult
        {
            get => _InitResult;
            set => SetProperty(ref _InitResult, value);
        }
        ObservableCollection<LovelCategoryElementResult> _GroupResult;

        public ObservableCollection<LovelCategoryElementResult> GroupResult
        {
            get => _GroupResult;
            set => SetProperty(ref _GroupResult, value);
        }
        ObservableCollection<LovelSearchElementResult> _QueryResult;
        public ObservableCollection<LovelSearchElementResult> QueryResult
        {
            get => _QueryResult;
            set => SetProperty(ref _QueryResult, value);
        }
        #endregion

        #region Method
        async void Init()
        {
            try
            {
                Module = 0;
                SetActivity();
                await Task.Delay(DataBus.Delay);
                var result = await LovelFactory.Lovel(opt =>
                  {
                      opt.RequestParam = new Input
                      {
                          CacheSpan = DataBus.Cache,
                          ImplType = DataCenter.ImplType(),
                          LovelType = LovelEnum.Init,
                          Login = new()
                      };
                  }).RunsAsync();
                InitResult = new ObservableCollection<LovelInitResult>(result.InitResults);
                SetState();
            }
            catch (Exception ex)
            {
                await Service.AddLog("FInit异常", ex);
                "FInit异常".OpenToast();
            }
        }
        async void GroupInit(bool More)
        {
            try
            {
                Module = 1;
                SetActivity();
                await Task.Delay(DataBus.Delay);
                var result = await LovelFactory.Lovel(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        CacheSpan = DataBus.Cache,
                        ImplType = DataCenter.ImplType(),
                        LovelType = LovelEnum.Category,
                        Category = new LovelCategory
                        {
                            Page = this.Page,
                            Route = Route
                        }
                    };
                }).RunsAsync();
                Total = result.CategoryResult.Total;
                if (More)
                    result.CategoryResult.ElementResults.ForEach(GroupResult.Add);
                else
                    GroupResult = new ObservableCollection<LovelCategoryElementResult>(result.CategoryResult.ElementResults);
                SetState();
            }
            catch (Exception ex)
            {
                await Service.AddLog("FGroupInit异常", ex);
                "FGroupInit异常".OpenToast();
            }
        }
        public async void QueryInit(bool More)
        {
            try
            {
                Module = 2;
                SetActivity();
                await Task.Delay(DataBus.Delay);
                var result = await LovelFactory.Lovel(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        CacheSpan = DataBus.Cache,
                        ImplType = DataCenter.ImplType(),
                        LovelType = LovelEnum.Search,
                        Search = new LovelSearch
                        {
                            KeyWord = Key,
                            SearchType = LovelSearchEnum.ArticleName,
                            Page = Page
                        }
                    };
                }).RunsAsync();
                Total = result.SearchResult.Total;
                if (More)
                    result.SearchResult.ElementResults.ForEach(QueryResult.Add);
                else
                    QueryResult = new ObservableCollection<LovelSearchElementResult>(result.SearchResult.ElementResults);
                GroupResult = new ObservableCollection<LovelCategoryElementResult>(QueryResult.ToMapest<List<LovelCategoryElementResult>>());
                SetState();
            }
            catch (Exception ex)
            {
                await Service.AddLog("FQeuryInit异常", ex);
                "FQeuryInit异常".OpenToast();
            }
        }
        #endregion

        #region Command
        public DelegateCommand<string> GroupCammand => new(input =>
        {
            this.Route = input;
            Task.Run(() => GroupInit(false));
        });
        public DelegateCommand RefreshCommand => new(() =>
        {
            this.Page = 1;
            if (Module == 1) Task.Run(() => GroupInit(false));
            if (Module == 2) Task.Run(() => QueryInit(false));
        });
        public DelegateCommand MoreCommand => new(() =>
        {
            this.Page += 1;
            if (this.Page > this.Total) return;
            if (Module == 1) Task.Run(() => GroupInit(true));
            if (Module == 2) Task.Run(() => QueryInit(true));
        });
        public DelegateCommand<LovelCategoryElementResult> DetailCommand => new(input =>
        {
            Nav.NavigateAsync(new Uri("F1", UriKind.Relative), new NavigationParameters { { "Route", input.DetailAddress }, { "Cover", input.Cover } });
        });
        #endregion
    }
}
