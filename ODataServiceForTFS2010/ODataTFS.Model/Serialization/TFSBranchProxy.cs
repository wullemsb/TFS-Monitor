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

    public class TFSBranchProxy : TFSBaseProxy, ITFSBranchProxy
    {
        public TFSBranchProxy(Uri uri, ICredentials credentials)
            : base(uri, credentials)
        {
        }

        public Branch GetBranch(string path)
        {
            var versionControlServer = this.TfsConnection.GetService<TeamFoundation.VersionControl.Client.VersionControlServer>();

            var identifier = new TeamFoundation.VersionControl.Client.ItemIdentifier(path);
            return versionControlServer.QueryBranchObjects(identifier, TeamFoundation.VersionControl.Client.RecursionType.None)
                                                .Where(b => !b.Properties.RootItem.IsDeleted)
                                                .Select(b => b.ToModel()).SingleOrDefault();
        }

        public IEnumerable<Branch> GetBranchesByProjectCollection()
        {
            var key = this.GetBranchesByProjectCollectionKey();

            if (HttpContext.Current.Items[key] == null)
            {
                HttpContext.Current.Items[key] = this.RequestBranchesByProjectCollection();
            }

            return (IEnumerable<Branch>)HttpContext.Current.Items[key];
        }

        public IEnumerable<Branch> GetBranchesByProject(string projectName)
        {
            var key = this.GetBranchesByProjectKey(projectName);

            if (HttpContext.Current.Items[key] == null)
            {
                HttpContext.Current.Items[key] = this.RequestBranchesByProject(projectName);
            }

            return (IEnumerable<Branch>)HttpContext.Current.Items[key];
        }

        private IEnumerable<Branch> RequestBranchesByProjectCollection()
        {
            var versionControlServer = this.TfsConnection.GetService<TeamFoundation.VersionControl.Client.VersionControlServer>();
            var rootObjects = versionControlServer.QueryRootBranchObjects(TeamFoundation.VersionControl.Client.RecursionType.None);

            return rootObjects.SelectMany(r => versionControlServer
                                                .QueryBranchObjects(r.Properties.RootItem, TeamFoundation.VersionControl.Client.RecursionType.Full)
                                                .Where(b => !b.Properties.RootItem.IsDeleted)
                                                .Select(b => b.ToModel())).ToArray();
        }

        private IEnumerable<Branch> RequestBranchesByProject(string projectName)
        {
            var versionControlServer = this.TfsConnection.GetService<TeamFoundation.VersionControl.Client.VersionControlServer>();
            var rootObjects = versionControlServer.QueryRootBranchObjects(TeamFoundation.VersionControl.Client.RecursionType.None);
            var projectPath = string.Format(CultureInfo.InvariantCulture, "$/{0}/", projectName);

            return rootObjects.SelectMany(r => versionControlServer
                                                .QueryBranchObjects(r.Properties.RootItem, TeamFoundation.VersionControl.Client.RecursionType.Full)
                                                .Where(b => !b.Properties.RootItem.IsDeleted && b.Properties.RootItem.Item.StartsWith(projectPath, StringComparison.OrdinalIgnoreCase))
                                                .Select(b => b.ToModel())).ToArray();
        }

        private string GetBranchesByProjectCollectionKey()
        {
            return "TFSBranchProxy.GetBranchesByProjectCollection";
        }

        private string GetBranchesByProjectKey(string projectName)
        {
            return string.Format(CultureInfo.InvariantCulture, "TFSBranchProxy.GetBranchesByProject_{0}", projectName);
        }
    }
}
