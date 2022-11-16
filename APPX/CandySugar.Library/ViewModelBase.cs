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
        }
        public virtual void Initialize(INavigationParameters parameters) { }
        public virtual void OnAppearing() { }
        public virtual void OnDisappearing() { }
        public virtual void OnNavigatedFrom(INavigationParameters parameters) { }
        public virtual void OnNavigatedTo(INavigationParameters parameters) { }
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
