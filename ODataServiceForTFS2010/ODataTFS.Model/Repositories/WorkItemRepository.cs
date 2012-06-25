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
    using Microsoft.Samples.DPE.ODataTFS.Model.ExpressionVisitors;
    using Microsoft.Samples.DPE.ODataTFS.Model.Serialization;

    public class WorkItemRepository : IWriteableRepository
    {
        private readonly ITFSWorkItemProxy proxy;

        public WorkItemRepository(ITFSWorkItemProxy proxy)
        {
            this.proxy = proxy;
        }

        public WorkItem GetOne(string id)
        {
            var workItemId = 0;
            if (!int.TryParse(id, NumberStyles.Integer, CultureInfo.InvariantCulture, out workItemId))
            {
                throw new ArgumentException("The id parameter must be numeric", "id");
            }

            return this.proxy.GetWorkItem(workItemId);
        }

        [RepositoryBehavior(HandlesFilter = true)]
        public IEnumerable<WorkItem> GetWorkItemsByProject(ODataSelectManyQueryOperation operation)
        {
            if (operation == null)
            {
                throw new ArgumentNullException("operation");
            }

            var parameters = new WorkItemFilterExpressionVisitor(operation.FilterExpression).Eval();
            return this.proxy.GetWorkItemsByProject(operation.Key, parameters);
        }

        public IEnumerable<WorkItem> GetWorkItemsByQuery(string id)
        {
            var queryId = default(Guid);
            if (!Guid.TryParse(id, out queryId))
            {
                throw new ArgumentException("The id paramter must be a GUID", "id");
            }

            return this.proxy.GetWorkItemsByQuery(queryId);
        }

        public IEnumerable<WorkItem> GetWorkItemsByChangeset(string id)
        {
            var changesetId = 0;
            if (!int.TryParse(id, NumberStyles.Integer, CultureInfo.InvariantCulture, out changesetId))
            {
                throw new ArgumentException("The id parameter must be numeric", "id");
            }

            return this.proxy.GetWorkItemsByChangeset(changesetId);
        }

        [RepositoryBehavior(HandlesFilter = true)]
        public IEnumerable<WorkItem> GetWorkItemsByBuild(ODataSelectManyQueryOperation operation)
        {
            if (operation == null)
            {
                throw new ArgumentNullException("operation");
            }

            var parameters = new WorkItemFilterExpressionVisitor(operation.FilterExpression).Eval();
            return this.proxy.GetWorkItemsByBuild(operation.Keys["project"], operation.Keys["number"], parameters);
        }

        [RepositoryBehavior(HandlesFilter = true)]
        public IEnumerable<WorkItem> GetAll(ODataQueryOperation operation)
        {
            if (operation == null)
            {
                throw new ArgumentNullException("operation");
            }

            var parameters = new WorkItemFilterExpressionVisitor(operation.FilterExpression).Eval();
            return this.proxy.GetWorkItemsByProjectCollection(parameters);
        }

        public object CreateDefaultEntity()
        {
            return new WorkItem();
        }

        public void CreateRelation(object targetResource, object resourceToBeAdded)
        {
            throw new DataServiceException(501, "Not Implemented", "'CreateRelation' Operation in the WorkItem Repository is not supported", "en-US", null);
        }

        public void Remove(object entity)
        {
            throw new DataServiceException(501, "Not Implemented", "The WorkItems cannot be deleted", "en-US", null);
        }

        public void Save(object entity)
        {
            var workItem = entity as WorkItem;
            if (workItem == null)
            {
                workItem = (entity as IEnumerable).Cast<WorkItem>().First();
            }

            if (workItem.Id > 0)
            {
                this.proxy.UpdateWorkItem(workItem);
            }
            else
            {
                this.proxy.CreateWorkItem(workItem);
            }
        }
    }
}