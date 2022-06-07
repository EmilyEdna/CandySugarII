using CandySugar.Resource.Properties;
using Sdk.Component.Manga.sdk.ViewModel;
using Sdk.Component.Manga.sdk;
using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CandySugar.Library;
using Sdk.Component.Manga.sdk.ViewModel.Enums;
using System.Collections.ObjectModel;
using Sdk.Component.Manga.sdk.ViewModel.Response;
using Sdk.Component.Manga.sdk.ViewModel.Request;
using HandyControl.Data;
using XExten.Advance.LinqFramework;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using HandyControl.Tools.Command;

namespace CandySugar.Controls.ContentViewModel
{
    public class MangaViewModel : Screen
    {
        public IContainer Container;
        public IWindowManager WindowManager;
        public MangaViewModel(IContainer Container, IWindowManager WindowManager)
        {
            this.WindowManager = WindowManager;
            this.Container = Container;
            this.StepOne = true;
            this.StepTwo = false;
            this.Page = 1;
            OnViewLoaded();
        }

        #region CommomProperty_Bool
        private bool _Loading;
        public bool Loading
        {
            get => _Loading;
            set => SetAndNotify(ref _Loading, value);
        }
        private bool _StepOne;
        public bool StepOne
        {
            get => _StepOne;
            set => SetAndNotify(ref _StepOne, value);
        }
        private bool _StepTwo;
        public bool StepTwo
        {
            get => _StepTwo;
            set => SetAndNotify(ref _StepTwo, value);
        }
        #endregion

        #region CommomProperty_Int
        private int _Page;
        public int Page
        {
            get => _Page;
            set => SetAndNotify(ref _Page, value);
        }
        private int _Total;
        public int Total
        {
            get => _Total;
            set => SetAndNotify(ref _Total, value);
        }
        #endregion

        #region Property
        private ObservableCollection<MangaInitCategoryResult> _CateResult;
        /// <summary>
        /// 初始化结果
        /// </summary>
        public ObservableCollection<MangaInitCategoryResult> CateResult
        {
            get => _CateResult;
            set => SetAndNotify(ref _CateResult, value);
        }
        private ObservableCollection<MangaCategoryElementResult> _CateElementResult;
        /// <summary>
        /// 分类结果
        /// </summary>
        public ObservableCollection<MangaCategoryElementResult> CateElementResult
        {
            get => _CateElementResult;
            set => SetAndNotify(ref _CateElementResult, value);
        }
        private ObservableCollection<MangaChapterDetailResult> _DetailResult;
        /// <summary>
        /// 详情结果
        /// </summary>
        public ObservableCollection<MangaChapterDetailResult> DetailResult
        {
            get => _DetailResult;
            set => SetAndNotify(ref _DetailResult, value);
        }
        private ObservableCollection<BitmapSource> _ByteResult;
        /// <summary>
        /// 文件流结果
        /// </summary>
        public ObservableCollection<BitmapSource> ByteResult
        {
            get => _ByteResult;
            set => SetAndNotify(ref _ByteResult, value);
        }
        #endregion

        #region Override
        protected override void OnViewLoaded()
        {
            InitManga();
        }
        #endregion

        #region Field
        private string CategoryRoute;
        private string KeyWord;
        #endregion

        #region Action
        public void CategoryAction(string input)
        {
            this.Page = 1;
            this.CategoryRoute = input;
            this.KeyWord = string.Empty;
            InitCategory(input);
        }
        public void DetailAction(MangaCategoryElementResult input)
        {
            if (input == null) return;
            InitDetail(input.Route);
        }

        public void PageCateAction(FunctionEventArgs<int> input)
        {
            Page = input.Info;
            if (this.KeyWord.IsNullOrEmpty() && !this.CategoryRoute.IsNullOrEmpty()) InitCategory(this.CategoryRoute);
            if (!this.KeyWord.IsNullOrEmpty() && this.CategoryRoute.IsNullOrEmpty()) InitSearch(this.KeyWord);
        }

        public void WatchAction(MangaChapterDetailResult input)
        {
            InitContent(input.Route);
        }

        public void SearchAction(string input)
        {
            this.KeyWord = input;
            this.CategoryRoute = string.Empty;
            InitSearch(input);
        }

        public ICommand HistoryAction => new RelayCommand((obj) =>
        {
            this.StepOne = true;
            this.StepTwo = false;
        });
        #endregion

        #region Method
        private async void InitManga()
        {
            Loading = true;
            await Task.Delay(CandySoft.Default.WaitSpan);
            var MangaInitData = await MangaFactory.Manga(opt =>
            {
                opt.RequestParam = new Input
                {
                    CacheSpan = CandySoft.Default.Cache,
                    Proxy = StaticResource.Proxy(),
                    ImplType = StaticResource.ImplType(),
                    MangaType = MangaEnum.Init,
                };
            }).RunsAsync();
            Loading = false;
            CateResult = new ObservableCollection<MangaInitCategoryResult>(MangaInitData.CateInitResults);
        }
        private async void InitSearch(string input)
        {
            Loading = true;
            await Task.Delay(CandySoft.Default.WaitSpan);
            var MangaQueryData = await MangaFactory.Manga(opt =>
            {
                opt.RequestParam = new Input
                {
                    CacheSpan = CandySoft.Default.Cache,
                    Proxy = StaticResource.Proxy(),
                    ImplType = StaticResource.ImplType(),
                    MangaType = MangaEnum.Search,
                    Search = new MangaSearch
                    {
                        Page = Page,
                        KeyWord = input
                    }
                };
            }).RunsAsync();
            Loading = false;
            Total = MangaQueryData.SearchResult.Total;
            var Target = MangaQueryData.SearchResult.ElementResults.ToMapper<List<MangaCategoryElementResult>>();
            CateElementResult = new ObservableCollection<MangaCategoryElementResult>(Target);
        }
        private async void InitCategory(string input)
        {
            Loading = true;
            await Task.Delay(CandySoft.Default.WaitSpan);
            var MangaCateData = await MangaFactory.Manga(opt =>
            {
                opt.RequestParam = new Input
                {
                    CacheSpan = CandySoft.Default.Cache,
                    Proxy = StaticResource.Proxy(),
                    ImplType = StaticResource.ImplType(),
                    MangaType = MangaEnum.Category,
                    Category = new MangaCategory
                    {
                        Page = Page,
                        Route = input
                    }
                };
            }).RunsAsync();
            Loading = false;
            Total = MangaCateData.CategoryResult.Total;
            CateElementResult = new ObservableCollection<MangaCategoryElementResult>(MangaCateData.CategoryResult.ElementResults);
        }
        private async void InitDetail(string input)
        {
            Loading = true;
            await Task.Delay(CandySoft.Default.WaitSpan);
            var MangaDetailData = await MangaFactory.Manga(opt =>
            {
                opt.RequestParam = new Input
                {
                    CacheSpan = CandySoft.Default.Cache,
                    Proxy = StaticResource.Proxy(),
                    ImplType = StaticResource.ImplType(),
                    MangaType = MangaEnum.Detail,
                    Detail = new MangaDetail
                    {
                        Route = input
                    }
                };
            }).RunsAsync();
            Loading = false;
            DetailResult = new ObservableCollection<MangaChapterDetailResult>(MangaDetailData.ChapterResults);
        }
        private async void InitContent(string input)
        {
            this.StepOne = false;
            this.StepTwo = true;
            Loading = true;
            await Task.Delay(CandySoft.Default.WaitSpan);
            var MangaCotnentlData = await MangaFactory.Manga(opt =>
            {
                opt.RequestParam = new Input
                {
                    CacheSpan = CandySoft.Default.Cache,
                    Proxy = StaticResource.Proxy(),
                    ImplType = StaticResource.ImplType(),
                    MangaType = MangaEnum.Content,
                    Content = new MangaContent
                    {
                        Route = input
                    }
                };
            }).RunsAsync();
            await Task.Delay(CandySoft.Default.WaitSpan);
            var MangaByteData = await MangaFactory.Manga(opt =>
            {
                opt.RequestParam = new Input
                {
                    CacheSpan = CandySoft.Default.Cache,
                    Proxy = StaticResource.Proxy(),
                    ImplType = StaticResource.ImplType(),
                    MangaType = MangaEnum.Download,
                    Down = new MangaBytes
                    {
                        Route = MangaCotnentlData.ContentResult.Route,
                        CacheKey = MangaCotnentlData.ContentResult.CacheKey
                    }
                };
            }).RunsAsync();
            Loading = false;
            var width = (int)(CandySoft.Default.ScreenWidth - 200);
            var height = (int)(CandySoft.Default.ScreenHeight - 30);
            ByteResult = new ObservableCollection<BitmapSource>(MangaByteData.DwonResult.Bytes.Select(t => StaticResource.ToImage(t, width, height)));
        }
        #endregion
    }
}
