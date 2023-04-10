﻿using CandySugar.Com.Controls.UIExtenControls;
using CandySugar.Com.Library;
using CandySugar.Com.Library.DLLoader;
using CandySugar.Com.Library.Internet;
using CandySugar.Com.Library.Threads;
using CandySugar.Com.Library.Transfers;
using CandySugar.Com.Options.ComponentGeneric;
using CandySugar.EntryUI.Views;
using Stylet;
using StyletIoC;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace CandySugar.EntryUI.ViewModels
{
    public class IndexViewModel : Conductor<IScreen>
    {
        public IContainer Container;
        public IWindowManager WindowManager;
        public IndexViewModel(IContainer Container, IWindowManager WindowManager)
        {
            this.Container = Container;
            this.WindowManager = WindowManager;
            this.Title = $"甜糖V{Assembly.GetExecutingAssembly().GetName().Version}";
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            var dlls = AssemblyLoader.Dll.Select(item => new ComponentMenu
            {
                InstanceType = item.InstanceType,
                Name = item.Description,
                IsEnable = item.IsEnable,
                ViewModel = item.InstanceViewModel
            });
            MenuObj = new ObservableCollection<ComponentMenu>(dlls);
            ThreadManage.Instance.StartLong(() =>
            {
                if (!InternetWork.GetNetworkState)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        new ScreenNotifyView(CommonHelper.InternetErrorInformation).Show();
                    });
                    Thread.Sleep(10000);
                }
            }, "InternetWorkCheck", true);
        }

        #region Property
        private string _Title;
        public string Title
        {
            get => _Title;
            set => SetAndNotify(ref _Title, value);
        }

        private Control _CandyControl;
        public Control CandyControl
        {
            get => _CandyControl;
            set => SetAndNotify(ref _CandyControl, value);
        }

        private ObservableCollection<ComponentMenu> _MenuObj;
        /// <summary>
        /// 组件菜单
        /// </summary>
        public ObservableCollection<ComponentMenu> MenuObj
        {
            get => _MenuObj;
            set => SetAndNotify(ref _MenuObj, value);
        }
        #endregion

        #region Command
        /// <summary>
        /// 激活组件内容
        /// </summary>
        /// <param name="InstanceType"></param>
        public void ActiveCommand(Type InstanceType)
        {
            this.View.Dispatcher.Invoke(() =>
            {
                var Ctrl = (Control)Activator.CreateInstance(InstanceType);
                var ViewModel = MenuObj.FirstOrDefault(t => t.InstanceType == InstanceType).ViewModel;
                Ctrl.DataContext = Activator.CreateInstance(ViewModel);
                CandyControl = Ctrl;
                var MainView = (IndexView)View;
                //将主窗体的长宽变动通知给子控件
                GenericDelegate.InformationAction?.Invoke(MainView.Width, MainView.Height);
            });
        }
        /// <summary>
        /// 组件检索
        /// </summary>
        /// <param name="keyword"></param>
        public void SearchCommand(string keyword)
        {
            if (this.CandyControl != null)
            {
                GenericDelegate.SearchAction?.Invoke(keyword);
            }
        }
        #endregion
    }
}
