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
    public class ChangesetRepositoryTests
    {
        [TestMethod]
        public void ItShouldGetOneChangeset()
        {
            var mockProxy = new Mock<ITFSChangesetProxy>();

            var expectedResult = new Changeset
            {
                Id = 1,
                Comment = "Checking in one file",
                Owner = @"Domain\User",
                Committer = @"Domain\User",
                CreationDate = DateTime.Now,
                ArtifactUri = "http://sampleuri/",
                Branch = "$/sampleproject/branch",
                Changes = new List<Change>(),
                WorkItems = new List<WorkItem>()                
            };

            mockProxy.Setup(p => p.GetChangeset(
                It.Is<int>(i => i == 1)))
                    .Returns(expectedResult)
                    .Verifiable();

            var repository = new ChangesetRepository(mockProxy.Object);

            var result = repository.GetOne("1");

            Assert.AreEqual(result, expectedResult);
            mockProxy.VerifyAll();
        }

        [TestMethod]
        public void ItShouldGetAllChangesetsForAGivenProject()
        {
            var mockProxy = new Mock<ITFSChangesetProxy>();
            var changesets = new List<Changeset>();

            changesets.Add(new Changeset { Id = 1, Comment = "Checking in one file" });
            changesets.Add(new Changeset { Id = 2, Comment = "Merging a branch" });

            mockProxy.Setup(p => p.GetChangesetsByProject(It.Is<string>(s => s.Equals("Sample Project")), It.IsAny<FilterNode>(), It.IsAny<int>()))
                 .Returns(changesets)
                 .Verifiable();

            var repository = new ChangesetRepository(mockProxy.Object);
            var operation = new ODataSelectManyQueryOperation();
            operation.Keys = new Dictionary<string, string>();
            operation.Keys.Add("Project", "Sample Project");
            var results = repository.GetChangesetsByProject(operation);

            Assert.IsTrue(results.SequenceEqual<Changeset>(changesets), "The expected changesets for a project don't match the results");
            mockProxy.VerifyAll();
        }

        [TestMethod]
        public void ItShouldGetAllChangesetsForAGivenCollection()
        {
            var mockProxy = new Mock<ITFSChangesetProxy>();
            var changesets = new List<Changeset>();

            changesets.Add(new Changeset { Id = 1, Comment = "Checking in one file in Project 1" });
            changesets.Add(new Changeset { Id = 2, Comment = "Merging a branch in Project 1" });

            mockProxy.Setup(p => p.GetChangesetsByProjectCollection(It.IsAny<FilterNode>(), It.IsAny<int>()))
                .Returns(changesets)
                .Verifiable();

            var repository = new ChangesetRepository(mockProxy.Object);

            var results = repository.GetAll(new ODataSelectManyQueryOperation());

            Assert.IsTrue(results.SequenceEqual<Changeset>(changesets), "The expected changesets for a collection don't match the results");
        }
    }
}
