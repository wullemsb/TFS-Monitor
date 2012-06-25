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

namespace Microsoft.Samples.DPE.ODataTFS.Model.Tests.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Data.Services.Toolkit.QueryModel;
    using Microsoft.Samples.DPE.ODataTFS.Model.Entities;
    using Microsoft.Samples.DPE.ODataTFS.Model.ExpressionVisitors;
    using Microsoft.Samples.DPE.ODataTFS.Model.Repositories;
    using Microsoft.Samples.DPE.ODataTFS.Model.Serialization;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class WorkItemRepositoryTests
    {
        [TestMethod]
        public void ItShouldGetOneWorkItem()
        {
            var mockProxy = new Mock<ITFSWorkItemProxy>();

            var item = new WorkItem { Id = 1, Description = "This is the first WorkItem", CreatedBy = "johndoe", Priority = "1", Title = "Bug #1" };

            mockProxy.Setup(p => p.GetWorkItem(1))
                 .Returns(item)
                 .Verifiable();

            var repository = new WorkItemRepository(mockProxy.Object);

            var result = repository.GetOne("1");

            Assert.IsTrue(result != null);
            Assert.AreEqual(result, item);
        }

        [TestMethod]
        public void ItShouldGetAllWorkItemsForAGivenCollection()
        {
            var mockProxy = new Mock<ITFSWorkItemProxy>();
            var items = new List<WorkItem>();

            items.Add(new WorkItem { Id = 1, Description = "This is the first WorkItem", CreatedBy = "johndoe", Priority = "1", Title = "Bug #1" });
            items.Add(new WorkItem { Id = 1, Description = "This is the second WorkItem", CreatedBy = "mary", Priority = "5", Title = "Bug #2" });

            mockProxy.Setup(p => p.GetWorkItemsByProjectCollection(It.IsAny<FilterNode>()))
                 .Returns(items)
                 .Verifiable();

            var repository = new WorkItemRepository(mockProxy.Object);
            var results = repository.GetAll(new ODataSelectManyQueryOperation());

            Assert.IsTrue(results.SequenceEqual<WorkItem>(items), "The expected queries for a collection don't match the results");
            mockProxy.VerifyAll();
        }

        [TestMethod]
        public void ItShouldGetAllWorkItemsForAGivenBuild()
        {
            var mockProxy = new Mock<ITFSWorkItemProxy>();
            var items = new List<WorkItem>();

            items.Add(new WorkItem { Id = 1, Description = "This is the first WorkItem", CreatedBy = "johndoe", Priority = "1", Title = "Bug #1" });
            items.Add(new WorkItem { Id = 1, Description = "This is the second WorkItem", CreatedBy = "mary", Priority = "5", Title = "Bug #2" });

            mockProxy.Setup(p => p.GetWorkItemsByBuild("Sample Project", "123", It.IsAny<FilterNode>()))
                 .Returns(items)
                 .Verifiable();

            var repository = new WorkItemRepository(mockProxy.Object);
            var operation = new ODataSelectManyQueryOperation();
            operation.Keys = new Dictionary<string, string>();
            operation.Keys.Add("project", "Sample Project");
            operation.Keys.Add("number", "123");

            var results = repository.GetWorkItemsByBuild(operation);

            Assert.IsTrue(results.SequenceEqual<WorkItem>(items), "The expected workitems for a build don't match the results");
            mockProxy.VerifyAll();
        }

        [TestMethod]
        public void ItShouldGetAllWorkItemsForAGivenChangeset()
        {
            var mockProxy = new Mock<ITFSWorkItemProxy>();
            var items = new List<WorkItem>();

            items.Add(new WorkItem { Id = 1, Description = "This is the first WorkItem", CreatedBy = "johndoe", Priority = "1", Title = "Bug #1" });
            items.Add(new WorkItem { Id = 1, Description = "This is the second WorkItem", CreatedBy = "mary", Priority = "5", Title = "Bug #2" });

            mockProxy.Setup(p => p.GetWorkItemsByChangeset(123))
                 .Returns(items)
                 .Verifiable();

            var repository = new WorkItemRepository(mockProxy.Object);

            var results = repository.GetWorkItemsByChangeset("123");

            Assert.IsTrue(results.SequenceEqual<WorkItem>(items), "The expected workitems for a changeset don't match the results");
            mockProxy.VerifyAll();
        }

        [TestMethod]
        public void ItShouldGetAllWorkItemsForAGivenProject()
        {
            var mockProxy = new Mock<ITFSWorkItemProxy>();
            var items = new List<WorkItem>();

            items.Add(new WorkItem { Id = 1, Description = "This is the first WorkItem", CreatedBy = "johndoe", Priority = "1", Title = "Bug #1" });
            items.Add(new WorkItem { Id = 1, Description = "This is the second WorkItem", CreatedBy = "mary", Priority = "5", Title = "Bug #2" });

            mockProxy.Setup(p => p.GetWorkItemsByProject("myproject", It.IsAny<FilterNode>()))
                 .Returns(items)
                 .Verifiable();

            var repository = new WorkItemRepository(mockProxy.Object);
            var operation = new ODataSelectManyQueryOperation();
            operation.Keys = new Dictionary<string, string>();
            operation.Keys.Add("project", "myproject");

            var results = repository.GetWorkItemsByProject(operation);

            Assert.IsTrue(results.SequenceEqual<WorkItem>(items), "The expected workitems for a changeset don't match the results");
            mockProxy.VerifyAll();
        }
    }
}
