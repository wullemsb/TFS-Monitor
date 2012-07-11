using System;
using System.Data.Services.Client;
using System.Globalization;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Caliburn.Micro;
using Ordina.TFSMonitor.Model.Entities;

namespace Ordina.TFSMonitor.WP7
{
    public class SettingsPageViewModel : Screen
    {
        private string _serviceUrl;
        private string _domain;
        private string _user;
        private string _password;
        private bool _allNotificationsEnabled;
        private bool _workitemNotificationsEnabled;
        private bool _changesetNotificationsEnabled;
        private bool _buildNotificationsEnabled;
        private bool _isAuthenticated;

        public SettingsPageViewModel()
        {
            _serviceUrl = "http://tfs.ordina.be:8080/TFSServicesV2/DefaultCollection";
            _domain = "ordina";
            this.TestStatus = "No test performed";
        }

        public string ServiceUrl
        {
            get { return _serviceUrl; }
            set { _serviceUrl = value; NotifyOfPropertyChange(() => this.ServiceUrl); NotifyOfPropertyChange(() => this.CanTestCredentials); }
        }

        public string Domain
        {
            get { return _domain; }
            set { _domain = value; NotifyOfPropertyChange(() => this.Domain); NotifyOfPropertyChange(() => this.CanTestCredentials); }
        }

        public string User
        {
            get { return _user; }
            set { _user = value; NotifyOfPropertyChange(() => this.User); NotifyOfPropertyChange(() => this.CanTestCredentials); }
        }

        public string Password
        {
            get { return _password; }
            set { _password = value; NotifyOfPropertyChange(() => this.Password); NotifyOfPropertyChange(() => this.CanTestCredentials); }
        }

        public bool AllNotificationsEnabled
        {
            get { return _allNotificationsEnabled; }
            set
            {
                _allNotificationsEnabled = value;
                NotifyOfPropertyChange(() => this.AllNotificationsEnabled);
                WorkitemNotificationsEnabled = value;
                ChangesetNotificationsEnabled = value;
                BuildNotificationsEnabled = value;
            }
        }

        public bool WorkitemNotificationsEnabled
        {
            get { return _workitemNotificationsEnabled; }
            set { _workitemNotificationsEnabled = value; NotifyOfPropertyChange(() => this.WorkitemNotificationsEnabled); }
        }

        public bool ChangesetNotificationsEnabled
        {
            get { return _changesetNotificationsEnabled; }
            set { _changesetNotificationsEnabled = value; NotifyOfPropertyChange(() => this.ChangesetNotificationsEnabled); }
        }

        public bool BuildNotificationsEnabled
        {
            get { return _buildNotificationsEnabled; }
            set { _buildNotificationsEnabled = value; NotifyOfPropertyChange(() => this.BuildNotificationsEnabled); }
        }

        public string TestStatus { get; private set; }


        public bool CanTestCredentials
        {
            get
            {
                return !string.IsNullOrWhiteSpace(this.ServiceUrl) && !string.IsNullOrWhiteSpace(User) &&
                       !string.IsNullOrWhiteSpace(Domain) && !string.IsNullOrWhiteSpace(Password);
            }
        }

        public bool IsAuthenticated
        {
            get { return _isAuthenticated; }
            set { _isAuthenticated = value; NotifyOfPropertyChange(() => this.IsAuthenticated); }
        }

        public void TestCredentials()
        {
            var context = IoC.Get<TFSDataServiceContext>();
            var requestUri = new Uri(string.Format(CultureInfo.InvariantCulture, "{0}/Projects?$top=1&$select=Name", this.ServiceUrl), UriKind.Absolute);

            //TODO: replace by IResult syntax
            context.BeginExecute<Project>(
                requestUri,
                r =>
                {
                    try
                    {
                        var result = context.EndExecute<Project>(r) as QueryOperationResponse<Project>;

                        this.IsAuthenticated = true;
                        this.TestStatus = "Authentication succeeded";
                    }
                    catch (Exception ex)
                    {
                        this.IsAuthenticated = true;
                        this.TestStatus = "Authentication failed";
                    }

                    NotifyOfPropertyChange(() => this.TestStatus);
                },
                null);
        }
    }
}
