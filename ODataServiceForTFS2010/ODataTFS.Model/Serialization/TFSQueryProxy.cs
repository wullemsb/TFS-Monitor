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
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Web;
    using Microsoft.Samples.DPE.ODataTFS.Model.Entities;
    using Microsoft.TeamFoundation.Server;

    public class TFSQueryProxy : TFSBaseProxy, ITFSQueryProxy
    {
        public TFSQueryProxy(Uri uri, ICredentials credentials) : base(uri, credentials)
        {
        }

        public Query GetQuery(Guid id)
        {
            var workItemServer = this.TfsConnection.GetService<TeamFoundation.WorkItemTracking.Client.WorkItemStore>();
            string projectName;
            try
            {
                var query = workItemServer.GetQueryDefinition(id);
                projectName = query.Project.Name;
            }
            catch (ArgumentException)
            {
                // Weird bug in the TFS API, queries stored in TFS2008 cannot be accessed
                // using the new 2010 API, must use the old deprecated method.
#pragma warning disable
                var query = workItemServer.GetStoredQuery(id);
                projectName = query.Project.Name;
#pragma warning restore
            }

            return GetQueriesInHierarchy(workItemServer.Projects[projectName].QueryHierarchy, projectName)
                        .SingleOrDefault(q => q.Id.Equals(id.ToString(), StringComparison.OrdinalIgnoreCase));
        }

        public IEnumerable<Query> GetQueriesByProjectCollection()
        {
            if (HttpContext.Current.Items[this.GetQueriesByProjectCollectionKey()] == null)
            {
                HttpContext.Current.Items[this.GetQueriesByProjectCollectionKey()] = this.RequestQueriesByProjectCollection();
            }

            return (IEnumerable<Query>)HttpContext.Current.Items[this.GetQueriesByProjectCollectionKey()];
        }

        public IEnumerable<Query> GetQueriesByProject(string projectName)
        {
            if (HttpContext.Current.Items[this.GetQueriesByProjectKey(projectName)] == null)
            {
                HttpContext.Current.Items[this.GetQueriesByProjectKey(projectName)] = this.RequestQueriesByProject(projectName);
            }

            return (IEnumerable<Query>)HttpContext.Current.Items[this.GetQueriesByProjectKey(projectName)];
        }

        private static List<Query> GetQueriesInHierarchy(IEnumerable<TeamFoundation.WorkItemTracking.Client.QueryItem> queries, string path = "")
        {
            var queryItems = new List<Query>();
            foreach (var queryItem in queries)
            {
                var queryPath = string.Format(CultureInfo.InvariantCulture, "{0}/{1}", path, queryItem.Name);
                if (queryItem is TeamFoundation.WorkItemTracking.Client.QueryFolder)
                {
                    queryItems.AddRange(GetQueriesInHierarchy(queryItem as TeamFoundation.WorkItemTracking.Client.QueryFolder, queryPath));
                }
                else
                {
                    queryItems.Add((queryItem as TeamFoundation.WorkItemTracking.Client.QueryDefinition).ToModel(queryPath));
                }
            }

            return queryItems;
        }

        private IEnumerable<Query> RequestQueriesByProjectCollection()
        {
            var workItemServer = this.TfsConnection.GetService<TeamFoundation.WorkItemTracking.Client.WorkItemStore>();
            return workItemServer.Projects
                                 .Cast<TeamFoundation.WorkItemTracking.Client.Project>().Where(p => p.HasWorkItemReadRights)
                                 .SelectMany(p => GetQueriesInHierarchy(p.QueryHierarchy, p.Name));
        }

        private IEnumerable<Query> RequestQueriesByProject(string projectName)
        {
            var workItemServer = this.TfsConnection.GetService<TeamFoundation.WorkItemTracking.Client.WorkItemStore>();

            return GetQueriesInHierarchy(workItemServer.Projects[projectName].QueryHierarchy, projectName);
        }

        private string GetQueriesByProjectCollectionKey()
        {
            return "TFSQueryProxy.GetQueriesByProjectCollection";
        }

        private string GetQueriesByProjectKey(string projectName)
        {
            return string.Format(CultureInfo.InvariantCulture, "TFSQueryProxy.GetQueriesByProject_{0}", projectName);
        }
    }
}
