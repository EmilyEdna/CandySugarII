using CandySugar.Library;
using CandySugar.Logic;
using System.Collections.ObjectModel;
using XExten.Advance.LinqFramework;
using static SQLite.SQLite3;

namespace CandySugar.Foundation
{
    public class X2ViewModel : ViewModelBase
    {
        readonly IService Service;
        public X2ViewModel(BaseServices baseServices) : base(baseServices)
        {
            Service = this.Container.Resolve<IService>();
        }
        #region Property
        public int Type { get; set; }
        #endregion

        #region Property
        ObservableCollection<BRootEntity> _BResult;
        public ObservableCollection<BRootEntity> BResult { get => _BResult; set => SetProperty(ref _BResult, value); }
        #endregion

        #region Method
        async void Query()
        {
            if (Type == 1)
            {
                BResult = new ObservableCollection<BRootEntity>(await Service.BQuery(string.Empty));
            }
        }

        #endregion


        #region Command
        public DelegateCommand BackCommand => new(() =>
        {
            Nav.GoBackAsync();
        });
        public DelegateCommand<string> WatchCommand => new(input =>
        {

            Type = input.AsInt();
            Query();
        });
        public DelegateCommand<string> BCommand => new(input => { });
        #endregion
    }
}
