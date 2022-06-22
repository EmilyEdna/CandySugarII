using CandySugar.Library;
using CandySugar.Logic.Entity.CandyEntity;
using CandySugar.Logic.IService;
using CandySugar.Resource.Properties;
using HandyControl.Data;
using Microsoft.Web.WebView2.Wpf;
using Polly.Caching;
using Sdk.Component.Hnime.sdk;
using Sdk.Component.Hnime.sdk.ViewModel;
using Sdk.Component.Hnime.sdk.ViewModel.Enums;
using Sdk.Component.Hnime.sdk.ViewModel.Request;
using Sdk.Component.Hnime.sdk.ViewModel.Response;
using Stylet;
using StyletIoC;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Controls.ContentViewModel
{
    public class HnimeViewModel : Screen
    {
        public IContainer Container;
        public IWindowManager WindowManager;
        public WebView2 WebView { get; set; }
        public ICandyHnime CandyHnime;
        public HnimeViewModel(IContainer Container, IWindowManager WindowManager)
        {
            this.WindowManager = WindowManager;
            this.Container = Container;
            this.CandyHnime = Container.Get<ICandyHnime>();
            this.Page = 1;
            this.StepOne = true;
            this.StepTwo = false;
            OnViewLoaded();
        }

        #region Field
        private HnimeSearch Query;
        private string CateRoute;
        #endregion

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
        private ObservableCollection<HnimeInitResult> _InitResult;
        public ObservableCollection<HnimeInitResult> InitResult
        {
            get => _InitResult;
            set => SetAndNotify(ref _InitResult, value);
        }
        private ObservableCollection<HnimeSearchElementResult> _QueryResult;
        public ObservableCollection<HnimeSearchElementResult> QueryResult
        {
            get => _QueryResult;
            set => SetAndNotify(ref _QueryResult, value);
        }
        private ObservableCollection<HnimePlayResult> _PlayResult;
        public ObservableCollection<HnimePlayResult> PlayResult
        {
            get => _PlayResult;
            set => SetAndNotify(ref _PlayResult, value);
        }
        #endregion

        #region Override
        protected override void OnViewLoaded()
        {
            InitHnime();
        }
        #endregion

        #region Action
        public void CategoryAction(string input)
        {
            CateRoute = input;
            this.Page = 1;
            Query = null;
            InitCategory(input);
        }
        public void SearchAction(HnimeSearch input)
        {
            Query = input;
            InitQuery(input);
        }
        public void LinkAction(HnimeSearchElementResult input)
        {
            InitWatch(input.Watch);
        }
        public void PlayAction(HnimePlayResult input)
        {
            if (!input.IsPlaying)
                InitWatch(input.WatchRoute);
            else
            {
                Logic(input);
                InitPlay(input.PlayRoute);
            }

        }
        public void PageAction(FunctionEventArgs<int> input)
        {
            if (Query != null)
            {
                Query.Page = input.Info;
                InitQuery(Query);
            }
            else
            {
                this.Page = input.Info;
                InitCategory(CateRoute);
            }
        }
        #endregion

        #region Method
        private async void InitHnime()
        {
            this.Loading = true;
            await Task.Delay(CandySoft.Default.WaitSpan);
            var AcgInitData = await HnimeFactory.Hnime(opt =>
            {
                opt.RequestParam = new Input
                {
                    HnimeType = HnimeEnum.Init,
                    CacheSpan = CandySoft.Default.Cache,
                    Proxy = StaticResource.Proxy(),
                    ImplType = StaticResource.ImplType(),
                    Init = new HnimeInit() { InitLabel = false }
                };
            }).RunsAsync();
            this.Loading = false;
            InitResult = new ObservableCollection<HnimeInitResult>(AcgInitData.InitResults);
        }
        private async void InitQuery(HnimeSearch input)
        {
            this.Loading = true;
            await Task.Delay(CandySoft.Default.WaitSpan);
            var AcgQueryData = await HnimeFactory.Hnime(opt =>
            {
                opt.RequestParam = new Input
                {
                    HnimeType = HnimeEnum.Search,
                    CacheSpan = CandySoft.Default.Cache,
                    Proxy = StaticResource.Proxy(),
                    ImplType = StaticResource.ImplType(),
                    Search = input
                };
            }).RunsAsync();
            this.Loading = false;
            Total = AcgQueryData.SearchResult.Total;
            QueryResult = new ObservableCollection<HnimeSearchElementResult>(AcgQueryData.SearchResult.ElementResult);
        }
        private async void InitCategory(string input)
        {
            this.Loading = true;
            await Task.Delay(CandySoft.Default.WaitSpan);
            var AcgCateData = await HnimeFactory.Hnime(opt =>
            {
                opt.RequestParam = new Input
                {
                    HnimeType = HnimeEnum.Category,
                    CacheSpan = CandySoft.Default.Cache,
                    Proxy = StaticResource.Proxy(),
                    ImplType = StaticResource.ImplType(),
                    Category = new HnimeCategory
                    {
                        Route = input,
                        Page = this.Page
                    }
                };
            }).RunsAsync();
            this.Loading = false;
            Total = AcgCateData.SearchResult.Total;
            QueryResult = new ObservableCollection<HnimeSearchElementResult>(AcgCateData.SearchResult.ElementResult);
        }
        private async void InitWatch(string input)
        {
            this.Loading = true;
            await Task.Delay(CandySoft.Default.WaitSpan);
            var AcgPlayData = await HnimeFactory.Hnime(opt =>
            {
                opt.RequestParam = new Input
                {
                    HnimeType = HnimeEnum.Watch,
                    CacheSpan = CandySoft.Default.Cache,
                    Proxy = StaticResource.Proxy(),
                    ImplType = StaticResource.ImplType(),
                    Play = new HnimePlay
                    {
                        Route = input
                    }
                };
            }).RunsAsync();
            this.Loading = false;
            PlayResult = new ObservableCollection<HnimePlayResult>(AcgPlayData.PlayResults);
        }
        private async void InitPlay(string input)
        {
            this.StepTwo = true;
            this.StepOne = false;
            await WebView.CoreWebView2.ExecuteScriptAsync($"Play('{input}','{CandySoft.Default.ScreenHeight - 30}')");
        }
        private async void Logic(HnimePlayResult input)
        {
            await this.CandyHnime.Add(new CandyHnime
            {
                Cover = input.Cover,
                Name = input.Title,
                Route = input.PlayRoute
            });
        }
        #endregion
    }
}
