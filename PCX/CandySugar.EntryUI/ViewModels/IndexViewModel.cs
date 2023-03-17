using CandySugar.Com.Library.DLLoader;
using Stylet;
using StyletIoC;
using System;
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
            var Component = AssemblyLoader.Dll.FirstOrDefault();
            CandyControl = (Control)Activator.CreateInstance(Component.InstanceType);
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
        #endregion
    }
}
