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

namespace Microsoft.Samples.DPE.ODataTFS.Model.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Samples.DPE.ODataTFS.Model.Entities;
    using Microsoft.Samples.DPE.ODataTFS.Model.Serialization;

    public class QueryRepository
    {
        private readonly ITFSQueryProxy proxy;

        public QueryRepository(ITFSQueryProxy proxy)
        {
            this.proxy = proxy;
        }

        public Query GetOne(string id)
        {
            Guid queryId;
            if (!Guid.TryParse(id, out queryId))
            {
                throw new ArgumentException("The parameter id should be a GUID", "id");
            }

            return this.proxy.GetQuery(queryId);
        }

        public IEnumerable<Query> GetQueriesByProject(string name)
        {
            return this.proxy.GetQueriesByProject(name);
        }

        public IEnumerable<Query> GetAll()
        {
            return this.proxy.GetQueriesByProjectCollection();
        }
    }
}
