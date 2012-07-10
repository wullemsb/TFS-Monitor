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
using Microsoft.Samples.DPE.ODataTFS.Model.Entities;

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

        public SettingsPageViewModel()
        {
            _serviceUrl = "http://tfs.ordina.be:8080/TFSServicesV2/DefaultCollection";
            _domain = "ordina";
            this.TestStatus = "No test performed";
        }

        public string ServiceUrl
        {
            get { return _serviceUrl; }
            set { _serviceUrl = value; NotifyOfPropertyChange(() => this.ServiceUrl); }
        }

        public string Domain
        {
            get { return _domain; }
            set { _domain = value; NotifyOfPropertyChange(() => this.Domain); }
        }

        public string User
        {
            get { return _user; }
            set { _user = value; NotifyOfPropertyChange(() => this.User); }
        }

        public string Password
        {
            get { return _password; }
            set { _password = value; NotifyOfPropertyChange(() => this.Password); }
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


        public void TestCredentials()
        {
            var context = new TFSData(new Uri(this.ServiceUrl));
            var requestUri = new Uri(string.Format(CultureInfo.InvariantCulture, "{0}/Projects?$top=1&$select=Name", this.ServiceUrl), UriKind.Absolute);

            context.SendingRequest += (s, e) =>
            {
                var credentials = string.Format(@"{0}\{1}:{2}", this.Domain, this.User, this.Password);
                e.RequestHeaders["Authorization"] = string.Format(CultureInfo.InvariantCulture, "Basic {0}", Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(credentials)));
            };

            context.BeginExecute<Project>(
                requestUri,
                r =>
                {
                    try
                    {
                        var result = context.EndExecute<Project>(r) as QueryOperationResponse<Project>;

                        this.TestStatus = "Authentication succeeded";
                    }
                    catch (Exception ex)
                    {
                        this.TestStatus = "Authentication failed";
                    }

                    NotifyOfPropertyChange(() => this.TestStatus);
                },
                null);
        }

        public void Save()
        {
           //TODO
        }

        public void Cancel()
        {
            //TODO
        }
    }
}
