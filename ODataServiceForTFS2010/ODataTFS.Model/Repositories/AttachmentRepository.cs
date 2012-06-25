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
    using System.Collections;
    using System.Collections.Generic;
    using System.Data.Services;
    using System.Globalization;
    using System.Linq;
    using Microsoft.Data.Services.Toolkit.QueryModel;
    using Microsoft.Samples.DPE.ODataTFS.Model.Entities;
    using Microsoft.Samples.DPE.ODataTFS.Model.Serialization;

    public class AttachmentRepository : IWriteableRepository
    {
        private readonly ITFSAttachmentProxy proxy;

        public AttachmentRepository(ITFSAttachmentProxy proxy)
        {
            this.proxy = proxy;
        }

        public Attachment GetOne(string id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            if (id.StartsWith("temp-", StringComparison.OrdinalIgnoreCase))
            {
                return new Attachment { Id = id };
            }

            var ids = id.Split(new[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
            if (ids.Length != 2)
            {
                throw new ArgumentException("The id parameter must have the following pattern: 'workItemId-index'", "id");
            }

            var workItemId = 0;
            if (!int.TryParse(ids[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out workItemId))
            {
                throw new ArgumentException("The workItemId segment of the id parameter must be numeric", "id");
            }

            var index = 0;
            if (!int.TryParse(ids[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out index))
            {
                throw new ArgumentException("The index segment of the id parameter must be numeric", "id");
            }

            return this.proxy.GetAttachment(workItemId, index);
        }

        public IEnumerable<Attachment> GetAttachmentsByWorkItem(string workItemId)
        {
            var workItemIdNumber = 0;
            if (!int.TryParse(workItemId, NumberStyles.Integer, CultureInfo.InvariantCulture, out workItemIdNumber))
            {
                throw new ArgumentException("The workItemId parameter must be numeric", "workItemId");
            }

            return this.proxy.GetAttachmentsByWorkItem(workItemIdNumber);
        }

        public IEnumerable<Attachment> GetAll()
        {
            throw new DataServiceException(501, "Not Implemented", "The 'Attachment' collection cannot be enumerated as a root collection. It should depend on a WorkItem. (e.g. /WorkItems(12345)/Attachments)", "en-US", null);
        }

        public object CreateDefaultEntity()
        {
            return new Attachment
            {
                Id = string.Format(CultureInfo.InvariantCulture, "temp-{0}", DateTime.Now.Ticks)
            };
        }

        public void CreateRelation(object targetResource, object resourceToBeAdded)
        {
            throw new DataServiceException(501, "Not Implemented", "'CreateRelation' Operation in the Attachment Repository is not supported", "en-US", null);
        }

        public void Remove(object entity)
        {
            throw new DataServiceException(501, "Not Implemented", "'Remove' Operation in the Attachment Repository is not supported", "en-US", null);
        }

        public void Save(object entity)
        {
            var attachment = entity as Attachment;
            if (attachment == null)
            {
                attachment = (entity as IEnumerable).Cast<Attachment>().First();
            }

            if (attachment.WorkItemId <= 0)
            {
                return;
            }

            this.proxy.CreateAttachment(attachment);
        }
    }
}