using CandySugar.Library;
using Sdk.Component.Anime.sdk;
using Sdk.Component.Anime.sdk.ViewModel;
using Sdk.Component.Anime.sdk.ViewModel.Enums;
using Sdk.Component.Anime.sdk.ViewModel.Request;
using Sdk.Component.Anime.sdk.ViewModel.Response;
using System.Collections.ObjectModel;

namespace CandySugar.Controls
{
    public class BViewModel : ViewModelBase
    {
        readonly IService Service;
        public BViewModel(BaseServices baseServices) : base(baseServices)
        {
            Service = this.Container.Resolve<IService>();
        }
        /// <summary>
        /// 1 查询 2 分类
        /// </summary>
        public static int Module = 0;

        public override void OnLoad()
        {
            Task.Run(() => Init());
        }

        #region Property
        public string Key { get; set; }
        private AnimeLetterEnum Letter = AnimeLetterEnum.全部;
        private AnimeTypeEnum Types = AnimeTypeEnum.全部;
        private AnimeAreaEnum Areas = AnimeAreaEnum.全部;
        private string Years = string.Empty;
        #endregion

        #region Property
        AnimeInitResult _InitResult;
        public AnimeInitResult InitResult
        {
            get => _InitResult;
            set => SetProperty(ref _InitResult, value);
        }
        ObservableCollection<AnimeSearchElementResult> _SearchResult;
        public ObservableCollection<AnimeSearchElementResult> SearchResult
        {
            get => _SearchResult;
            set => SetProperty(ref _SearchResult, value);
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
                var result = await AnimeFactory.Anime(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        CacheSpan = DataBus.Cache,
                        ImplType = DataCenter.ImplType(),
                        AnimeType = AnimeEnum.Init
                    };
                }).RunsAsync();
                this.InitResult = result.InitResult;
                SetState();
            }
            catch (Exception ex)
            {
                await Service.AddLog("BInit异常", ex);
                "BInit异常".OpenToast();
            }
        }
        async void GroupInit(bool More)
        {
            try
            {
                Module = 2;
                SetActivity();
                await Task.Delay(DataBus.Delay);
                var result = await AnimeFactory.Anime(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        CacheSpan = DataBus.Cache,
                        ImplType = DataCenter.ImplType(),
                        AnimeType = AnimeEnum.Category,
                        Category = new AnimeCategory
                        {
                            LetterType = Letter,
                            Area = Areas,
                            Type = Types,
                            Year = Years,
                            Page = Page
                        }
                    };
                }).RunsAsync();
                Total = result.SeachResult.Total;
                if (More)
                    result.SeachResult.ElementResult.ForEach(SearchResult.Add);
                else
                    SearchResult = new ObservableCollection<AnimeSearchElementResult>(result.SeachResult.ElementResult);
                SetState();
            }
            catch (Exception ex)
            {
                await Service.AddLog("BGroupInit异常", ex);
                "BGroupInit异常".OpenToast();
            }
        }
        public async void QueryInit(bool More)
        {
            try
            {
                Module = 1;
                SetActivity();
                await Task.Delay(DataBus.Delay);
                var result = await AnimeFactory.Anime(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        CacheSpan = DataBus.Cache,
                        ImplType = DataCenter.ImplType(),
                        AnimeType = AnimeEnum.Search,
                        Search = new AnimeSearch
                        {
                            KeyWord = Key,
                            Page = this.Page
                        }
                    };
                }).RunsAsync();
                Total = result.SeachResult.Total;
                if (More)
                    result.SeachResult.ElementResult.ForEach(SearchResult.Add);
                else
                    SearchResult = new ObservableCollection<AnimeSearchElementResult>(result.SeachResult.ElementResult);
                SetState();
            }
            catch (Exception ex)
            {
                await Service.AddLog("BQueryInit异常", ex);
                "BQueryInit异常".OpenToast();
            }
        }
        #endregion

        #region Command
        public DelegateCommand<string> LetterCammand => new(input => {
            Letter = Enum.Parse<AnimeLetterEnum>(input);
            Task.Run(() => GroupInit(false));
        });
        public DelegateCommand<string> TypeCammand => new(input => {
            Types = Enum.Parse<AnimeTypeEnum>(input);
            Task.Run(() => GroupInit(false));
        });
        public DelegateCommand<string> YearCammand => new(input => {
            Years = input.Equals("全部") ? string.Empty : input;
            Task.Run(() => GroupInit(false));
        });
        public DelegateCommand<string> AreaCammand => new(input => {
            Areas = Enum.Parse<AnimeAreaEnum>(input);
            Task.Run(() => GroupInit(false));
        });
        public DelegateCommand<string> DetailCommand => new(input =>
        {
            Nav.NavigateAsync(new Uri("B1", UriKind.Relative), new NavigationParameters { { "Route", input } });
        });
        public DelegateCommand RefreshCommand => new(() =>
        {
            this.Page = 1;
            if (Module == 1) Task.Run(() => QueryInit(false));
            if (Module == 2) Task.Run(() => GroupInit(false));
        });
        public DelegateCommand MoreCommand => new(() =>
        {
            this.Page += 1;
            if (this.Page > this.Total) return;
            if (Module == 1) Task.Run(() => QueryInit(true));
            if (Module == 2) Task.Run(() => GroupInit(true));
        });
        #endregion
    }
}
