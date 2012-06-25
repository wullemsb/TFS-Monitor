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
    using System.Windows;
    using Microsoft.Samples.DPE.ODataTFS.Mobile.Helpers;
    using Microsoft.Samples.DPE.ODataTFS.Model.Entities;

    public class LoginViewModel : ViewModelBase
    {
        private string username;
        private string password;
        private string domain;
        private string endpoint;
        private bool logingIn;

        public bool IsLoginEnabled
        {
            get
            {
                return !string.IsNullOrEmpty(this.Username)
                        && !string.IsNullOrEmpty(this.Password)
                        && !string.IsNullOrEmpty(this.Endpoint);
            }
        }

        public string Username
        {
            get
            {
                return this.username;
            }

            set
            {
                this.username = value;

                this.OnPropertyChanged("Username");
                this.OnPropertyChanged("IsLoginEnabled");
            }
        }

        public string Password
        {
            get
            {
                return this.password;
            }

            set
            {
                this.password = value;
                this.OnPropertyChanged("Password");
                this.OnPropertyChanged("IsLoginEnabled");
            }
        }

        public string Domain
        {
            get
            {
                return this.domain;
            }

            set
            {
                this.domain = value;
                this.OnPropertyChanged("Domain");
            }
        }

        public string Endpoint
        {
            get
            {
                return this.endpoint;
            }

            set
            {
                this.endpoint = value;

                this.OnPropertyChanged("Endpoint");
                this.OnPropertyChanged("IsLoginEnabled");
            }
        }

        public bool LogingIn
        {
            get
            {
                return this.logingIn;
            }

            set
            {
                this.logingIn = value;
                this.OnPropertyChanged("LogingIn");
            }
        }

        public void Login(Action<LoginResult> callback)
        {
            var errorMessage = string.Empty;
            if (!this.Validate(out errorMessage))
            {
                callback.Invoke(new LoginResult { IsAuthenticated = false, Description = errorMessage });
                return;
            }

            this.LogingIn = true;
            var context = App.CreateTfsDataServiceContext();
            var requestUri = new Uri(string.Format(CultureInfo.InvariantCulture, "{0}/Projects?$top=1&$select=Name", this.Endpoint), UriKind.Absolute);

            context.BeginExecute<Project>(
                requestUri,
                r =>
                {
                    try
                    {
                        var result = context.EndExecute<Project>(r) as QueryOperationResponse<Project>;
                        if (result != null)
                        {
                            // Save the valid credentials in the Isolated Storage
                            IsolatedStorageHelper.ClearCredentials(App.IsolatedStorageFileName);
                            IsolatedStorageHelper.SaveCredentials(this, App.IsolatedStorageFileName);

                            this.DispatchResult(callback, new LoginResult { IsAuthenticated = true, Description = string.Empty });
                        }
                    }
                    catch (Exception ex)
                    {
                        Exception dataServiceException;
                        var result = new LoginResult { IsAuthenticated = false, Description = ex.Message };

                        if (DataServiceExceptionUtil.TryParse(ex, out dataServiceException))
                        {
                            result.Description = dataServiceException.Message;
                        }
                        else if (ex.InnerException != null)
                        {
                            result.Description = ex.InnerException.Message;
                        }

                        this.DispatchResult(callback, result);
                    }
                },
                null);
        }

        private bool Validate(out string errorMessage)
        {
            errorMessage = string.Empty;

            Uri tfsServerUri;
            if (!Uri.TryCreate(this.endpoint, UriKind.Absolute, out tfsServerUri))
            {
                errorMessage = "The OData service for Team Foundation Server endpoint URL is not valid.";
                return false;
            }

            if ((tfsServerUri.Scheme != Uri.UriSchemeHttp) && (tfsServerUri.Scheme != Uri.UriSchemeHttps))
            {
                errorMessage = "The Uri scheme used is not supported. Please use HTTP or HTTPS schemes.";
                return false;
            }

            return true;
        }

        private void DispatchResult(Action<LoginResult> callback, LoginResult result)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                this.LogingIn = false;
                callback.Invoke(result);
            });
        }
    }
}
