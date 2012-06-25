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
    using System.Data.Services;
    using System.Data.Services.Common;
    using Microsoft.Data.Services.Toolkit.Providers;
    using Microsoft.Samples.DPE.ODataTFS.Model.Helpers;

    [HasStream]
    [DataServiceKey("Id")]
    [ETag("AttachedTime")]
    [EntityPropertyMapping("AttachedTime", SyndicationItemProperty.Updated, SyndicationTextContentKind.Plaintext, true)]
    [EntityPropertyMapping("Name", SyndicationItemProperty.Title, SyndicationTextContentKind.Plaintext, true)]
    [EntityPropertyMapping("Comment", SyndicationItemProperty.Summary, SyndicationTextContentKind.Plaintext, true)]
    public class Attachment : IStreamEntity
    {
        public string Id { get; set; }

        public int WorkItemId { get; set; }

        public int Index { get; set; }

        public DateTime AttachedTime { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime LastWriteTime { get; set; }

        public string Name { get; set; }

        public string Extension { get; set; }

        public string Comment { get; set; }

        public long Length { get; set; }

        public string Uri { get; set; }

        public string GetContentTypeForStreaming()
        {
            return RegistryHelper.GetMimeType(this.Extension);
        }

        public string GetStreamETag()
        {
            return "\"" + this.AttachedTime + "\"";
        }

        public Uri GetUrlForStreaming()
        {
            return !string.IsNullOrWhiteSpace(this.Uri) ? new Uri(this.Uri, UriKind.RelativeOrAbsolute) : new Uri("http://temp-uri", UriKind.RelativeOrAbsolute);
        }
    }
}
