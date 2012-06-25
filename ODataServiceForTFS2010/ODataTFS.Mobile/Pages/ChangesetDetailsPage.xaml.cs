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

    public partial class ChangesetDetailsPage : PhoneApplicationPage
    {
        public ChangesetDetailsPage()
        {
            this.InitializeComponent();
        }
        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var viewModel = PhoneApplicationService.Current.State["CurrentChangeset"] as Changeset;
            if (viewModel != null)
            {
                this.DataContext = viewModel;
            }
        }

        private void WorkItemsLinkClick(object sender, RoutedEventArgs e)
        {
            var viewModel = this.DataContext as Changeset;
            if (viewModel != null)
            {
                this.NavigationService.Navigate(new Uri(string.Format(CultureInfo.InvariantCulture, "/Pages/WorkItemsPage.xaml?changesetId={0}", viewModel.Id), UriKind.Relative));
            }
        }

        private void ChangesLinkClick(object sender, RoutedEventArgs e)
        {
            var viewModel = this.DataContext as Changeset;
            if (viewModel != null)
            {
                this.NavigationService.Navigate(new Uri(string.Format(CultureInfo.InvariantCulture, "/Pages/ChangesPage.xaml?changesetId={0}", viewModel.Id), UriKind.Relative));
            }
        }
    }
}