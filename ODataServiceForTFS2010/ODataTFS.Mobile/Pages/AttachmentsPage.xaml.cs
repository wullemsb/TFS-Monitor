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

    public partial class AttachmentsPage : PhoneApplicationPage
    {
        public AttachmentsPage()
        {
            this.InitializeComponent();
        }
        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var viewModel = this.DataContext as AttachmentListViewModel;

            // If viewModel is not null it means that the user navigated back to this page and the viewModel is already loaded.
            if (viewModel == null)
            {
                string workItemId = string.Empty;
                
                if (this.NavigationContext.QueryString.TryGetValue("workItemId", out workItemId))
                {
                    viewModel = new AttachmentListViewModel(string.Format(CultureInfo.InvariantCulture, "WorkItems({0})", workItemId));
                    if (this.State.ContainsKey("CurrentPageNumber"))
                    {
                        viewModel.PageNumber = (int)this.State["CurrentPageNumber"];
                    }

                    viewModel.LoadData();

                    this.DataContext = viewModel;
                }
                else
                {
                    MessageBox.Show("Navigating from a page different to the 'Work Items' page is not supported.", "Not Supported", MessageBoxButton.OK);
                }
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            var viewModel = this.DataContext as AttachmentListViewModel;
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
            var viewModel = this.DataContext as AttachmentListViewModel;
            if (viewModel != null)
            {
                viewModel.LoadData();
            }
        }

        private void LoadMoreItems(object sender, RoutedEventArgs e)
        {
            var viewModel = this.DataContext as AttachmentListViewModel;
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

            // Sets the selected item as the current item.
            var currentAttachment = this.ItemsListBox.SelectedItem as Attachment;
            PhoneApplicationService.Current.State["CurrentAttachment"] = currentAttachment;
            
            // Navigate to the new page.
            this.NavigationService.Navigate(new Uri(string.Format(CultureInfo.InvariantCulture, "/Pages/AttachmentDetailsPage.xaml?workItemId={0}&index={1}", currentAttachment.WorkItemId, currentAttachment.Index), UriKind.Relative));

            // Reset selected index to -1 (no selection)
            this.ItemsListBox.SelectedIndex = -1;
        }
    }
}