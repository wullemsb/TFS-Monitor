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
    using System.Data.Services;
    using System.Globalization;
    using System.Linq;
    using Microsoft.Data.Services.Toolkit.QueryModel;
    using Microsoft.Samples.DPE.ODataTFS.Model.Entities;
    using Microsoft.Samples.DPE.ODataTFS.Model.ExpressionVisitors;
    using Microsoft.Samples.DPE.ODataTFS.Model.Serialization;

    public class ChangeRepository
    {
        private readonly ITFSChangeProxy proxy;

        public ChangeRepository(ITFSChangeProxy proxy)
        {
            this.proxy = proxy;
        }

        public Change GetOne(string changeset, string path)
        {
            var changesetId = 0;
            if (!int.TryParse(changeset, NumberStyles.Integer, CultureInfo.InvariantCulture, out changesetId))
            {
                throw new ArgumentException("the changeset parameter must be numeric", "changeset");
            }

            return this.proxy.GetChangesByChangeset(changesetId, 1).SingleOrDefault(c => c.Path.Equals(path, StringComparison.OrdinalIgnoreCase));
        }

        [RepositoryBehavior(HandlesTop = true)]
        public IEnumerable<Change> GetChangesByChangeset(ODataSelectManyQueryOperation operation)
        {
            var changesetId = 0;
            if (!int.TryParse(operation.Key, NumberStyles.Integer, CultureInfo.InvariantCulture, out changesetId))
            {
                throw new ArgumentException("The id parameter must be numeric", "id");
            }

            int topRequestValue = this.GetTopRequestValue(operation);
     
            return this.proxy.GetChangesByChangeset(changesetId, topRequestValue);  
        }

        public IEnumerable<Change> GetAll()
        {
            throw new DataServiceException(501, "Not Implemented", "The 'Change' collection cannot be enumerated as a root collection. It should depend on a Changeset. (e.g. /Changesets('12345')/Changes)", "en-US", null);
        }

        /// <summary>
        /// Gets the count treshold to minimize the number of elements retrieved from TFS
        /// </summary>
        private int GetTopRequestValue(ODataQueryOperation operation)
        {
            int topRequestValue = int.MaxValue;

            if (operation.IsCountRequest)
            {
                if (!string.IsNullOrEmpty(operation.ContinuationToken))
                {
                    var parts = operation.ContinuationToken.Split(':').Select(int.Parse);
                    topRequestValue = parts.First() + parts.Last() + 1;
                }
            }
            else
            {
                if (operation.TopCount > 0)
                {
                    topRequestValue = operation.TopCount + operation.SkipCount + 1;
                }
            }

            return topRequestValue;
        }
    }
}