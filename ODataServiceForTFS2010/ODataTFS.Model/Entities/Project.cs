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

namespace Microsoft.Samples.DPE.ODataTFS.Model.Entities
{
    using System.Collections.Generic;
    using System.Data.Services.Common;
    using Microsoft.Data.Services.Toolkit.QueryModel;

    [DataServiceKey("Name")]
    [EntityPropertyMapping("Name", SyndicationItemProperty.Title, SyndicationTextContentKind.Plaintext, true)]
    public class Project
    {
        public string Name { get; set; }

        // Team Project Collection Name
        public string Collection { get; set; }

        [ForeignProperty]
        public IEnumerable<Changeset> Changesets { get; set; }

        [ForeignProperty]
        public IEnumerable<Build> Builds { get; set; }

        [ForeignProperty]
        public IEnumerable<BuildDefinition> BuildDefinitions { get; set; }

        [ForeignProperty]
        public IEnumerable<WorkItem> WorkItems { get; set; }

        [ForeignProperty]
        public IEnumerable<Query> Queries { get; set; }

        [ForeignProperty]
        public IEnumerable<Branch> Branches { get; set; }

        [ForeignProperty]
        public IEnumerable<AreaPath> AreaPaths { get; set; }
    }
}
