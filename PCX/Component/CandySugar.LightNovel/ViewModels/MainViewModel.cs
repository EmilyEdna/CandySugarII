using CandySugar.LightNovel.View;
using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CandySugar.LightNovel.ViewModels
{
    public class MainViewModel : PropertyChangedBase
    {

        public MainViewModel()
        {
            ComponentControl = Module.IocModule.Resolve<IndexView>();
        }

        #region Property
        private Control _ComponentControl;
        public Control ComponentControl
        {
            get => _ComponentControl;
            set => SetAndNotify(ref _ComponentControl, value);
        }
        #endregion
    }
}
