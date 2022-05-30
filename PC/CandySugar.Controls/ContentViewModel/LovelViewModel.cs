using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Controls.ContentViewModel
{
    public class LovelViewModel : Screen
    {
        public IContainer Container;
        public IWindowManager WindowManager;
        public LovelViewModel(IContainer Container, IWindowManager WindowManager)
        {
            this.WindowManager = WindowManager;
            this.Container = Container;
        }
    }
}
