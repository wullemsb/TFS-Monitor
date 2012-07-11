using System;
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
    public class RecentProjectsViewModel:Screen
    {
        public RecentProjectsViewModel()
        {
            this.DisplayName = "recent";

        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            this.RecentProjects=new BindableCollection<Project>();
            for (int i = 0; i < 5;i++ )
                this.RecentProjects.Add(new Project() { Name = "Test"+i });
        }

        public IObservableCollection<Project> RecentProjects { get; set; }
    }
}
