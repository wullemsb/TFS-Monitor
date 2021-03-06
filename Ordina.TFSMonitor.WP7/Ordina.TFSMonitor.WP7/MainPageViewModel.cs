﻿using System;
using Caliburn.Micro;

namespace Ordina.TFSMonitor.WP7 {
    public class MainPageViewModel:Conductor<IScreen>.Collection.OneActive
    {
        private readonly INavigationService _navigationService;

        public MainPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public void GoToSettings()
        {
            _navigationService
                .UriFor<SettingsPageViewModel>()
                .Navigate();
        }  

        public void GoToAbout()
        {
            _navigationService.Navigate(new Uri("/YourLastAboutDialog;component/AboutPage.xaml", UriKind.Relative));
        }

        protected override void OnInitialize()
         {
             var recentProjects = IoC.Get<RecentProjectsViewModel>();
             recentProjects.DisplayName = "recent";    
             var allProjects = IoC.Get<AllProjectsViewModel>();
             allProjects.DisplayName = "all";
             this.Items.Add(recentProjects);
             this.Items.Add(allProjects);

            ActivateItem(Items[0]);
        }

    }
}
