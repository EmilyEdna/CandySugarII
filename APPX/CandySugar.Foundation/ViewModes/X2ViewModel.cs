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
        ObservableCollection<ERootEntity> _EResult;
        public ObservableCollection<ERootEntity> EResult { get => _EResult; set => SetProperty(ref _EResult, value); }
        ObservableCollection<FRootEntity> _FResult;
        public ObservableCollection<FRootEntity> FResult { get => _FResult; set => SetProperty(ref _FResult, value); }
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
            if (Type == 4)
            {
                if (Refresh)
                    await Service.ERemove(Root);
                EResult = new ObservableCollection<ERootEntity>(await Service.EQuery());
            }
            if (Type == 5)
            {
                if (Refresh)
                    await Service.FRemove(Root);
                FResult = new ObservableCollection<FRootEntity>(await Service.FQuery());
            }
            //if (Type == 3)
            //{
            //    if (Refresh)
            //        await Service.GRemove(Root);
            //    DResult = new ObservableCollection<DRootEntity>(await Service.DQuery());
            //}
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
        public DelegateCommand<string> ECommand => new(input =>
        {
            Nav.NavigateAsync(new Uri("E1", UriKind.Relative), new NavigationParameters { { "Route", input } });
        });
        public DelegateCommand<FRootEntity> FCommand => new(input =>
        {
            Nav.NavigateAsync(new Uri("F1", UriKind.Relative), new NavigationParameters { { "Route", input.Route }, { "Cover", input.Cover } });
        });
        #endregion
    }
}
