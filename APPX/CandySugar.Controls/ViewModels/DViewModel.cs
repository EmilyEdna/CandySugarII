using Sdk.Component.Manga.sdk;
using Sdk.Component.Manga.sdk.ViewModel;
using Sdk.Component.Manga.sdk.ViewModel.Enums;
using Sdk.Component.Manga.sdk.ViewModel.Request;
using Sdk.Component.Manga.sdk.ViewModel.Response;
using System.Collections.ObjectModel;
using XExten.Advance.LinqFramework;

namespace CandySugar.Controls
{
    public class DViewModel : ViewModelBase
    {
        public DViewModel(BaseServices baseServices) : base(baseServices)
        {
        }

        public override void OnLoad()
        {
            Task.Run(Init);
        }

        /// <summary>
        /// 1 分类 2 查询
        /// </summary>
        public static int Module = 0;

        #region Property
        public string Key { get; set; }
        public string Group { get; set; }
        #endregion

        #region Property
        ObservableCollection<MangaInitCategoryResult> _InitResult;
        public ObservableCollection<MangaInitCategoryResult> InitResult
        {
            get => _InitResult;
            set => SetProperty(ref _InitResult, value);
        }
        ObservableCollection<MangaCategoryElementResult> _GroupResult;
        public ObservableCollection<MangaCategoryElementResult> GroupResult
        {
            get => _GroupResult;
            set => SetProperty(ref _GroupResult, value);
        }
        #endregion

        #region Method
        async void Init()
        {
            Module = 0;
            Activity = true;
            await Task.Delay(100);
            var result = await MangaFactory.Manga(opt =>
            {
                opt.RequestParam = new Input
                {
                    CacheSpan = DataBus.Cache,
                    ImplType = DataCenter.ImplType(),
                    MangaType = MangaEnum.Init,
                };
            }).RunsAsync();
            InitResult = new ObservableCollection<MangaInitCategoryResult>(result.CateInitResults);
            SetState();
        }
        async void GroupInit(bool More)
        {
            Module = 1;
            if (!More) Activity = true;
            await Task.Delay(100);
            var result = await MangaFactory.Manga(opt =>
            {
                opt.RequestParam = new Input
                {
                    CacheSpan = DataBus.Cache,
                    ImplType = DataCenter.ImplType(),
                    MangaType = MangaEnum.Category,
                    Category = new MangaCategory
                    {
                        Page = Page,
                        Route = Group
                    }
                };
            }).RunsAsync();
            Total = result.CategoryResult.Total;
            if (More)
                result.CategoryResult.ElementResults.ForEach(GroupResult.Add);
            else
                GroupResult = new ObservableCollection<MangaCategoryElementResult>(result.CategoryResult.ElementResults);
            SetState();
        }
        public async void QueryInit(bool More)
        {
            Module = 2;
            if (!More) Activity = true;
            await Task.Delay(100);
            var result = await MangaFactory.Manga(opt =>
            {
                opt.RequestParam = new Input
                {
                    CacheSpan = DataBus.Cache,
                    ImplType = DataCenter.ImplType(),
                    MangaType = MangaEnum.Search,
                    Search = new MangaSearch
                    {
                        Page = Page,
                        KeyWord = Key
                    }
                };
            }).RunsAsync();
            Total = result.SearchResult.Total;
            if (More)
                result.SearchResult.ElementResults.ToMapper<List<MangaCategoryElementResult>>().ForEach(GroupResult.Add);
            else
                GroupResult = new ObservableCollection<MangaCategoryElementResult>(result.SearchResult.ElementResults.ToMapper<List<MangaCategoryElementResult>>());
            SetState();
        }
        #endregion

        #region Command
        public DelegateCommand<string> GroupCammand => new(input =>
        {
            this.Group = input;
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
            Nav.NavigateAsync(new Uri("D1", UriKind.Relative), new NavigationParameters { { "Route", input } });
        });
        #endregion
    }
}
