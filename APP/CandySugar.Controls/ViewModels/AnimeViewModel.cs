using CandySugar.Controls.Views.AnimeViews;
using Sdk.Component.Anime.sdk;
using Sdk.Component.Anime.sdk.ViewModel;
using Sdk.Component.Anime.sdk.ViewModel.Enums;
using Sdk.Component.Anime.sdk.ViewModel.Request;
using Sdk.Component.Anime.sdk.ViewModel.Response;

namespace CandySugar.Controls.ViewModels
{
    public class AnimeViewModel : BaseViewModel
    {
        public AnimeViewModel()
        {
            this.Page = 1;
            this.Words = new ObservableCollection<string>("A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z".Split(","));
            Task.Run(() => InitAnime());
        }

        #region 字段
        bool IsCategoryType = true;
        bool LoadMore = false;
        string CategoryKeyWord = string.Empty;
        #endregion

        #region 属性
        /// <summary>
        /// 字母
        /// </summary>
        ObservableCollection<string> _Words;
        public ObservableCollection<string> Words
        {
            get => _Words;
            set => SetProperty(ref _Words, value);
        }
        /// <summary>
        /// 初始化结果
        /// </summary>
        ObservableCollection<AnimeWeekDayIndexResult> _InitResult;
        public ObservableCollection<AnimeWeekDayIndexResult> InitResult
        {
            get => _InitResult;
            set => SetProperty(ref _InitResult, value);
        }
        /// <summary>
        /// 检索分类结果
        /// </summary>
        ObservableCollection<AnimeSearchElementResult> _SearchResult;
        public ObservableCollection<AnimeSearchElementResult> SearchResult
        {
            get => _SearchResult;
            set => SetProperty(ref _SearchResult, value);
        }
        /// <summary>
        /// 详情结果
        /// </summary>
        ObservableCollection<AnimeDetailResult> _DetailResult;
        public ObservableCollection<AnimeDetailResult> DetailResult
        {
            get => _DetailResult;
            set => SetProperty(ref _DetailResult, value);
        }
        #endregion

        #region 命令
        public DelegateCommand QueryAction => new(() =>
        {
            this.Page = 1;
            this.CategoryKeyWord = string.Empty;
            LoadMore = false;
            Task.Run(() => InitSearch());
        });
        public DelegateCommand<string> CategoryTypeAction => new(input =>
        {
            SetRefresh();
            this.Page = 1;
            this.IsCategoryType = true;
            this.KeyWord = string.Empty;
            this.CategoryKeyWord = input;
            LoadMore = false;
            Task.Run(() => InitCatagory());
        });
        public DelegateCommand<string> CategoryLetterAction => new(input =>
        {
            SetRefresh();
            this.Page = 1;
            this.IsCategoryType = false;
            this.KeyWord = string.Empty;
            this.CategoryKeyWord = input;
            LoadMore = false;
            Task.Run(() => InitCatagory());
        });
        public DelegateCommand RefreshAction => new(() =>
        {
            this.Page = 1;
            SetRefresh(false);
            LoadMore = false;
            if (!KeyWord.IsNullOrEmpty()) InitSearch();
            else InitCatagory();
        });
        public DelegateCommand LoadMoreAction => new(() =>
        {
            SetRefresh();
            if (Lock) return;
            this.Page += 1;
            if (this.Page > Total) return;
            LoadMore = true;
            if (KeyWord.IsNullOrEmpty()) InitCatagory();
            else InitSearch();
        });
        public DelegateCommand<AnimeSearchElementResult> DetailAction => new(input =>
        {
            InitDetail(input.Route);
        });
        #endregion

        #region 方法
        async void InitAnime()
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
                var result = await AnimeFactory.Anime(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        CacheSpan = CandySoft.Cache,
                        Proxy = StaticResource.Proxy(),
                        ImplType = StaticResource.ImplType(),
                        AnimeType = AnimeEnum.Init
                    };
                }).RunsAsync();
                CloseBusy();
                InitResult = new ObservableCollection<AnimeWeekDayIndexResult>(result.RecResults);
            }
            catch (Exception ex)
            {
                StaticResource.PopToast(ex.Message);
            }
        }
        async void InitCatagory()
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
                var model = new AnimeCategory
                {
                    Route = CategoryKeyWord,
                    Page = this.Page
                };
                if (!IsCategoryType)
                    model.LetterType = Enum.Parse<AnimeLetterEnum>(CategoryKeyWord);
                var result = await AnimeFactory.Anime(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        CacheSpan = CandySoft.Cache,
                        Proxy = StaticResource.Proxy(),
                        ImplType = StaticResource.ImplType(),
                        AnimeType = IsCategoryType ? AnimeEnum.CategoryType : AnimeEnum.Category,
                        Category = model
                    };
                }).RunsAsync();
                CloseBusy();
                Total = result.SeachResult.Total;
                if (LoadMore)
                    result.SeachResult.ElementResult.ForEach(item =>
                    {
                        SearchResult.Add(item);
                    });
                else
                    SearchResult = new ObservableCollection<AnimeSearchElementResult>(result.SeachResult.ElementResult);
            }
            catch (Exception ex)
            {
                StaticResource.PopToast(ex.Message);
            }
        }
        async void InitSearch()
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
                var result = await AnimeFactory.Anime(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        CacheSpan = CandySoft.Cache,
                        Proxy = StaticResource.Proxy(),
                        ImplType = StaticResource.ImplType(),
                        AnimeType = AnimeEnum.Search,
                        Search = new AnimeSearch
                        {
                            KeyWord = KeyWord,
                            Page = this.Page
                        }
                    };
                }).RunsAsync();
                CloseBusy();
                Total = result.SeachResult.Total;
                if (LoadMore)
                    result.SeachResult.ElementResult.ForEach(item =>
                    {
                        SearchResult.Add(item);
                    });
                else
                    SearchResult = new ObservableCollection<AnimeSearchElementResult>(result.SeachResult.ElementResult);
            }
            catch (Exception ex)
            {
                StaticResource.PopToast(ex.Message);
            }
        }
        async void InitDetail(string input)
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
                var result = await AnimeFactory.Anime(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        CacheSpan = CandySoft.Cache,
                        Proxy = StaticResource.Proxy(),
                        ImplType = StaticResource.ImplType(),
                        AnimeType = AnimeEnum.Detail,
                        Detail = new AnimeDetail
                        {
                            Route = input
                        }
                    };
                }).RunsAsync();
                CloseBusy();
                DetailResult = new ObservableCollection<AnimeDetailResult>(result.DetailResults.Where(t => t.IsDownURL == false));
                Navigation(DetailResult.ToList());
            }
            catch (Exception ex)
            {
                StaticResource.PopToast(ex.Message);
            }
        }
        async void Navigation(List<AnimeDetailResult> input)
        {
            await Shell.Current.GoToAsync(nameof(AnimeDetailView), new Dictionary<string, object> { { "Route", input } });
        }
        #endregion
    }
}
