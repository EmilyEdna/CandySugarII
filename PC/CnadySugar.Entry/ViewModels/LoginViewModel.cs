using CandySugar.Library;
using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CnadySugar.Entry.ViewModels
{
    public class LoginViewModel : PropertyChangedBase
    {
        public IContainer Container;
        public IWindowManager WindowManager;
        public LoginViewModel(IContainer Container, IWindowManager WindowManager)
        {
            this.WindowManager = WindowManager;
            this.Container = Container;
        }

        private string _Account;
        public string Account
        {
            get => _Account;
            set => SetAndNotify(ref _Account, value);
        }

        private string _PassWord;
        public string PassWord
        {
            get => _PassWord;
            set => SetAndNotify(ref _PassWord, value);
        }

        public void LoginAction()
        {
            WindowManager.ShowWindow(Container.Get<RootViewModel>());
            Application.Current.MainWindow.Close();
        }
    }
}
