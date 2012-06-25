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
    using Microsoft.Samples.DPE.ODataTFS.Model.Entities;
    using Microsoft.Samples.DPE.ODataTFS.Model.Repositories;
    using Microsoft.Samples.DPE.ODataTFS.Model.Serialization;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class QueryRepositoryTests
    {
        [TestMethod]
        public void ItShouldGetOneQuery()
        {
            var mockProxy = new Mock<ITFSQueryProxy>();
            var queries = new List<Query>();
            var queryId = Guid.NewGuid();

            var expectedQuery = new Query { Id = queryId.ToString(), Description = "This is the first query", Name = "Query 1", Path = "$/sampleproject/Query 1", Project = "sampleproject", QueryText = "Query", QueryType = "Type", WorkItems = new List<WorkItem>() };

            mockProxy.Setup(p => p.GetQuery(queryId))
                 .Returns(expectedQuery)
                 .Verifiable();

            var repository = new QueryRepository(mockProxy.Object);

            var query = repository.GetOne(queryId.ToString());

            Assert.IsTrue(query != null);
            Assert.AreEqual(query.Id, queryId.ToString());
            Assert.AreEqual(query.Description, "This is the first query");
        }

        [TestMethod]
        public void ItShouldGetAllQuerysForAGivenCollection()
        {
            var mockProxy = new Mock<ITFSQueryProxy>();
            var queries = new List<Query>();

            queries.Add(new Query { Name = "Query 01", Description = "This is the first query" });
            queries.Add(new Query { Name = "Query 02", Description = "This is the second query" });

            mockProxy.Setup(p => p.GetQueriesByProjectCollection())
                 .Returns(queries)
                 .Verifiable();

            var repository = new QueryRepository(mockProxy.Object);
            var results = repository.GetAll();

            Assert.IsTrue(results.SequenceEqual<Query>(queries), "The expected queries for a collection don't match the results");
            mockProxy.VerifyAll();
        }

        [TestMethod]
        public void ItShouldGetAllQuerysForAGivenProject()
        {
            var mockProxy = new Mock<ITFSQueryProxy>();
            var queries = new List<Query>();

            queries.Add(new Query { Name = "Query 01", Description = "This is the first query", Project = "Project 1" });
            queries.Add(new Query { Name = "Query 02", Description = "This is the second query", Project = "Project 1" });

            mockProxy.Setup(p => p.GetQueriesByProject(It.Is<string>(s => s == "Project 1")))
                 .Returns(queries)
                 .Verifiable();

            var repository = new QueryRepository(mockProxy.Object);
            var results = repository.GetQueriesByProject("Project 1");

            Assert.IsTrue(results.SequenceEqual<Query>(queries), "The expected queries for a project don't match the results");
            mockProxy.VerifyAll();
        }
    }
}
