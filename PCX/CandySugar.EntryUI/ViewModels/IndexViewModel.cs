using Stylet;
using StyletIoC;

namespace CandySugar.EntryUI.ViewModels
{
    public   class  IndexViewModel: Conductor<IScreen>
    {
        public IContainer Container;
        public IWindowManager WindowManager;
        public IndexViewModel(IContainer Container, IWindowManager WindowManager)
        {
            this.Container = Container;
            this.WindowManager = WindowManager;
            this.Title = "IndexView";
        }

        #region Property
        private string _Title;
        public string Title
        {
            get => _Title;
            set => SetAndNotify(ref _Title, value);
        }
        #endregion
    }
}
