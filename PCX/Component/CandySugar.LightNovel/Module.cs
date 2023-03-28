using CandySugar.LightNovel.View;
using CandySugar.LightNovel.ViewModels;
using DryIoc;
using System.Linq;
using System.Windows.Controls;

namespace CandySugar.LightNovel
{
    public class Module
    {
        public static Module IocModule { get; set; }
        private static IContainer Container { get; set; }
        public Module()
        {
            IocModule = this;
            Container = new Container();
            Container.Register(typeof(IndexView),Reuse.Transient);
            Container.Register(typeof(ReaderView), Reuse.Transient);

            Container.Register(typeof(IndexViewModel), Reuse.Transient);
            Container.Register(typeof(ReaderViewModel), Reuse.Transient);
        }
        public T Resolve<T>() where T : UserControl
        {
            var Ctrl = (UserControl)Container.Resolve(typeof(T));
            var VM = this.GetType().Assembly.GetTypes().FirstOrDefault(t => t.Name == $"{typeof(T).Name}Model");
            Ctrl.DataContext = Container.Resolve(VM);
            return (T)Ctrl;
        }
    }
}
