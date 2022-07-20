using CandySugar.Controls.Views.NovelViews;
using Sdk.Component.Novel.sdk;
using Sdk.Component.Novel.sdk.ViewModel;
using Sdk.Component.Novel.sdk.ViewModel.Enums;
using Sdk.Component.Novel.sdk.ViewModel.Request;
using Sdk.Component.Novel.sdk.ViewModel.Response;


namespace CandySugar.Controls.ViewModels.NovelViewModels
{
    public class NovelDetailViewModel : BaseViewModel
    {
        ICandyService Service;
        public NovelDetailViewModel()
        {
            Service = CandyContainer.Instance.Resolves<ICandyService>();
        }

        public override void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            this.Page = 1;
            Sort = true;
            Route = query["Route"].ToString();
            DetailResult = query["Key"] as NovelDetailRootResult;
            ElementResult = new ObservableCollection<NovelDetailElementResult>(DetailResult.ElementResults);
        }

        #region 属性
        string _Route;
        public string Route
        {
            get => _Route;
            set => SetProperty(ref _Route, value);
        }
        NovelDetailRootResult _DetailResult;
        public NovelDetailRootResult DetailResult
        {
            get => _DetailResult;
            set => SetProperty(ref _DetailResult, value);
        }

        ObservableCollection<NovelDetailElementResult> _ElementResult;
        public ObservableCollection<NovelDetailElementResult> ElementResult
        {
            get => _ElementResult;
            set => SetProperty(ref _ElementResult, value);
        }
        bool _Sort;
        public bool Sort
        {
            get => _Sort;
            set => SetProperty(ref _Sort, value);
        }
        #endregion

        #region 命令
        public DelegateCommand LoadMoreAction => new(() =>
        {
            if (Lock) return;
            if (Sort)
            {
                this.Page += 1;
                if (this.Page > DetailResult.Total) return;
                InitDetail();
            }
            else
            {
                Total -= 1;
                if (this.Total >= 1)
                    InitDetail();
            }
        });
        public DelegateCommand<string> SortTypeAction => new(input =>
        {
            this.Sort = bool.Parse(input);
            this.Total = DetailResult.Total;
            ElementResult = new ObservableCollection<NovelDetailElementResult>();
            InitDetail();

        });
        public DelegateCommand<NovelDetailElementResult> ViewAction => new(input =>
        {
            Navigation(input);
        });
        #endregion

        #region 方法
        async void InitDetail()
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
                var result = await NovelFactory.Novel(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        CacheSpan = CandySoft.Cache,
                        Proxy = StaticResource.Proxy(),
                        ImplType = StaticResource.ImplType(),
                        NovelType = NovelEnum.Detail,
                        Detail = new NovelDetail
                        {
                            Page = Sort ? this.Page : (int)this.Total,
                            DetailRoute = Route
                        }
                    };
                }).RunsAsync();
                CloseBusy();

                DetailResult = result.DetailResult;
                if (!Sort)
                    result.DetailResult.ElementResults.Reverse();
                result.DetailResult.ElementResults.ForEach(item =>
                {
                    ElementResult.Add(item);
                });
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("错误！", ex.Message, "是");
            }
        }
        async void Navigation(NovelDetailElementResult input)
        {
            Logic(input);
            await Shell.Current.GoToAsync(nameof(NovelContentView), new Dictionary<string, object> { { "Key", input }, { "Book", DetailResult } });
        }
        void Logic(NovelDetailElementResult input)
        {
            var Model = DetailResult.ToMapest<CandyNovel>();
            Model.Route = input.ChapterRoute;
            Model.Chapter = input.ChapterName;
            Service.AddOrAlterNovel(Model);
        }
        #endregion
    }
}
