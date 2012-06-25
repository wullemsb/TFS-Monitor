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
    public class AreaPathRepositoryTests
    {
        [TestMethod]
        public void ItShouldGetOneAreaPath()
        {
            var mockProxy = new Mock<ITFSAreaPathProxy>();
            var areas = new List<AreaPath>();

            areas.Add(new AreaPath { Name = "Area 1", Path = "myproject\\area1" });
            areas.Add(new AreaPath { Name = "Area 2", Path = "myproject\\area2" });

            mockProxy.Setup(p => p.GetAllAreaPaths())
                 .Returns(areas)
                 .Verifiable();

            var repository = new AreaPathRepository(mockProxy.Object);

            var area = repository.GetOne("myproject\\area1");

            Assert.IsTrue(area != null);
            Assert.AreEqual(area.Name, "Area 1");
            Assert.AreEqual(area.Path, "myproject\\area1");
        }

        [TestMethod]
        public void ItShouldGetAllAreasForAGivenCollection()
        {
            var mockProxy = new Mock<ITFSAreaPathProxy>();
            var areas = new List<AreaPath>();

            areas.Add(new AreaPath { Name = "Area 1", Path = "myproject\\area1" });
            areas.Add(new AreaPath { Name = "Area 2", Path = "myproject\\area2" });

            mockProxy.Setup(p => p.GetAllAreaPaths())
                 .Returns(areas)
                 .Verifiable();

            var repository = new AreaPathRepository(mockProxy.Object);

            var results = repository.GetAll();

            Assert.IsTrue(results.SequenceEqual<AreaPath>(areas), "The expected areas for a collection don't match the results");
            mockProxy.VerifyAll();
        }

        [TestMethod]
        public void ItShouldGetAllAreasForAGivenProject()
        {
            var mockProxy = new Mock<ITFSAreaPathProxy>();
            var areas = new List<AreaPath>();

            areas.Add(new AreaPath { Name = "Area 1", Path = "myproject\\area1" });
            areas.Add(new AreaPath { Name = "Area 2", Path = "myproject\\area2" });

            mockProxy.Setup(p => p.GetAreaPathsByProject(It.Is<string>(s => s == "Project 1")))
                 .Returns(areas)
                 .Verifiable();

            var repository = new AreaPathRepository(mockProxy.Object);

            var results = repository.GetAreaPathsByProject("Project 1");

            Assert.IsTrue(results.SequenceEqual<AreaPath>(areas), "The expected areas for a project don't match the results");
            mockProxy.VerifyAll();
        }
    }
}
