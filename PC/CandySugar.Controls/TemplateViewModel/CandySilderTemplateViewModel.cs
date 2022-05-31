using CandySugar.Controls.ContentView;
using CandySugar.Controls.ContentViewModel;
using CandySugar.Controls.Template;
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
using System.Windows;
using XExten.Advance.LinqFramework;

namespace CandySugar.Controls.TemplateViewModel
{
    public class CandySilderTemplateViewModel : Screen
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

        public void ActivityAction(Dictionary<object, object> input)
        {
            var parentDataContext = Window.GetWindow((CandySilderTemplateView)input.Values.FirstOrDefault()).DataContext;
            var parentMethodInfo = parentDataContext.GetType().GetMethod("ScreenActivity");

            switch ((int)input.Keys.FirstOrDefault())
            {
                case 1:
                     StaticResource.CreateControl<NovelView>(parentMethodInfo, parentDataContext,Container.Get<NovelViewModel>());
                    break;
                case 2:
                    StaticResource.CreateControl<LovelView>(parentMethodInfo, parentDataContext, Container.Get<LovelViewModel>());
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    break;
                case 6:
                    break;
                case 7:
                    break;
                default:
                    break;
            }
        }
    }
}
