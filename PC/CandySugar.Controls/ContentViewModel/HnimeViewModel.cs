using CandySugar.Logic.IService;
using Microsoft.Web.WebView2.Wpf;
using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Controls.ContentViewModel
{
    public class HnimeViewModel: Screen
    {
        public IContainer Container;
        public IWindowManager WindowManager;
        public WebView2 WebView { get; set; }
        public HnimeViewModel(IContainer Container, IWindowManager WindowManager)
        {
            this.WindowManager = WindowManager;
            this.Container = Container;
        }
    }
}
