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

    public partial class WorkItemDetailsPage : PhoneApplicationPage
    {
        public WorkItemDetailsPage()
        {
            this.InitializeComponent();
        }
        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var viewModel = PhoneApplicationService.Current.State["CurrentWorkItem"] as WorkItem;
            if (viewModel != null)
            {
                this.DataContext = viewModel;
            }
        }

        private void AttachmentsLinkClick(object sender, RoutedEventArgs e)
        {
            var viewModel = this.DataContext as WorkItem;
            if (viewModel != null)
            {
                this.NavigationService.Navigate(new Uri(string.Format(CultureInfo.InvariantCulture, "/Pages/AttachmentsPage.xaml?workItemId={0}", viewModel.Id), UriKind.Relative));
            }
        }
    }
}