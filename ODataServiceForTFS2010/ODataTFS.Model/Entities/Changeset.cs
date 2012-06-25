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

    [DataServiceKey("Id")]
    [ETag("CreationDate")]
    [EntityPropertyMapping("CreationDate", SyndicationItemProperty.Updated, SyndicationTextContentKind.Plaintext, true)]
    [EntityPropertyMapping("ArtifactUri", SyndicationItemProperty.Title, SyndicationTextContentKind.Plaintext, true)]
    [EntityPropertyMapping("Comment", SyndicationItemProperty.Summary, SyndicationTextContentKind.Plaintext, true)]
    public class Changeset
    {
        public int Id { get; set; }

        public string ArtifactUri { get; set; }

        public string Comment { get; set; }

        public string Committer { get; set; }
        
        public DateTime CreationDate { get; set; }

        public string Owner { get; set; }

        public string Branch { get; set; }

        public string WebEditorUrl { get; set; }

        [ForeignProperty]
        public IEnumerable<Change> Changes { get; set; }

        [ForeignProperty]
        public IEnumerable<WorkItem> WorkItems { get; set; }
    }
}
