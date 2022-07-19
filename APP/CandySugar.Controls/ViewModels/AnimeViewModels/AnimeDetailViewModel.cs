using CandySugar.Controls.Views.AnimeViews;
using Sdk.Component.Anime.sdk.ViewModel.Response;

namespace CandySugar.Controls.ViewModels.AnimeViewModels
{
    public class AnimeDetailViewModel : BaseViewModel
    {
        public override void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            var data = (List<AnimeDetailResult>)query["Route"];
            Name = data.FirstOrDefault().Name;
            Cover = data.FirstOrDefault().Cover;
            Query = new ObservableCollection<AnimeDetailResult>(data);
        }

        #region 属性
        ObservableCollection<AnimeDetailResult> _Qeury;
        public ObservableCollection<AnimeDetailResult> Query
        {
            get => _Qeury;
            set => SetProperty(ref _Qeury, value);
        }
        string _Name;
        public string Name
        {
            get => _Name;
            set => SetProperty(ref _Name, value);
        }
        string _Cover;
        public string Cover
        {
            get => _Cover;
            set => SetProperty(ref _Cover, value);
        }
        #endregion

        #region 命令
        public DelegateCommand<string> ViewAction => new(input => Navigation(input));
        #endregion

        #region 方法
        async void Navigation(string input) 
        {
            await Shell.Current.GoToAsync($"{nameof(AnimePlayView)}?Key={input}");
        }
        #endregion
    }
}
