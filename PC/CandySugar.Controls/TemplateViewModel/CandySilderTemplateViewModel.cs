using CandySugar.Library;
using CandySugar.Library.TemplateModel;
using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Controls.TemplateViewModel
{
    public class CandySilderTemplateViewModel : PropertyChangedBase
    {
        public IContainer Container;
        public IWindowManager WindowManager;
        public CandySilderTemplateViewModel(IContainer Container, IWindowManager WindowManager)
        {
            this.WindowManager = WindowManager;
            this.Container = Container;
            this.SilderMenu = new ObservableCollection<SilderModel>(StaticResource.InitMenu());
        }

        private ObservableCollection<SilderModel> _SilderMenu;
        public ObservableCollection<SilderModel> SilderMenu
        {
            get => _SilderMenu;
            set => SetAndNotify(ref _SilderMenu, value);
        }
    }
}
