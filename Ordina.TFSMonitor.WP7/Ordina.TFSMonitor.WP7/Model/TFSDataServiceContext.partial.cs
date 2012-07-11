using System;
using System.Globalization;
using Ordina.TFSMonitor.WP7;

namespace Ordina.TFSMonitor.Model.Entities
{
    /// <summary>
    /// There are no comments for TFSData in the schema.
    /// </summary>
    public partial class TFSDataServiceContext : global::System.Data.Services.Client.DataServiceContext
    {
        /// <summary>
        /// Initialize a new TFSData object.
        /// </summary>
        public TFSDataServiceContext(SettingsPageViewModel viewModel) :
            this(new Uri(viewModel.ServiceUrl))
        {
            this.SendingRequest += (s, e) =>
            {
                var credentials = string.Format(@"{0}\{1}:{2}", viewModel.Domain, viewModel.User, viewModel.Password);
                e.RequestHeaders["Authorization"] = string.Format(CultureInfo.InvariantCulture, "Basic {0}", Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(credentials)));
            };
        }
    }
}
