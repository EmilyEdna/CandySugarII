using Stylet;

namespace CnadySugar.Entry.ViewModels
{
    public class RootViewModel : PropertyChangedBase
    {
        private string _title = "HandyControl Application";
        public string Title
        {
            get { return _title; }
            set { SetAndNotify(ref _title, value); }
        }

        public RootViewModel()
        {

        }
    }
}
