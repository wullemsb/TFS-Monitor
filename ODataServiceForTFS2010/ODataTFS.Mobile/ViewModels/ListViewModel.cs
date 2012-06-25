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

namespace Microsoft.Samples.DPE.ODataTFS.Mobile.ViewModels
{
    using System;
    using System.Data.Services.Client;
    using System.Globalization;
    using System.Net;
    using System.Text;
    using System.Windows;
    using Microsoft.Samples.DPE.ODataTFS.Mobile.Helpers;

    public abstract class ListViewModel<T> : ViewModelBase
    {
        private int currentPage = 0;
        private bool searching = false;
        private bool hasResults = false;
        private string query = string.Empty;
        private string noResultsFoundMessage = string.Empty;

        public ListViewModel()
        {
            this.Items = new DataServiceCollection<T>(App.CreateTfsDataServiceContext());
            this.Items.LoadCompleted += this.ItemsLoadCompleted;
        }

        public ListViewModel(string path)
            : this()
        {
            this.Path = path;
        }

        public DataServiceCollection<T> Items { get; set; }

        public int PageNumber { get; set; }

        public string Query
        {
            get
            {
                return this.query;
            }
            set
            {
                this.query = value;
                this.OnPropertyChanged("Query");
            }
        }

        public bool IsSearching
        {
            get
            {
                return this.searching;
            }
            set
            {
                this.searching = value;
                this.OnPropertyChanged("IsSearching");
            }
        }

        public bool HasResults
        {
            get
            {
                return this.hasResults;
            }
            set
            {
                this.hasResults = value;
                this.OnPropertyChanged("HasResults");
            }
        }

        public bool HasContinuation
        {
            get
            {
                return this.Items.Continuation != null;
            }
        }

        public bool LoadMoreResultsVisible
        {
            get
            {
                return this.HasContinuation && !this.IsSearching;
            }
        }

        public string NoResultsFoundMessage
        {
            get
            {
                return this.noResultsFoundMessage;
            }
            set
            {
                this.noResultsFoundMessage = value;
                this.OnPropertyChanged("NoResultsFoundMessage");
            }
        }

        protected string Path { get; set; }

        public virtual void LoadData()
        {
            this.IsSearching = true;

            var requestUri = BuildRequestUri(this.GetEntityCollectionName(), this.Path, this.Query);
            
            this.Items.Clear();
            this.Items.LoadAsync(requestUri);
        }

        public void LoadNextPage()
        {
            if (this.HasContinuation && !this.IsSearching)
            {
                this.IsSearching = true;
                this.Items.LoadNextPartialSetAsync();
            }
        }

        public abstract string GetNoResultsFoundMessage();

        public abstract string GetEntityCollectionName();

        private static Uri BuildRequestUri(string entityCollectionName, string path, string filter)
        {
            var sb = new StringBuilder().Append(App.LoginViewModel.Endpoint.TrimEnd('/'));

            if (!string.IsNullOrEmpty(path))
            {
                sb.AppendFormat(CultureInfo.InvariantCulture, "/{0}/{1}", HttpUtility.UrlEncode(path), HttpUtility.UrlEncode(entityCollectionName));
            }
            else
            {
                sb.AppendFormat(CultureInfo.InvariantCulture, "/{0}", HttpUtility.UrlEncode(entityCollectionName));
            }

            if (!string.IsNullOrEmpty(filter))
            {
                sb = sb.AppendFormat(CultureInfo.InvariantCulture, "?$filter={0}", HttpUtility.UrlEncode(filter));
            }

            return new Uri(sb.ToString(), UriKind.Absolute);
        }

        private void ItemsLoadCompleted(object sender, LoadCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                this.DispatchResult(true, "The operation has been cancelled");
            }
            else if (e.Error != null)
            {
                var message = e.Error.Message;

                Exception dataServiceException;
                if (DataServiceExceptionUtil.TryParse(e.Error, out dataServiceException))
                {
                    message = dataServiceException.Message;
                }
                else if ((e.Error.InnerException != null) && !string.IsNullOrEmpty(e.Error.InnerException.Message))
                {
                    message = e.Error.InnerException.Message;
                }

                this.DispatchResult(true, message);
            }
            else
            {
                this.currentPage++;
                
                if (this.PageNumber < this.currentPage)
                {
                    this.PageNumber = this.currentPage;
                }

                if (this.HasContinuation && this.PageNumber > this.currentPage)
                {
                    this.Items.LoadNextPartialSetAsync();
                }

                this.DispatchResult(false);
            }
        }

        private void DispatchResult(bool hasErrors, string errorMessage = null)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                this.HasResults = false;
                this.IsSearching = false;

                if (!hasErrors)
                {
                    if ((this.Items == null) || (this.Items.Count == 0))
                    {
                        this.NoResultsFoundMessage = this.GetNoResultsFoundMessage();
                    }
                    else
                    {
                        this.HasResults = true;
                    }
                }
                else
                {
                    this.NoResultsFoundMessage = errorMessage;
                }

                this.OnPropertyChanged("LoadMoreResultsVisible");
            });
        }
    }
}
