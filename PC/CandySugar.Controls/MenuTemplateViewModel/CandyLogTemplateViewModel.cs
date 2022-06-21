using CandySugar.Logic.Entity.CandyEntity;
using CandySugar.Logic.IService;
using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Controls.MenuTemplateViewModel
{
    public class CandyLogTemplateViewModel : PropertyChangedBase
    {
        public IContainer Container;
        public IWindowManager WindowManager;
        public ICandyLog CandyLog;
        public CandyLogTemplateViewModel(IContainer Container, IWindowManager WindowManager)
        {
            this.WindowManager = WindowManager;
            this.Container = Container;
            this.CandyLog = Container.Get<ICandyLog>();
            InitLog();
        }

        private ObservableCollection<CandyLog> _LogResult;
        public ObservableCollection<CandyLog> LogResult
        {
            get => _LogResult;
            set => SetAndNotify(ref _LogResult, value);
        }

        public async void InitLog()
        {
            LogResult = new ObservableCollection<CandyLog>(await CandyLog.Get());
        }
    }
}
