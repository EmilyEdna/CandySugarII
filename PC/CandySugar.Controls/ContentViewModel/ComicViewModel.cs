using CandySugar.Library;
using CandySugar.Resource.Properties;
using HandyControl.Data;
using Sdk.Component.Comic.sdk;
using Sdk.Component.Comic.sdk.ViewModel;
using Sdk.Component.Comic.sdk.ViewModel.Enums;
using Sdk.Component.Comic.sdk.ViewModel.Request;
using Sdk.Component.Comic.sdk.ViewModel.Response;
using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XExten.Advance.LinqFramework;

namespace CandySugar.Controls.ContentViewModel
{
    public class ComicViewModel : Screen
    {
        public IContainer Container;
        public IWindowManager WindowManager;
        public ComicViewModel(IContainer Container, IWindowManager WindowManager)
        {
            this.WindowManager = WindowManager;
            this.Container = Container;
            this.StepOne = true;
            this.StepTwo = false;
            this.StepThree = false;
            this.Page = 1;
        }

        #region 布尔
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
        private bool _StepThree;
        public bool StepThree
        {
            get => _StepThree;
            set => SetAndNotify(ref _StepThree, value);
        }
        #endregion

        #region 整型
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

        #region 属性
        private ObservableCollection<ComicSearchElementResult> _Element;
        public ObservableCollection<ComicSearchElementResult> Element
        {
            get => _Element;
            set => SetAndNotify(ref _Element, value);
        }
        private ComicViewResult _Views;
        public ComicViewResult Views
        {
            get => _Views;
            set => SetAndNotify(ref _Views, value);
        }
        private ComicSearchElementResult _Search;
        public ComicSearchElementResult Search
        {
            get => _Search;
            set => SetAndNotify(ref _Search, value);
        }
        private ObservableCollection<string> _Image;
        public ObservableCollection<string> Image
        {
            get => _Image;
            set => SetAndNotify(ref _Image, value);
        }
        #endregion

        #region 字段
        private string Keyword;
        private string CategoryRoute;
        #endregion

        #region 命令
        public void ViewAction(ComicSearchElementResult input)
        {
            this.Search = input;
            this.StepOne = false;
            this.StepTwo = true;
            this.StepThree = false;
            if (input != null)
                InitView(input.Route);
        }
        public void SearchAction(string input)
        {
            Keyword = input;
            InitSearch();
        }
        public void PageAction(FunctionEventArgs<int> input)
        {
            Page = input.Info;
            if (this.Keyword.IsNullOrEmpty() && !this.CategoryRoute.IsNullOrEmpty()) InitCategory();
            if (!this.Keyword.IsNullOrEmpty() && this.CategoryRoute.IsNullOrEmpty()) InitSearch();
        }
        public void CategoryAction(string input)
        {
            CategoryRoute = input;
            this.StepOne = true;
            this.StepTwo = false;
            this.StepThree = false;
            InitCategory();
        }
        public void BackAction()
        {
            this.StepOne = true;
            this.StepTwo = false;
            this.StepThree = false;
        }
        public void ShowDetailAction()
        {
            this.StepOne = false;
            this.StepTwo = false;
            this.StepThree = true;
            Image = new ObservableCollection<string>();
            Views.Image.ForEach(item =>
            {
                var Spit = item.LastIndexOf("/") + 1;
                var Len = item.Length - Spit;
                var Hix = item.Substring(Spit, Len).Split(".");
                var NewName = item.Substring(0, Spit) + Hix.FirstOrDefault().Replace("t", "") + "." + Hix.LastOrDefault();
                Image.Add(NewName);
            });
        }
        #endregion

        #region 方法
        private async void InitSearch()
        {
            Loading = true;
            await Task.Delay(CandySoft.Default.WaitSpan);
            var ComicQueryData = await ComicFactory.Comic(opt =>
            {
                opt.RequestParam = new Input
                {
                    CacheSpan = CandySoft.Default.Cache,
                    Proxy = StaticResource.Proxy(),
                    ImplType = StaticResource.ImplType(),
                    ComicType = ComicEnum.Search,
                    Search = new ComicSearch
                    {
                        Page = Page,
                        Keyword = Keyword
                    }
                };
            }).RunsAsync();
            Loading = false;
            this.Total = ComicQueryData.SearchResult.Total;
            this.Element = new ObservableCollection<ComicSearchElementResult>(ComicQueryData.SearchResult.ElementResults);
        }
        private async void InitView(string input)
        {
            Loading = true;
            await Task.Delay(CandySoft.Default.WaitSpan);
            var ComicViewData = await ComicFactory.Comic(opt =>
            {
                opt.RequestParam = new Input
                {
                    CacheSpan = CandySoft.Default.Cache,
                    Proxy = StaticResource.Proxy(),
                    ImplType = StaticResource.ImplType(),
                    ComicType = ComicEnum.View,
                    View = new ComicView
                    {
                        Route = input
                    }
                };
            }).RunsAsync();
            Loading = false;
            this.Views = ComicViewData.ViewResult;
        }
        private async void InitCategory()
        {
            Loading = true;
            await Task.Delay(CandySoft.Default.WaitSpan);
            var ComicCateData = await ComicFactory.Comic(opt =>
            {
                opt.RequestParam = new Input
                {
                    CacheSpan = CandySoft.Default.Cache,
                    Proxy = StaticResource.Proxy(),
                    ImplType = StaticResource.ImplType(),
                    ComicType = ComicEnum.Category,
                    Category = new ComicCategory
                    {
                        Page = Page,
                        Route = CategoryRoute
                    }
                };
            }).RunsAsync();
            Loading = false;
            this.Total = ComicCateData.CategoryResult.Total;
            var Target = ComicCateData.CategoryResult.ElementResults.ToMapest<List<ComicSearchElementResult>>();
            this.Element = new ObservableCollection<ComicSearchElementResult>(Target);
        }
        #endregion
    }
}
