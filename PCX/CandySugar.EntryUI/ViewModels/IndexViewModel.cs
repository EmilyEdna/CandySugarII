using CandySugar.Com.Controls.UIExtenControls;
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
            this.Title = "甜糖V1.0.0";
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            var dlls = AssemblyLoader.Dll.Select(item => new MenuObject
            {
                InstanceType = item.InstanceType,
                Name = item.Description,
                IsEnable = item.IsEnable,
                ViewModel = item.InstanceViewModel
            });
            MenuObj = new ObservableCollection<MenuObject>(dlls);
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

        private ObservableCollection<MenuObject> _MenuObj;
        public ObservableCollection<MenuObject> MenuObj
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
                GenericDelegate.InformationAction?.Invoke(MainView.Width,MainView.Height);
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
                this.CandyControl.DataContext.GetType()
                    .GetMethod(CommonHelper.DefaultMethod)?.Invoke(this.CandyControl.DataContext, new object[] { keyword });
            }
        }
        #endregion
    }
}
