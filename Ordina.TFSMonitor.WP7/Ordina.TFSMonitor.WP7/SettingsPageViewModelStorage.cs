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

namespace Ordina.TFSMonitor.WP7
{
    public class SettingsPageViewModelStorage : StorageHandler<SettingsPageViewModel>
    {
        public override void Configure()
        {
            Id(x => x.DisplayName);

            Property(x => x.ServiceUrl)
                .InAppSettings()
                .RestoreAfterActivation();
            Property(x => x.Domain)
                .InAppSettings()
                .RestoreAfterActivation();
            Property(x => x.User)
                .InAppSettings()
                .RestoreAfterActivation();
            Property(x => x.Password)
                .InAppSettings()
                .RestoreAfterActivation();
            Property(x => x.AllNotificationsEnabled)
               .InAppSettings()
               .RestoreAfterActivation();
            Property(x => x.BuildNotificationsEnabled)
              .InAppSettings()
              .RestoreAfterActivation();
            Property(x => x.ChangesetNotificationsEnabled)
                .InAppSettings()
                .RestoreAfterActivation();
            Property(x => x.WorkitemNotificationsEnabled)
              .InAppSettings()
              .RestoreAfterActivation();
        }
    }
}
