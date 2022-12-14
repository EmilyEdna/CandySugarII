using CandySugar.Library;
using CandySugar.Logic;
using System.Collections.ObjectModel;
using XExten.Advance.LinqFramework;


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
        public Guid Root { get; set; }
        #endregion

        #region Property
        ObservableCollection<BRootEntity> _BResult;
        public ObservableCollection<BRootEntity> BResult { get => _BResult; set => SetProperty(ref _BResult, value); }
        ObservableCollection<CRootEntity> _CResult;
        public ObservableCollection<CRootEntity> CResult { get => _CResult; set => SetProperty(ref _CResult, value); }
        ObservableCollection<DRootEntity> _DResult;
        public ObservableCollection<DRootEntity> DResult { get => _DResult; set => SetProperty(ref _DResult, value); }
        #endregion

        #region Method
        async void Query(bool Refresh=false)
        {
            if (Type == 1)
            {
                if (Refresh)
                   await Service.BRemove(Root);
                BResult = new ObservableCollection<BRootEntity>(await Service.BQuery());
            }
            if (Type == 2)
            {
                if (Refresh)
                    await Service.CRemove(Root);
                CResult = new ObservableCollection<CRootEntity>(await Service.CQuery());
            }
            if (Type == 3)
            {
                if (Refresh)
                    await Service.DRemove(Root);
                DResult = new ObservableCollection<DRootEntity>(await Service.DQuery());
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
        public DelegateCommand<dynamic> DelCommand => new(input =>
        {
            Root = input;
            Query(true);
        });
        public DelegateCommand<string> BCommand => new(input =>
        {
            Nav.NavigateAsync(new Uri("B2", UriKind.Relative), new NavigationParameters { { "Route", input } });
        });
        public DelegateCommand<string> CCommand => new(input =>
        {
            Nav.NavigateAsync(new Uri("C1", UriKind.Relative), new NavigationParameters { { "Route", input } });
        });
        public DelegateCommand<DRootEntity> DCommand => new(input =>
        {
            Nav.NavigateAsync(new Uri("D1", UriKind.Relative), new NavigationParameters { { "Route", input.Route }, { "Cover", input.Cover } });
        });
        #endregion
    }
}
