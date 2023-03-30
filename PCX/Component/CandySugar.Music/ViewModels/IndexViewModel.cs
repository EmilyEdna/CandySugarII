namespace CandySugar.Music.ViewModels
{
    public class IndexViewModel : PropertyChangedBase
    {
        public IndexViewModel()
        {
            MenuIndex = new ObservableCollection<string> { "网易音乐", "QQ音乐", "酷我音乐", "酷狗音乐", "咪咕音乐" };
        }


        #region Property
        private ObservableCollection<string> _MenuIndex;
        public ObservableCollection<string> MenuIndex
        {
            get => _MenuIndex;
            set => SetAndNotify(ref _MenuIndex, value);
        }
        #endregion
    }
}
