using CandySugar.Library;
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
    public class CandyHeadTemplateViewModel : PropertyChangedBase
    {
        public IContainer Container;
        public IWindowManager WindowManager;
        public CandyHeadTemplateViewModel(IContainer Container, IWindowManager WindowManager)
        {
            this.WindowManager = WindowManager;
            this.Container = Container;
            Handle();
        }

        private ObservableCollection<string> _MenuVlaue;
        public ObservableCollection<string> MenuVlaue
        {
            get => _MenuVlaue;
            set => SetAndNotify(ref _MenuVlaue, value);
        }

        private void Handle()
        {
            this.MenuVlaue = new ObservableCollection<string>();
            StaticResource.InitMenu().ForEach(item =>
            {
                if (item.Show)
                    this.MenuVlaue.Add(item.Query);
            });
        }
    }
}
