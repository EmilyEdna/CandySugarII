using CandySugar.Controls.Views.MangaViews;
using Sdk.Component.Manga.sdk;
using Sdk.Component.Manga.sdk.ViewModel;
using Sdk.Component.Manga.sdk.ViewModel.Enums;
using Sdk.Component.Manga.sdk.ViewModel.Request;
using Sdk.Component.Manga.sdk.ViewModel.Response;

namespace CandySugar.Controls.ViewModels
{
    public class MangaViewModel : BaseViewModel
    {

        public MangaViewModel()
        {
            Task.Run(() => InitManga());
        }

        #region 字段
        string CategoryRoute;
        bool LoadMore = false;
        #endregion

        #region 属性
        /// <summary>
        /// 首页数据
        /// </summary>
        ObservableCollection<MangaInitCategoryResult> _InitResult;
        public ObservableCollection<MangaInitCategoryResult> InitResult
        {
            get => _InitResult;
            set => SetProperty(ref _InitResult, value);
        }

        ObservableCollection<MangaCategoryElementResult> _CategoryResult;
        public ObservableCollection<MangaCategoryElementResult> CategoryResult
        {
            get => _CategoryResult;
            set => SetProperty(ref _CategoryResult, value);
        }
        #endregion

        #region 命令
        public DelegateCommand<string> CategoryAction => new(input =>
        {
            this.Page = 1;
            SetRefresh();
            LoadMore = false;
            this.CategoryRoute = input;
            this.KeyWord = string.Empty;
            Task.Run(() => InitCategory());
        });

        public DelegateCommand QueryAction => new(() =>
        {
            this.Page = 1;
            SetRefresh();
            LoadMore = false;
            this.CategoryRoute = string.Empty;
            Task.Run(() => InitQeury());
        });

        public DelegateCommand RefreshAction => new(() =>
        {
            this.Page = 1;
            SetRefresh(false);
            if (!CategoryRoute.IsNullOrEmpty()) Task.Run(() => InitCategory());
            else Task.Run(() => InitQeury());
        });

        public DelegateCommand LoadMoreAction => new(() =>
        {
            if (Lock) return;
            this.Page += 1;
            if (this.Page > Total) return;
            SetRefresh();
            LoadMore = true;
            if (KeyWord.IsNullOrEmpty()) InitCategory();
            else InitQeury();
        });

        public DelegateCommand<MangaCategoryElementResult> DetailAction => new(input =>
        {
            SetRefresh();
            InitDetail(input);
        });

        #endregion

        #region 方法
        async void InitManga()
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
                var result = await MangaFactory.Manga(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        CacheSpan = CandySoft.Cache,
                        Proxy = StaticResource.Proxy(),
                        ImplType = StaticResource.ImplType(),
                        MangaType = MangaEnum.Init,
                    };
                }).RunsAsync();
                CloseBusy();
                InitResult = new ObservableCollection<MangaInitCategoryResult>(result.CateInitResults);
            }
            catch (Exception ex)
            {
                StaticResource.PopToast(ex.Message);
            }
        }
        async void InitCategory()
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
                var result = await MangaFactory.Manga(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        CacheSpan = CandySoft.Cache,
                        Proxy = StaticResource.Proxy(),
                        ImplType = StaticResource.ImplType(),
                        MangaType = MangaEnum.Category,
                        Category = new MangaCategory
                        {
                            Page = Page,
                            Route = CategoryRoute
                        }
                    };
                }).RunsAsync();
                CloseBusy();
                Total = result.CategoryResult.Total;
                if (LoadMore)
                    result.CategoryResult.ElementResults.ForEach(item =>
                    {
                        CategoryResult.Add(item);
                    });
                else
                    CategoryResult = new ObservableCollection<MangaCategoryElementResult>(result.CategoryResult.ElementResults);
            }
            catch (Exception ex)
            {
                StaticResource.PopToast(ex.Message);
            }
        }
        async void InitQeury()
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
                var result = await MangaFactory.Manga(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        CacheSpan = CandySoft.Cache,
                        Proxy = StaticResource.Proxy(),
                        ImplType = StaticResource.ImplType(),
                        MangaType = MangaEnum.Search,
                        Search = new MangaSearch
                        {
                            Page = Page,
                            KeyWord = this.KeyWord
                        }
                    };
                }).RunsAsync();
                CloseBusy();
                Total = result.SearchResult.Total;
                if (LoadMore)
                    result.SearchResult.ElementResults.ToMapper<List<MangaCategoryElementResult>>().ForEach(item =>
                    {
                        CategoryResult.Add(item);
                    });
                else
                    CategoryResult = new ObservableCollection<MangaCategoryElementResult>(result.SearchResult.ElementResults.ToMapper<List<MangaCategoryElementResult>>());
            }
            catch (Exception ex)
            {
                StaticResource.PopToast(ex.Message);
            }
        }
        async void InitDetail(MangaCategoryElementResult input)
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
                var result = await MangaFactory.Manga(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        CacheSpan = CandySoft.Cache,
                        Proxy = StaticResource.Proxy(),
                        ImplType = StaticResource.ImplType(),
                        MangaType = MangaEnum.Detail,
                        Detail = new MangaDetail
                        {
                            Route = input.Route
                        }
                    };
                }).RunsAsync();
                CloseBusy();
                Navigation(result.ChapterResults, input.Name, input.Cover);
            }
            catch (Exception ex)
            {
                StaticResource.PopToast(ex.Message);
            }
        }
        async void Navigation(List<MangaChapterDetailResult> input, string name, string cover)
        {
            await Shell.Current.GoToAsync(nameof(MangaChapterView), new Dictionary<string, object> { { "Name", name }, { "Chapter", input }, { "Cover", cover } });
        }
        #endregion
    }
}
