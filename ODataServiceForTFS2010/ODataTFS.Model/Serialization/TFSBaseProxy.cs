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

namespace Microsoft.Samples.DPE.ODataTFS.Model.Serialization
{
    using System;
    using System.Data.Services;
    using System.Linq;
    using System.Net;
    using Microsoft.Samples.DPE.ODataTFS.Model.ExpressionVisitors;
    using Microsoft.TeamFoundation;
    using Microsoft.TeamFoundation.Client;

    public class TFSBaseProxy : IDisposable
    {
        private TfsConnection tfsConnection;

        public TFSBaseProxy(Uri tfsCollection, ICredentials credentials)
        {
            this.TfsConnection = new TfsTeamProjectCollection(tfsCollection, credentials);
        }

        protected TfsConnection TfsConnection
        {
            get { return this.tfsConnection; }
            set { this.tfsConnection = value; }
        }

        public static bool CollectionExists(Uri collectionUri, ICredentials credentials, out bool isAuthorized)
        {
            TfsTeamProjectCollection collection = null;

            try
            {
                collection = new TfsTeamProjectCollection(collectionUri, credentials);
                collection.EnsureAuthenticated();
                isAuthorized = true;

                return true;
            }
            catch (TeamFoundationServerUnauthorizedException)
            {
                isAuthorized = false;

                return true;
            }
            catch
            {
                isAuthorized = false;

                return false;
            }
            finally
            {
                collection.Dispose();
                collection = null;
            }
        }

        public static bool IsAuthenticated(Uri tfsUri, ICredentials credentials)
        {
            return true;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected TeamFoundation.WorkItemTracking.Client.WorkItemCollection QueryWorkItems(string wiql)
        {
            var workItemServer = this.TfsConnection.GetService<TeamFoundation.WorkItemTracking.Client.WorkItemStore>();

            try
            {
                return workItemServer.Query(wiql);
            }
            catch (Microsoft.TeamFoundation.WorkItemTracking.Client.ValidationException ex)
            {
                throw new DataServiceException(500, "Internal Server Error", ex.Message, "en-US", ex);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.tfsConnection != null)
                {
                    this.tfsConnection.Dispose();
                    this.tfsConnection = null;
                }
            }
        }
        
        protected Uri GetTfsWebAccessArtifactUrl(Uri uri)
        {
            var hyperlinkService = this.TfsConnection.GetService<Microsoft.TeamFoundation.Client.TswaClientHyperlinkService>();

            // For CodePlex TFS we need to convine the base URL with the path and query.
            return new Uri(this.TfsConnection.Uri, hyperlinkService.GetArtifactViewerUrl(uri).PathAndQuery);
        }
        
        protected virtual string GetFilterNodeKey(FilterNode rootFilterNode)
        {
            string filterNodeKey = string.Empty;
            if (rootFilterNode != null)
            {
                foreach (FilterNode f in rootFilterNode)
                {
                    filterNodeKey = filterNodeKey + f.Key.ToLower() + ":" + f.Value.Trim().ToLower() + ":" + f.Sign.ToString() + "-";
                }
            }

            return filterNodeKey;
        }
    }
}
