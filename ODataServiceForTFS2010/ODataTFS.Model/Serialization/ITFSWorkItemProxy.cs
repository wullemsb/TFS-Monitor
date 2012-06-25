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
    using Microsoft.Samples.DPE.ODataTFS.Model.Entities;
    using Microsoft.Samples.DPE.ODataTFS.Model.ExpressionVisitors;

    public interface ITFSWorkItemProxy
    {
        WorkItem GetWorkItem(int workItemId);

        IEnumerable<WorkItem> GetWorkItemsByProjectCollection(FilterNode rootFilterNode);

        IEnumerable<WorkItem> GetWorkItemsByProject(string projectName, FilterNode rootFilterNode);

        IEnumerable<WorkItem> GetWorkItemsByQuery(Guid queryId);

        IEnumerable<WorkItem> GetWorkItemsByBuild(string projectName, string buildNumber, FilterNode rootFilterNode);

        IEnumerable<WorkItem> GetWorkItemsByChangeset(int changesetId);

        void CreateWorkItem(WorkItem workItem);

        void UpdateWorkItem(WorkItem workItem);
    }
}
