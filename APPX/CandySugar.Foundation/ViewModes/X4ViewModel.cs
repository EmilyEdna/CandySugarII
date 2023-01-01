using CandySugar.Library;
using CandySugar.Logic;
using NStandard;
using System.Collections.ObjectModel;

namespace CandySugar.Foundation
{
    public class X4ViewModel : ViewModelBase
    {
        readonly IService Service;
        public X4ViewModel(BaseServices baseServices) : base(baseServices)
        {
            Service = this.Container.Resolve<IService>();
            
        }
        public override void OnLoad()
        {
            this.IsActivity = true;
            this.IsNotActivity = false;
            Task.Run(Query);
        }

        #region Property
        ObservableCollection<LogEntity> _Result;
        public ObservableCollection<LogEntity> Result
        {
            get => _Result;
            set => SetProperty(ref _Result, value);
        }
        string _Trace;
        public string Trace
        {
            get => _Trace;
            set => SetProperty(ref _Trace, value);
        }
        bool _IsActivity;
        public bool IsActivity
        {
            get { return _IsActivity; }
            set { SetProperty(ref _IsActivity, value); }
        }
        bool _IsNotActivity;
        public bool IsNotActivity
        {
            get { return _IsNotActivity; }
            set { SetProperty(ref _IsNotActivity, value); }
        }
        #endregion

        #region Command
        public DelegateCommand BackCommand => new(() =>
        {
            Nav.GoBackAsync();
        });
        public DelegateCommand ReBackCommand => new(() =>
        {
            this.IsActivity = true;
            this.IsNotActivity = false;
        });
        public DelegateCommand<string> DetailCommand => new(input =>
        {
            this.IsActivity = false;
            this.IsNotActivity = true;
            Trace = input;
        });
        #endregion

        #region Method
        async void Query()
        {
            Result = new ObservableCollection<LogEntity>(await Service.QueryLog());
        }
        #endregion
    }
}
