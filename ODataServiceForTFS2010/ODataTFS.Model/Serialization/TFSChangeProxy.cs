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
        
    public class TFSChangeProxy : TFSBaseProxy, ITFSChangeProxy
    {
        public TFSChangeProxy(Uri uri, ICredentials credentials)
            : base(uri, credentials)
        {
        }

        public IEnumerable<Change> GetChangesByChangeset(int changesetId, int topRequestValue)
        {
            var key = this.GetChangesByChangesetKey(changesetId.ToString());

            if (HttpContext.Current.Items[key] == null)
            {
                HttpContext.Current.Items[key] = this.RequestChangesByChangeset(changesetId, topRequestValue);
            }

            return (IEnumerable<Change>)HttpContext.Current.Items[key];
        }

        private IEnumerable<Change> RequestChangesByChangeset(int changesetId, int topRequestValue)
        {
            var versionControlServer = this.TfsConnection.GetService<TeamFoundation.VersionControl.Client.VersionControlServer>();

            return versionControlServer.GetChangesForChangeset(changesetId, false, topRequestValue, null)
                .Select(c => c.ToModel(this.TfsConnection.Name, changesetId)).ToArray();
        }      

        private string GetChangesByChangesetKey(string changeset)
        {
            return string.Format(CultureInfo.InvariantCulture, "TFSChangeProxy.GetChangesByChangeset_{0}", changeset);
        }
    }
}
