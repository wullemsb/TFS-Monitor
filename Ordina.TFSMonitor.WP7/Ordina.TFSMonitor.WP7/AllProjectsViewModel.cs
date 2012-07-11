using System;
using System.Data.Services.Client;
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
    public class AllProjectsViewModel:Screen
    {
        private readonly SettingsPageViewModel _settings;
        private DataServiceCollection<Project> _collection; 

        public AllProjectsViewModel(SettingsPageViewModel settings)
        {
            _settings = settings;
            this.DisplayName = "all";
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            LoadAvailableProjects();
        }

        private void LoadAvailableProjects()
        {
            var uri = new Uri(_settings.ServiceUrl+"/Projects");
            var context = IoC.Get<TFSDataServiceContext>();
            _collection = new DataServiceCollection<Project>(context);
            _collection.LoadCompleted += this.ItemsLoadCompleted;
            _collection.LoadAsync(uri);
        }

        private void ItemsLoadCompleted(object sender, LoadCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                
            }
            else if (e.Error != null)
            {
               
            }
            else
            {
                AllProjects = new BindableCollection<Project>(_collection);
                this.NotifyOfPropertyChange(()=> this.AllProjects);
            }
        }

        public IObservableCollection<Project> AllProjects { get; set; }
    }
}
