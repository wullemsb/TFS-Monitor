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
    using System.Collections.Generic;
    using System.Data.Services;
    using System.Linq;
    using Microsoft.Data.Services.Toolkit.QueryModel;
    using Microsoft.Samples.DPE.ODataTFS.Model.Entities;
    using Microsoft.Samples.DPE.ODataTFS.Model.Repositories;
    using Microsoft.Samples.DPE.ODataTFS.Model.Serialization;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class ChangeRepositoryTests
    {
        [TestMethod]
        public void ItShouldGetOneChange()
        {
            var mockProxy = new Mock<ITFSChangeProxy>();
            var changes = new List<Change>();

            changes.Add(new Change { Changeset = 123,  Path = "root>path1", ChangeType = "rename" });
            changes.Add(new Change { Changeset = 123, Path = "root>path2", ChangeType = "delete" });

            mockProxy.Setup(p => p.GetChangesByChangeset(123, 1))
                 .Returns(changes)
                 .Verifiable();

            var repository = new ChangeRepository(mockProxy.Object);

            var change = repository.GetOne("123", "root>path1");

            Assert.IsTrue(change != null);
            Assert.AreEqual(change.Changeset, 123, 1);
            Assert.AreEqual(change.Path, "root>path1");
            Assert.AreEqual(change.ChangeType, "rename");
        }

        [TestMethod]
        public void ItShouldGetAllChangesForAGivenChangeset()
        {
            var mockProxy = new Mock<ITFSChangeProxy>();
            var changes = new List<Change>();

            changes.Add(new Change { Changeset = 123, Path = "root>path1", ChangeType = "rename" });
            changes.Add(new Change { Changeset = 123, Path = "root>path2", ChangeType = "delete" });

            mockProxy.Setup(p => p.GetChangesByChangeset(123, int.MaxValue))
                 .Returns(changes)
                 .Verifiable();

            var repository = new ChangeRepository(mockProxy.Object);
            var operation = new ODataSelectManyQueryOperation();
            operation.Keys = new Dictionary<string, string>();
            operation.Keys.Add("Changeset", "123");
            var results = repository.GetChangesByChangeset(operation);

            Assert.IsTrue(results.SequenceEqual<Change>(changes), "The expected changes for a collection don't match the results");
            mockProxy.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(DataServiceException))]
        public void ItShouldThrowExceptionOnGetAll()
        {
            var mockProxy = new Mock<ITFSChangeProxy>();
            var repository = new ChangeRepository(mockProxy.Object);

            var results = repository.GetAll();
        }
    }
}
