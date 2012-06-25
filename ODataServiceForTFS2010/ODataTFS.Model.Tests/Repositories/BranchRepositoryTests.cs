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
    using System.Linq;
    using Microsoft.Samples.DPE.ODataTFS.Model.Entities;
    using Microsoft.Samples.DPE.ODataTFS.Model.Repositories;
    using Microsoft.Samples.DPE.ODataTFS.Model.Serialization;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class BranchPathRepositoryTests
    {
        [TestMethod]
        public void ItShouldGetOneBranch()
        {
            var mockProxy = new Mock<ITFSBranchProxy>();
            var branches = new List<Branch>();
            var expectedResult = new Branch { Path = "myproject>root", Description = "This is the trunk" };
            
            branches.Add(expectedResult);
            branches.Add(new Branch { Path = "myproject>branch1", Description = "This is one branch" });
            branches.Add(new Branch { Path = "myproject>branch2", Description = "This is another branch" });

            mockProxy.Setup(p => p.GetBranch(EntityTranslator.DecodePath("myproject>root")))
                 .Returns(expectedResult)
                 .Verifiable();

            var repository = new BranchRepository(mockProxy.Object);

            var result = repository.GetOne("myproject>root");

            Assert.IsTrue(result != null);
            Assert.AreEqual(result.Path, expectedResult.Path);
            Assert.AreEqual(result.Description, expectedResult.Description);
        }

        [TestMethod]
        public void ItShouldGetAllBranchesForAGivenCollection()
        {
            var mockProxy = new Mock<ITFSBranchProxy>();
            var branches = new List<Branch>();

            branches.Add(new Branch { Path = "myproject>root", Description = "This is the trunk" });
            branches.Add(new Branch { Path = "myproject>branch1", Description = "This is one branch" });
            branches.Add(new Branch { Path = "myproject>branch2", Description = "This is another branch" });

            mockProxy.Setup(p => p.GetBranchesByProjectCollection())
                 .Returns(branches)
                 .Verifiable();

            var repository = new BranchRepository(mockProxy.Object);

            var results = repository.GetAll();

            Assert.IsTrue(results.SequenceEqual<Branch>(branches), "The expected branches for a collection don't match the results");
            mockProxy.VerifyAll();
        }

        [TestMethod]
        public void ItShouldGetAllBranchesForAGivenProject()
        {
            var mockProxy = new Mock<ITFSBranchProxy>();
            var branches = new List<Branch>();

            branches.Add(new Branch { Path = "myproject>root", Description = "This is the trunk" });
            branches.Add(new Branch { Path = "myproject>branch1", Description = "This is one branch" });
            branches.Add(new Branch { Path = "myproject>branch2", Description = "This is another branch" });

            mockProxy.Setup(p => p.GetBranchesByProject("myproject"))
                 .Returns(branches)
                 .Verifiable();

            var repository = new BranchRepository(mockProxy.Object);

            var results = repository.GetBranchesByProject("myproject");

            Assert.IsTrue(results.SequenceEqual<Branch>(branches), "The expected branches for a project don't match the results");
            mockProxy.VerifyAll();
        }
    }
}
