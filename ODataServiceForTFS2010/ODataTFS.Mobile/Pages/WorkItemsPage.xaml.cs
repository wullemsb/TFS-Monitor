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
    using System.Windows.Controls;
    using System.Windows.Navigation;
    using Microsoft.Phone.Controls;
    using Microsoft.Phone.Shell;
    using Microsoft.Samples.DPE.ODataTFS.Mobile.ViewModels;
    using Microsoft.Samples.DPE.ODataTFS.Model.Entities;

    public partial class WorkItemsPage : PhoneApplicationPage
    {
        public WorkItemsPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var viewModel = this.DataContext as WorkItemListViewModel;

            // If viewModel is not null it means that the user navigated back to this page and the viewModel is already loaded.
            if (viewModel == null)
            {
                var selectedIndex = string.Empty;
                var projectName = string.Empty;
                var queryId = string.Empty;
                var changesetId = string.Empty;
                var buildId = string.Empty;

                if (this.NavigationContext.QueryString.TryGetValue("projectName", out projectName))
                {
                    viewModel = new WorkItemListViewModel(string.Format(CultureInfo.InvariantCulture, "Projects('{0}')", projectName));
                }
                else if (this.NavigationContext.QueryString.TryGetValue("queryId", out queryId))
                {
                    viewModel = new WorkItemListViewModel(string.Format(CultureInfo.InvariantCulture, "Queries('{0}')", queryId));
                }
                else if (this.NavigationContext.QueryString.TryGetValue("changesetId", out changesetId))
                {
                    viewModel = new WorkItemListViewModel(string.Format(CultureInfo.InvariantCulture, "Changesets({0})", changesetId));
                }
                else if (this.NavigationContext.QueryString.TryGetValue("buildId", out buildId))
                {
                    var keys = buildId.Split(new[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                    viewModel = new WorkItemListViewModel(string.Format(CultureInfo.InvariantCulture, "Builds(Project='{0}',Definition='{1}',Number='{2}')", keys[0], keys[1], keys[2]));
                }
                else
                {
                    viewModel = new WorkItemListViewModel();
                }

                if (this.State.ContainsKey("CurrentPageNumber"))
                {
                    viewModel.PageNumber = (int)this.State["CurrentPageNumber"];
                }

                if (this.State.ContainsKey("CurrentQuery"))
                {
                    viewModel.Query = (string)this.State["CurrentQuery"];
                }

                this.DataContext = viewModel;

                viewModel.LoadData();
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            var viewModel = this.DataContext as WorkItemListViewModel;
            if (viewModel != null)
            {
                if (this.State.ContainsKey("CurrentPageNumber"))
                {
                    this.State.Remove("CurrentPageNumber");
                }

                if (this.State.ContainsKey("CurrentQuery"))
                {
                    this.State.Remove("CurrentQuery");
                }

                this.State.Add("CurrentPageNumber", viewModel.PageNumber);
                this.State.Add("CurrentQuery", viewModel.Query);
            }
        }

        private void FilterItems(object sender, RoutedEventArgs e)
        {
            var viewModel = this.DataContext as WorkItemListViewModel;
            if (viewModel != null)
            {
                viewModel.LoadData();
            }
        }

        private void LoadMoreItems(object sender, RoutedEventArgs e)
        {
            var viewModel = this.DataContext as WorkItemListViewModel;
            if (viewModel != null)
            {
                viewModel.LoadNextPage();
            }
        }

        private void ItemSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // If selected index is -1 (no selection) do nothing
            if (this.ItemsListBox.SelectedIndex == -1)
            {
                return;
            }

            // Set the selected item as the current item.
            var currentWorkItem = this.ItemsListBox.SelectedItem as WorkItem;
            PhoneApplicationService.Current.State["CurrentWorkItem"] = currentWorkItem;

            // Navigate to the new page.
            this.NavigationService.Navigate(new Uri(string.Format(CultureInfo.InvariantCulture, "/Pages/WorkItemDetailsPage.xaml?workItemId={0}", currentWorkItem.Id), UriKind.Relative));

            // Reset selected index to -1 (no selection)
            this.ItemsListBox.SelectedIndex = -1;
        }
    }
}