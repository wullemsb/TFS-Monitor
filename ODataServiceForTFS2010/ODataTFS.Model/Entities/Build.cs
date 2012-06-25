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

    [DataServiceKey("Project", "Definition", "Number")]
    [ETag("LastChangedOn")]
    [EntityPropertyMapping("LastChangedOn", SyndicationItemProperty.Updated, SyndicationTextContentKind.Plaintext, true)]
    [EntityPropertyMapping("Number", SyndicationItemProperty.Title, SyndicationTextContentKind.Plaintext, true)]
    [EntityPropertyMapping("Status", SyndicationItemProperty.Summary, SyndicationTextContentKind.Plaintext, true)]
    public class Build
    {
        // Represents the Project Name
        public string Project { get; set; }

        // Represents the build definition name
        public string Definition { get; set; }

        public string Number { get; set; }

        public string Reason { get; set; }

        public string Quality { get; set; }

        public string Status { get; set; }

        public string RequestedBy { get; set; }

        public string RequestedFor { get; set; }

        public string LastChangedBy { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime FinishTime { get; set; }

        public DateTime LastChangedOn { get; set; }

        public bool BuildFinished { get; set; }

        public string DropLocation { get; set; }

        public string Errors { get; set; }

        public string Warnings { get; set; }

        [ForeignProperty]
        public IEnumerable<WorkItem> WorkItems { get; set; }

        [ForeignProperty]
        public IEnumerable<Changeset> Changesets { get; set; }
    }
}
