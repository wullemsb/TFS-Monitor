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
    using System.Linq;
    using System.Net;
    using System.Web;
    using Microsoft.Samples.DPE.ODataTFS.Model.Entities;
    using Microsoft.TeamFoundation.Server;

    public class TFSProjectProxy : TFSBaseProxy, ITFSProjectProxy
    {
        public TFSProjectProxy(Uri uri, ICredentials credentials)
            : base(uri, credentials)
        {
        }

        public IEnumerable<Project> GetProjectsByProjectCollection()
        {
            if (HttpContext.Current.Items[this.GetProjectsByProjectCollectionHashKey()] == null)
            {
                HttpContext.Current.Items[this.GetProjectsByProjectCollectionHashKey()] = this.RequestProjectsByProjectCollection();
            }

            return (IEnumerable<Project>)HttpContext.Current.Items[this.GetProjectsByProjectCollectionHashKey()];
        }

        private IEnumerable<Project> RequestProjectsByProjectCollection()
        {
            var css = this.TfsConnection.GetService<ICommonStructureService3>();
            var teamProjects = css.ListAllProjects();
            return teamProjects.Select(p => p.ToModel(this.TfsConnection.Name));
        }             

        private string GetProjectsByProjectCollectionHashKey()
        {
            return "TFSProjectProxy.GetProjectsByProjectCollection";
        }
    }
}
