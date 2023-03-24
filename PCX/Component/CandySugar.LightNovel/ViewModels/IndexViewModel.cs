﻿using CandySugar.Com.Controls.UIExtenControls;
using Sdk.Component.Lovel.sdk;
using Sdk.Core;
using Serilog;
using Stylet;
using System;
using Sdk.Component.Lovel.sdk.ViewModel;
using Sdk.Component.Lovel.sdk.ViewModel.Enums;
using Sdk.Component.Lovel.sdk.ViewModel.Request;
using Sdk.Component.Lovel.sdk.ViewModel.Response;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Sdk.Component.Plugins;
using CandySugar.Com.Options.ComponentObject;
using System.Collections.ObjectModel;
using CandySugar.Com.Library;

namespace CandySugar.LightNovel.ViewModels
{
    public class IndexViewModel : PropertyChangedBase
    {
        public IndexViewModel()
        {
            SdkLicense.Register(new SdkLicenseModel
            {
                Account = "EmilyEdna",
                Password = DateTime.Now.ToString("yyyyMMdd")
            });
            OnInit();
        }

        #region Property
        /// <summary>
        /// 分类菜单
        /// </summary>
        private ObservableCollection<LovelInitResult> _MenuIndex;
        public ObservableCollection<LovelInitResult> MenuIndex
        {
            get => _MenuIndex;
            set => SetAndNotify(ref _MenuIndex, value);
        }
        private ObservableCollection<LovelCategoryElementResult> _InformationElement;
        /// <summary>
        /// 分类结果
        /// </summary>
        public ObservableCollection<LovelCategoryElementResult> InformationElement
        {
            get => _InformationElement;
            set => SetAndNotify(ref _InformationElement, value);
        }
        #endregion

        #region Command
        public void ActiveCommand(string route)
        {
            OnInitInformation(route);
        }
        #endregion

        #region Method
        /// <summary>
        /// 初始化
        /// </summary>
        private void OnInit()
        {
            Task.Run(async () =>
            {
                try
                {
                    var result = (await LovelFactory.Lovel(opt =>
                    {
                        opt.RequestParam = new Input
                        {
                            CacheSpan = ComponentBinding.OptionObjectModels.Cache,
                            ImplType = SdkImpl.Rest,
                            LovelType = LovelEnum.Init,
                            Login = new()
                        };
                    }).RunsAsync()).InitResults;
                    MenuIndex = new ObservableCollection<LovelInitResult>(result);
                }
                catch (Exception ex)
                {
                    Log.Logger.Error(ex, "");
                    new ScreenNotifyView(CommonHelper.ComponentErrorInformation).Show();
                }
            });
        }
        /// <summary>
        /// 初始化分类
        /// </summary>
        /// <param name="route"></param>
        private void OnInitInformation(string route)
        {
            Task.Run(async () =>
            {
                try
                {
                    var result = (await LovelFactory.Lovel(opt =>
                    {
                        opt.RequestParam = new Input
                        {
                            CacheSpan = ComponentBinding.OptionObjectModels.Cache,
                            ImplType = SdkImpl.Rest,
                            LovelType = LovelEnum.Category,
                            Category = new LovelCategory
                            {
                                Page = 1,
                                Route = route
                            }
                        };
                    }).RunsAsync()).CategoryResult;
                    InformationElement = new ObservableCollection<LovelCategoryElementResult>(result.ElementResults);
                }
                catch (Exception ex)
                {
                    Log.Logger.Error(ex, "");
                    new ScreenNotifyView(CommonHelper.ComponentErrorInformation).Show();
                }
            });
        }
        #endregion

        #region ExternalCalls
        /// <summary>
        /// 检索数据
        /// </summary>
        /// <param name="keyword"></param>
        public void SearchHandler(string keyword)
        {

        }
        #endregion
    }
}
