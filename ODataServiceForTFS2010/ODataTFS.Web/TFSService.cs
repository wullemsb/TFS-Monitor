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

namespace Microsoft.Samples.DPE.ODataTFS.Web
{
    using System;
    using System.Data.Services;
    using System.Data.Services.Common;
    using System.Data.Services.Providers;
    using System.Diagnostics;
    using System.Globalization;
    using System.Net;
    using System.ServiceModel;
    using System.ServiceModel.Web;
    using System.Web;
    using Microsoft.Data.Services.Toolkit;
    using Microsoft.Data.Services.Toolkit.ServiceModel;
    using Microsoft.Samples.DPE.ODataTFS.Model;
    using Microsoft.Samples.DPE.ODataTFS.Model.Providers;
    using Microsoft.Samples.DPE.ODataTFS.Model.Serialization;
    using Microsoft.Samples.DPE.ODataTFS.Web.Infrastructure;

    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    [EnableOutputCache]
    [DispatchInspector(typeof(TFSInspector))]
    public class TFSService : ODataService<TFSData>
    {
        public TFSService()
        {
        }

        public ICredentials TFSCredentials
        {
            get
            {
                var user = HttpContext.Current.User.Identity as IBasicUser;
                if (user == null || string.IsNullOrWhiteSpace(user.Name) || string.IsNullOrWhiteSpace(user.Password))
                {
                    return null;
                }

                return new NetworkCredential(user.Name, user.Password, user.Domain);
            }
        }

        public static void InitializeService(DataServiceConfiguration config)
        {
            config.SetEntitySetAccessRule("Builds", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("BuildDefinitions", EntitySetRights.All);
            config.SetEntitySetAccessRule("Changesets", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("Projects", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("WorkItems", EntitySetRights.All);
            config.SetEntitySetAccessRule("Attachments", EntitySetRights.All);
            config.SetEntitySetAccessRule("Changes", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("Queries", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("Branches", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("AreaPaths", EntitySetRights.AllRead);

            config.SetServiceOperationAccessRule("TriggerBuild", ServiceOperationRights.All);

            config.SetEntitySetPageSize("Builds", 20);
            config.SetEntitySetPageSize("BuildDefinitions", 20);
            config.SetEntitySetPageSize("Changesets", 20);
            config.SetEntitySetPageSize("Projects", 20);
            config.SetEntitySetPageSize("WorkItems", 20);
            config.SetEntitySetPageSize("Attachments", 20);
            config.SetEntitySetPageSize("Changes", 20);
            config.SetEntitySetPageSize("Queries", 20);
            config.SetEntitySetPageSize("Branches", 20);

            config.DataServiceBehavior.MaxProtocolVersion = DataServiceProtocolVersion.V2;
            config.UseVerboseErrors = true;
        }

        public override object GetService(Type serviceType)
        {
            if (serviceType == typeof(IDataServiceStreamProvider))
            {
                return new TFSStreamProvider(this.TFSCredentials);
            }

            return base.GetService(serviceType);
        }

        [WebInvoke(Method = "POST")]
        public void TriggerBuild()
        {
            var project = HttpContext.Current.Request["project"];
            var definition = HttpContext.Current.Request["definition"];

            this.CurrentDataSource.TriggerBuild(project, definition);
        }

        protected override TFSData CreateDataSource()
        {
            var request = OperationContext.Current.IncomingMessageProperties;
            if (!request.ContainsKey("TfsCollectionName"))
            {
                throw new DataServiceException(400, "Bad Request", "You need to specify the name of a TFS Project Collection as the first part of the URL.", "en-us", null);
            }

            var collection = request["TfsCollectionName"] as string;
            var tfsCollectionUri = string.Format(CultureInfo.InvariantCulture, "{0}/{1}", ConfigReader.GetConfigValue("ODataTFS.TfsServer").TrimEnd('/'), collection);

            return new TFSData(new TFSProxyFactory(new Uri(tfsCollectionUri, UriKind.Absolute), this.TFSCredentials));
        }

        protected override void HandleException(HandleExceptionArgs args)
        {
            if (args.Exception is DataServiceException && ((DataServiceException)args.Exception).StatusCode == 500)
            {
                this.ValidateTeamProjectCollectionAndAuthorization();
            }

            if ((args != null) && (args.Exception != null))
            {
                Trace.TraceError(args.Exception.ToString());
            }

            base.HandleException(args);
        }

        private void ValidateTeamProjectCollection(object sender, DataServiceProcessingPipelineEventArgs e)
        {
            this.ValidateTeamProjectCollectionAndAuthorization();
        }

        private void ValidateTeamProjectCollectionAndAuthorization()
        {
            var collection = OperationContext.Current.IncomingMessageProperties["TfsCollectionName"] as string;
            var tfs = ConfigReader.GetConfigValue("ODataTFS.TfsServer").TrimEnd('/');
            var collectionUri = new Uri(string.Format(CultureInfo.InvariantCulture, "{0}/{1}", tfs, collection));

            if (this.TFSCredentials != null)
            {
                var isAuthorized = false;
                var collectionExists = TFSBaseProxy.CollectionExists(collectionUri, this.TFSCredentials, out isAuthorized);
                if (!collectionExists)
                {
                    throw new DataServiceException(404, "Not found", string.Format(CultureInfo.InvariantCulture, "The TFS Project Collection named {0} was not found.", collection), "en-US", null);
                }
                else if (!isAuthorized)
                {
                    this.SendAuthHeader();
                    throw new DataServiceException(401, "Not authorized", string.Format(CultureInfo.InvariantCulture, "You are not authorized to view the TFS Project Collection named {0}.", collection), "en-US", null);
                }
            }
        }
        
        private void SendAuthHeader()
        {
            var context = HttpContext.Current;
            var tfsServerUri = new Uri(ConfigReader.GetConfigValue("ODataTFS.TfsServer"), UriKind.Absolute);

            context.Response.Clear();
            context.Response.ContentType = "application/xml";
            context.Response.StatusCode = 401;
            context.Response.StatusDescription = "Unauthorized";
            context.Response.AddHeader("WWW-Authenticate", string.Format(CultureInfo.InvariantCulture, "Basic realm=\"{0}\"", tfsServerUri.AbsoluteUri));
            context.Response.AddHeader("DataServiceVersion", "1.0");
            context.Response.Write(@"<?xml version=""1.0"" encoding=""utf-8"" standalone=""yes"" ?>
                                     <error xmlns=""http://schemas.microsoft.com/ado/2007/08/dataservices/metadata"">
                                         <code>Not authorized</code>
                                         <message xml:lang=""en-US"">Please provide valid TFS credentials (domain\\username and password)</message>
                                     </error>");
            context.Response.End();
        }
    }
}