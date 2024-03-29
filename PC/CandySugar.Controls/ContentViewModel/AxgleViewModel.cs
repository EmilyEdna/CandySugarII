﻿using CandySugar.Library;
using CandySugar.Logic.Entity.CandyEntity;
using CandySugar.Logic.IService;
using CandySugar.Resource.Properties;
using HandyControl.Data;
using Microsoft.Web.WebView2.Wpf;
using Sdk.Component.Axgle.sdk;
using Sdk.Component.Axgle.sdk.ViewModel;
using Sdk.Component.Axgle.sdk.ViewModel.Enums;
using Sdk.Component.Axgle.sdk.ViewModel.Request;
using Sdk.Component.Axgle.sdk.ViewModel.Response;
using Serilog;
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
    public class AxgleViewModel : Screen
    {
        public IContainer Container;
        public IWindowManager WindowManager;
        public ICandyAxgle CandyAxgle;
        public WebView2 WebView { get; set; }
        public AxgleViewModel(IContainer Container, IWindowManager WindowManager)
        {
            this.WindowManager = WindowManager;
            this.Container = Container;
            this.CandyAxgle = Container.Get<ICandyAxgle>();
            this.StepOne = true;
            this.StepTwo = false;
            this.Page = 1;
            OnViewLoaded();
        }

        #region 重写
        protected override void OnViewLoaded()
        {
            InitAxgle();
        }
        #endregion

        #region 字段
        private string Keyword;
        private int Category;
        #endregion

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
        private ObservableCollection<AxgleInitResult> _InitResult;
        public ObservableCollection<AxgleInitResult> InitResult
        {
            get => _InitResult;
            set => SetAndNotify(ref _InitResult, value);
        }
        private ObservableCollection<AxgleSearchElementResult> _QueryResult;
        public ObservableCollection<AxgleSearchElementResult> QueryResult
        {
            get => _QueryResult;
            set => SetAndNotify(ref _QueryResult, value);
        }
        private string _PlayRoute;
        public string PlayRoute
        {
            get => _PlayRoute;
            set => SetAndNotify(ref _PlayRoute, value);
        }
        #endregion

        #region 命令
        public void SearchAction(string input)
        {
            this.Keyword = input;
            InitQuery(input);
        }
        public void CategoryAction(string input)
        {
            this.Keyword = string.Empty;
            this.Category = input.AsInt();
            this.Page = 1;
            InitCategory(input.AsInt());
        }
        public void PageAction(FunctionEventArgs<int> input)
        {
            this.Page = input.Info;
            if (this.Keyword.IsNullOrEmpty()) InitCategory(this.Category);
            else InitQuery(this.Keyword);
        }
        public void SaveAction(AxgleSearchElementResult input)
        {
            Logic(input);
        }
        public void ViewAction(string input)
        {
            this.PlayRoute = input;
            this.StepOne = false;
            this.StepTwo = true;

        }
        public void ClearAdAction()
        {
            StaticResource.ClearAd(WebView);
        }
        public void ReloadAction()
        {
            WebView.CoreWebView2.Reload();
        }
        #endregion

        #region 方法
        private async void InitAxgle()
        {
            try
            {
                this.Loading = true;
                await Task.Delay(CandySoft.Default.WaitSpan);
                var AxgleInitData = await AxgleFactory.Axgle(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        CacheSpan = CandySoft.Default.Cache,
                        Proxy = StaticResource.Proxy(),
                        ImplType = StaticResource.ImplType(),
                        AxgleType = AxgleEnum.Init
                    };
                }).RunsAsync();
                this.Loading = false;
                InitResult = new ObservableCollection<AxgleInitResult>(AxgleInitData.InitResults);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "");
                HandyControl.Controls.Growl.Error("服务异常");
            }
           
        }
        private async void InitQuery(string input)
        {
            try
            {
                this.Loading = true;
                await Task.Delay(CandySoft.Default.WaitSpan);
                var AxgleQueryData = await AxgleFactory.Axgle(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        CacheSpan = CandySoft.Default.Cache,
                        Proxy = StaticResource.Proxy(),
                        ImplType = StaticResource.ImplType(),
                        AxgleType = AxgleEnum.Search,
                        Search = new AxgleSearch
                        {
                            KeyWord = input,
                            Page = Page
                        }
                    };
                }).RunsAsync();
                this.Loading = false;
                this.Total = AxgleQueryData.SearchResult.Total;
                this.QueryResult = new ObservableCollection<AxgleSearchElementResult>(AxgleQueryData.SearchResult.ElementResult);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "");
                HandyControl.Controls.Growl.Error("服务异常");
            }
        }
        private async void InitCategory(int input)
        {
            try
            {
                this.Loading = true;
                await Task.Delay(CandySoft.Default.WaitSpan);
                var AxgleCateData = await AxgleFactory.Axgle(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        CacheSpan = CandySoft.Default.Cache,
                        Proxy = StaticResource.Proxy(),
                        ImplType = StaticResource.ImplType(),
                        AxgleType = AxgleEnum.Category,
                        Category = new AxgleCategory
                        {
                            CId = input,
                            Desc = (AxgleDescEnum)CandySoft.Default.AxModule,
                            Page = Page
                        }
                    };
                }).RunsAsync();
                this.Loading = false;
                this.Total = AxgleCateData.CategoryResult.Total;
                var Target = AxgleCateData.CategoryResult.ElementResult.ToMapest<List<AxgleSearchElementResult>>();
                this.QueryResult = new ObservableCollection<AxgleSearchElementResult>(Target);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "");
                HandyControl.Controls.Growl.Error("服务异常");
            }
        }
        private async void Logic(AxgleSearchElementResult input)
        {
            await this.CandyAxgle.Add(input.ToMapest<CandyAxgle>());
        }
        #endregion
    }
}
