using CandySugar.Com.Library;
using CandySugar.Com.Library.DLLoader;
using CandySugar.Com.Library.Transfers;
using Stylet;
using StyletIoC;
using System;
using System.Collections.ObjectModel;
using System.Linq;
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
        public void ActiveCommand(Type InstanceType)
        {
            this.View.Dispatcher.Invoke(() =>
            {
                var Ctrl = (Control)Activator.CreateInstance(InstanceType);
                var ViewModel = MenuObj.FirstOrDefault(t => InstanceType == InstanceType).ViewModel;
                Ctrl.DataContext = Activator.CreateInstance(ViewModel);
                CandyControl = Ctrl;
            });
        }

        public void SearchCommand(string keyword)
        {
            if (this.CandyControl != null)
            {
                this.CandyControl.DataContext.GetType()
                    .GetMethod(CommonHelper.DefaultMethod)
                    .Invoke(this.CandyControl.DataContext, new object[] { keyword });
            }
        }
        #endregion
    }
}
