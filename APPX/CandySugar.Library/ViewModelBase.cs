using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Library
{
    public abstract class ViewModelBase : BindableBase, IInitialize, INavigatedAware, IPageLifecycleAware
    {
        protected INavigationService _navigationService { get; }
        protected IPageDialogService _pageDialogs { get; }
        protected IDialogService _dialogs { get; }
        protected ViewModelBase(BaseServices baseServices)
        {
            _navigationService = baseServices.NavigationService;
            _pageDialogs = baseServices.PageDialogs;
            _dialogs = baseServices.Dialogs;
            this.Activity = false;
            this.Refresh = false;
            this.Page = 1;
            OnLoad();
        }
        public virtual void Initialize(INavigationParameters parameters) { }
        public virtual void OnAppearing() { }
        public virtual void OnDisappearing() { }
        public virtual void OnLoad() { }
        public virtual void OnNavigatedFrom(INavigationParameters parameters) { }
        public virtual void OnNavigatedTo(INavigationParameters parameters) { }

        #region Property
        bool _Refresh;
        public bool Refresh
        {
            get { return _Refresh; }
            set { SetProperty(ref _Refresh, value); }
        }
        bool _Activity;
        public bool Activity
        {
            get { return _Activity; }
            set { SetProperty(ref _Activity, value); }
        }
        int _Page;
        public int Page
        {
            get { return _Page; }
            set { SetProperty(ref _Page, value); }
        }
        int _Total;
        public int Total
        {
            get => _Total;
            set => SetProperty(ref _Total, value);
        }
        #endregion

    }
    public class BaseServices
    {
        public BaseServices(INavigationService navigationService, IPageDialogService pageDialogs, IDialogService dialogService, IDialogViewRegistry dialogRegistry)
        {
            NavigationService = navigationService;
            PageDialogs = pageDialogs;
            Dialogs = dialogService;
            DialogRegistry = dialogRegistry;
        }

        public INavigationService NavigationService { get; }
        public IPageDialogService PageDialogs { get; }
        public IDialogService Dialogs { get; }
        public IDialogViewRegistry DialogRegistry { get; }
    }
}
