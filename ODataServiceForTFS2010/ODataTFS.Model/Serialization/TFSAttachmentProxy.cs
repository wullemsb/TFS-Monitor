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
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Web;
    using Microsoft.Samples.DPE.ODataTFS.Model.Entities;

    public class TFSAttachmentProxy : TFSBaseProxy, ITFSAttachmentProxy
    {
        public TFSAttachmentProxy(Uri uri, ICredentials credentials)
            : base(uri, credentials)
        {
        }

        public Attachment GetAttachment(int workItemId, int index)
        {
            var wiql = string.Format(CultureInfo.InvariantCulture, "SELECT [System.Id] FROM WorkItems WHERE [System.Id] = {0}", workItemId);
            var workItem = this.QueryWorkItems(wiql)
                                            .Cast<TeamFoundation.WorkItemTracking.Client.WorkItem>()
                                            .FirstOrDefault();

            if ((workItem != null) && (workItem.Attachments.Count > index))
            {
                return workItem.Attachments
                                .Cast<TeamFoundation.WorkItemTracking.Client.Attachment>()
                                .OrderBy(a => a.AttachedTimeUtc)
                                .ElementAt(index)
                                .ToModel(workItemId, index);
            }

            return null;
        }

        public IEnumerable<Attachment> GetAttachmentsByWorkItem(int workItemId)
        {
            if (HttpContext.Current.Items[this.GetAttachmentsByWorkItemKey(workItemId)] == null)
            {
                HttpContext.Current.Items[this.GetAttachmentsByWorkItemKey(workItemId)] = this.RequestAttachmentsByWorkItemId(workItemId);
            }

            return (IEnumerable<Attachment>)HttpContext.Current.Items[this.GetAttachmentsByWorkItemKey(workItemId)];
        }

        public void CreateAttachment(Attachment attachment)
        {
            if (attachment == null)
            {
                throw new ArgumentNullException("attachment");
            }

            var wiql = string.Format(CultureInfo.InvariantCulture, "SELECT [System.Id] FROM WorkItems WHERE [System.Id] = {0}", attachment.WorkItemId);

            var workItem = this.QueryWorkItems(wiql)
                               .Cast<TeamFoundation.WorkItemTracking.Client.WorkItem>()
                               .FirstOrDefault();

            if (workItem == null)
            {
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "There is not any Work Item with an Id equals to {0}", attachment.WorkItemId), "attachment.WorkItemId");
            }

            var path = Path.Combine(Path.GetTempPath(), attachment.Id);
            var fileInfo = new FileInfo(path);

            if (!string.IsNullOrWhiteSpace(attachment.Name))
            {
                var fileName = string.IsNullOrWhiteSpace(attachment.Extension)
                                ? attachment.Name
                                : attachment.Extension.StartsWith(".", StringComparison.OrdinalIgnoreCase)
                                    ? string.Format(CultureInfo.InvariantCulture, "{0}{1}", attachment.Name, attachment.Extension)
                                    : string.Format(CultureInfo.InvariantCulture, "{0}.{1}", attachment.Name, attachment.Extension);

                path = Path.Combine(Path.GetTempPath(), fileName);

                if (File.Exists(path))
                {
                    File.Delete(path);
                }

                fileInfo.MoveTo(path);
            }

            workItem.Attachments.Add(attachment.ToEntity(path));
            workItem.Save();

            fileInfo.Delete();
        }

        private IEnumerable<Attachment> RequestAttachmentsByWorkItemId(int workItemId)
        {
            var wiql = string.Format(CultureInfo.InvariantCulture, "SELECT [System.Id] FROM WorkItems WHERE [System.Id] = {0}", workItemId);
            var workItem = this.QueryWorkItems(wiql)
                                            .Cast<TeamFoundation.WorkItemTracking.Client.WorkItem>()
                                            .FirstOrDefault();
            var index = 0;
            return workItem.Attachments
                            .Cast<TeamFoundation.WorkItemTracking.Client.Attachment>()
                            .OrderBy(a => a.AttachedTimeUtc)
                            .Select(a => a.ToModel(workItemId, index++)).ToArray();
        }

        private string GetAttachmentsByWorkItemKey(int workItemId)
        {
            return string.Format(CultureInfo.InvariantCulture, "TFSAttachmentProxy.GetAttachmentsByWorkItem_{0}", workItemId);
        }
    }
}