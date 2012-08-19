using System;
using System.Globalization;
using Ordina.TFSMonitor.Win8.Model;

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
        public TFSDataServiceContext(Settings settings) :
            this(new Uri(settings.ServiceUrl))
        {
            this.SendingRequest += (s, e) =>
            {
                var credentials = string.Format(@"{0}\{1}:{2}", settings.Domain, settings.User, settings.Password);
                e.RequestHeaders["Authorization"] = string.Format(CultureInfo.InvariantCulture, "Basic {0}", Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(credentials)));
            };
        }
    }
}
