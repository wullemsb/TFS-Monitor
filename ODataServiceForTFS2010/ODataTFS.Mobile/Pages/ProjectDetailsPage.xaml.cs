// ----------------------------------------------------------------------------------
// Microsoft Developer & Platform Evangelism
// 
// Copyright (c) Microsoft Corporation. All rights reserved.
// 
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
// OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// ----------------------------------------------------------------------------------
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
// ----------------------------------------------------------------------------------

namespace Microsoft.Samples.DPE.ODataTFS.Mobile.Pages
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Navigation;
    using Microsoft.Phone.Controls;
    using Microsoft.Phone.Shell;
    using Microsoft.Samples.DPE.ODataTFS.Model.Entities;

    public partial class ProjectDetailsPage : PhoneApplicationPage
    {
        public ProjectDetailsPage()
        {
            this.InitializeComponent();
        }
        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var viewModel = PhoneApplicationService.Current.State["CurrentProject"] as Project;
            if (viewModel != null)
            {
                this.DataContext = viewModel;
            }
        }

        private void QueriesLinkClick(object sender, RoutedEventArgs e)
        {
            var viewModel = this.DataContext as Project;
            if (viewModel != null)
            {
                this.NavigationService.Navigate(new Uri(string.Format(CultureInfo.InvariantCulture, "/Pages/QueriesPage.xaml?projectName={0}", viewModel.Name), UriKind.Relative));
            }
        }

        private void WorkItemsLinkClick(object sender, RoutedEventArgs e)
        {
            var viewModel = this.DataContext as Project;
            if (viewModel != null)
            {
                this.NavigationService.Navigate(new Uri(string.Format(CultureInfo.InvariantCulture, "/Pages/WorkItemsPage.xaml?projectName={0}", viewModel.Name), UriKind.Relative));
            }
        }

        private void BranchesLinkClick(object sender, RoutedEventArgs e)
        {
            var viewModel = this.DataContext as Project;
            if (viewModel != null)
            {
                this.NavigationService.Navigate(new Uri(string.Format(CultureInfo.InvariantCulture, "/Pages/BranchesPage.xaml?projectName={0}", viewModel.Name), UriKind.Relative));
            }
        }

        private void ChangesetsLinkClick(object sender, RoutedEventArgs e)
        {
            var viewModel = this.DataContext as Project;
            if (viewModel != null)
            {
                this.NavigationService.Navigate(new Uri(string.Format(CultureInfo.InvariantCulture, "/Pages/ChangesetsPage.xaml?projectName={0}", viewModel.Name), UriKind.Relative));
            }
        }

        private void BuildsLinkClick(object sender, RoutedEventArgs e)
        {
            var viewModel = this.DataContext as Project;
            if (viewModel != null)
            {
                this.NavigationService.Navigate(new Uri(string.Format(CultureInfo.InvariantCulture, "/Pages/BuildsPage.xaml?projectName={0}", viewModel.Name), UriKind.Relative));
            }
        }
    }
}