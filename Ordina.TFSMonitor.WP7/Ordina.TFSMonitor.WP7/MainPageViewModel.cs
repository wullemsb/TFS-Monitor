using Caliburn.Micro;

namespace Ordina.TFSMonitor.WP7 {
    public class MainPageViewModel
    {
        private readonly INavigationService _navigationService;

        public MainPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public void GotoSettings()
        {
            _navigationService
                .UriFor<SettingsPageViewModel>()
                .Navigate();
        }  


    }
}
