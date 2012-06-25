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
    using System;
    using System.Collections.Generic;
    using System.Data.Services;
    using System.Data.Services.Common;
    using Microsoft.Data.Services.Toolkit.QueryModel;

    [DataServiceKey("Path")]
    [ETag("DateCreated")]
    [EntityPropertyMapping("DateCreated", SyndicationItemProperty.Updated, SyndicationTextContentKind.Plaintext, true)]
    [EntityPropertyMapping("Path", SyndicationItemProperty.Title, SyndicationTextContentKind.Plaintext, true)]
    [EntityPropertyMapping("Description", SyndicationItemProperty.Summary, SyndicationTextContentKind.Plaintext, true)]
    public class Branch
    {
        public string Path { get; set; }

        public string Description { get; set; }

        public DateTime DateCreated { get; set; }

        [ForeignProperty]
        public IEnumerable<Changeset> Changesets { get; set; }
    }
}
