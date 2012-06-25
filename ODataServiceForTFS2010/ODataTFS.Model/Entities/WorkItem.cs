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
    [ETag("ChangedDate")]
    [EntityPropertyMapping("ChangedDate", SyndicationItemProperty.Updated, SyndicationTextContentKind.Plaintext, true)]
    [EntityPropertyMapping("Title", SyndicationItemProperty.Title, SyndicationTextContentKind.Plaintext, true)]
    [EntityPropertyMapping("Description", SyndicationItemProperty.Summary, SyndicationTextContentKind.Plaintext, true)]
    public class WorkItem
    {
        public int Id { get; set; }

        public string AreaPath { get; set; }

        public string IterationPath { get; set; }

        public int Revision { get; set; }

        public string Priority { get; set; }

        public string Severity { get; set; }

        public string StackRank { get; set; }

        public string Project { get; set; }

        public string AssignedTo { get; set; }        

        public DateTime CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime ChangedDate { get; set; }

        public string ChangedBy { get; set; }

        public string ResolvedBy { get; set; }

        public string Title { get; set; }

        public string State { get; set; }

        public string Type { get; set; }

        public string Reason { get; set; }

        public string Description { get; set; }

        public string ReproSteps { get; set; }

        public string FoundInBuild { get; set; }

        public string IntegratedInBuild { get; set; }
        
        public string WebEditorUrl { get; set; }

        [ForeignProperty]
        public IEnumerable<Attachment> Attachments { get; set; }
    }
}
