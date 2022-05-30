using CandySugar.Controls.Template;
using CandySugar.Controls.TemplateViewModel;
using CandySugar.Entry.CandyViewModels;
using CandySugar.Library.Template;
using CandySugar.Resource.Properties;
using Stylet;
using StyletIoC;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using CandySugar.Library;
using CandySugar.Controls.ContentView;
using CandySugar.Controls.ContentViewModel;

namespace CandySugar.Entry.ViewModels
{
    public class RootViewModel : Conductor<IScreen>
    {
        public IContainer Container;
        public IWindowManager WindowManager;
        public RootViewModel(IContainer Container, IWindowManager WindowManager)
        {
            this.Container = Container;
            this.WindowManager = WindowManager;
            SilderView = new CandySilderTemplateView
            {
                DataContext = Container.Get<CandySilderTemplateViewModel>()
            };
            HeadViewModel = Container.Get<CandyHeadTemplateViewModel>();
        }

        public CandySilderTemplateView SilderView { get; set; }
        public CandyHeadTemplateViewModel HeadViewModel { get; set; }

        private CandyControl _Ctrl;
        public CandyControl Ctrl
        {
            get => _Ctrl;
            set => SetAndNotify(ref _Ctrl, value);
        }

        public void SettingAction()
        {
            WindowManager.ShowWindow(Container.Get<OptionViewModel>());
        }

        public void SearchAction(Dictionary<object, object> param)
        {

            if (param.Keys.FirstOrDefault() is string type)
            {
                switch (type)
                {
                    case "XS":
                        Ctrl = StaticResource.CreateControl<NovelView>(Container.Get<NovelViewModel>(),param.Values.FirstOrDefault().ToString());
                        break;
                    case "LXS":
                        Ctrl = StaticResource.CreateControl<LovelView>(Container.Get<LovelViewModel>(), param.Values.FirstOrDefault().ToString());
                        break;
                    default:
                        break;
                }
            }
        }

        public void ScreenActivity(CandyControl input)
        {
            Ctrl = input;
        }
    }
}
