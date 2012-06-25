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
    public class ProjectRepositoryTests
    {
        [TestMethod]
        public void ItShouldGetOneProject()
        {
            var mockProxy = new Mock<ITFSProjectProxy>();
            var projects = new List<Project>();

            projects.Add(new Project { Name = "Project 01", Changesets = new List<Changeset>(), WorkItems = new List<WorkItem>(), Queries = new List<Query>(), Builds = new List<Build>() });
            projects.Add(new Project { Name = "Project 02", Changesets = new List<Changeset>(), WorkItems = new List<WorkItem>(), Queries = new List<Query>(), Builds = new List<Build>() });

            mockProxy.Setup(p => p.GetProjectsByProjectCollection())
                 .Returns(projects)
                 .Verifiable();

            var repository = new ProjectRepository(mockProxy.Object);

            var project = repository.GetOne("Project 01");

            Assert.IsTrue(project != null);
            Assert.AreEqual(project.Name, "Project 01");
            mockProxy.VerifyAll();
        }

        [TestMethod]
        public void ItShouldGetAllProjectsForAGivenCollection()
        {
            var mockProxy = new Mock<ITFSProjectProxy>();
            var projects = new List<Project>();

            projects.Add(new Project { Name = "Project 01" });
            projects.Add(new Project { Name = "Project 02" });

            mockProxy.Setup(p => p.GetProjectsByProjectCollection())
                 .Returns(projects)
                 .Verifiable();

            var repository = new ProjectRepository(mockProxy.Object);
            var results = repository.GetAll();

            Assert.IsTrue(results != null);
            Assert.IsTrue(results.SequenceEqual<Project>(projects), "The expected projects for a given collection don't much the results");
        }
    }
}
