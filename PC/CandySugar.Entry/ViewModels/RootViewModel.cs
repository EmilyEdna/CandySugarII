using CandySugar.Controls.Template;
using CandySugar.Controls.TemplateViewModel;
using CandySugar.Entry.CandyViewModels;
using CandySugar.Resource.Properties;
using Stylet;
using StyletIoC;
using System.Windows;

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


        public void SettingAction()
        {
            WindowManager.ShowWindow(Container.Get<OptionViewModel>());
        }
    }
}
