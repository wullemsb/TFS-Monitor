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

    public partial class ChangesPage : PhoneApplicationPage
    {
        public ChangesPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var viewModel = this.DataContext as ChangeListViewModel;

            // If viewModel is not null it means that the user navigated back to this page and the viewModel is already loaded.
            if (viewModel == null)
            {
                var changesetId = string.Empty;

                if (this.NavigationContext.QueryString.TryGetValue("changesetId", out changesetId))
                {
                    viewModel = new ChangeListViewModel(string.Format(CultureInfo.InvariantCulture, "Changesets({0})", changesetId));
                }
                else
                {
                    viewModel = new ChangeListViewModel();
                }

                if (this.State.ContainsKey("CurrentPageNumber"))
                {
                    viewModel.PageNumber = (int)this.State["CurrentPageNumber"];
                }

                if (this.State.ContainsKey("CurrentQuery"))
                {
                    viewModel.Query = (string)this.State["CurrentQuery"];
                }

                viewModel.LoadData();

                this.DataContext = viewModel;
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            var viewModel = this.DataContext as ChangeListViewModel;
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
            var viewModel = this.DataContext as ChangeListViewModel;
            if (viewModel != null)
            {
                viewModel.LoadData();
            }
        }

        private void LoadMoreItems(object sender, RoutedEventArgs e)
        {
            var viewModel = this.DataContext as ChangeListViewModel;
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
            var currentChange = this.ItemsListBox.SelectedItem as Change;
            PhoneApplicationService.Current.State["CurrentChange"] = currentChange;

            // Navigate to the new page.
            this.NavigationService.Navigate(new Uri(string.Format(CultureInfo.InvariantCulture, "/Pages/ChangeDetailsPage.xaml?ChangesetId={0}&Path={1}", currentChange.Changeset, currentChange.Path), UriKind.Relative));

            // Reset selected index to -1 (no selection)
            this.ItemsListBox.SelectedIndex = -1;
        }
    }
}