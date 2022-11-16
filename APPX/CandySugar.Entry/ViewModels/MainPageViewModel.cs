using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Entry.ViewModels
{
    public class MainPageViewModel : IPageLifecycleAware
    {
        private INavigationService _navigationService { get; }

        public MainPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }
        public void OnAppearing()
        {
        }

        public void OnDisappearing()
        {
        }
    }
}
