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
    public class BuildRepositoryTests
    {
        [TestMethod]
        public void ItShouldGetOneBuild()
        {
            var mockProxy = new Mock<ITFSBuildProxy>();

            var expectedResult = new Build
            {
                Definition = "Full Build",
                Number = "FB Build 01",
                Project = "SampleProject",
                Status = "Failed",
                Quality = "Quality",
                BuildFinished = true,
                DropLocation = @"\\server\drops\SampleDrop1",
                Reason = "Reason",
                Errors = "Error description",
                Warnings = "Warning description",
                RequestedBy = @"Domain\User",
                RequestedFor = @"Domain\User",
                LastChangedBy = @"Domain\User",
                StartTime = DateTime.Now,
                FinishTime = DateTime.Now,
                LastChangedOn = DateTime.Now,
                WorkItems = new List<WorkItem>(),
                Changesets = new List<Changeset>()
            };

            mockProxy.Setup(p => p.GetBuild(
                It.Is<string>(s => s == "SampleProject"), 
                It.Is<string>(s => s == "Full Build"), 
                It.Is<string>(s => s == "FB Build 01")))
                    .Returns(expectedResult)
                    .Verifiable();

            var repository = new BuildRepository(mockProxy.Object);

            var result = repository.GetOne("SampleProject", "Full Build", "FB Build 01");

            Assert.AreEqual(result, expectedResult);
            mockProxy.VerifyAll();
        }

        [TestMethod]
        public void ItShouldGetAllBuildsForAGivenProject()
        {
            var mockProxy = new Mock<ITFSBuildProxy>();
            var builds = new List<Build>();

            builds.Add(new Build { Definition = "Full Build", Number = "FB Build 01", Project = "SampleProject" });
            builds.Add(new Build { Definition = "Custom Build", Number = "CB Build 01", Project = "SampleProject" });

            mockProxy.Setup(p => p.GetBuildsByProject(It.Is<string>(s => s.Equals("Sample Project")), It.IsAny<FilterNode>()))
                 .Returns(builds)
                 .Verifiable();

            var repository = new BuildRepository(mockProxy.Object);

            var parameters = new ODataSelectManyQueryOperation();
            parameters.FilterExpression = null;
            parameters.Keys = new Dictionary<string, string>();
            parameters.Keys.Add("Project", "Sample Project");

            var results = repository.GetBuildsByProject(parameters);

            Assert.IsTrue(results.SequenceEqual<Build>(builds), "The expected builds for a project don't match the results");
            mockProxy.VerifyAll();
        }

        [TestMethod]
        public void ItShouldGetAllBuildsForAGivenCollection()
        {
            var mockProxy = new Mock<ITFSBuildProxy>();
            var projects = new List<Project>();
            var buildsFromProjects = new List<Build>();

            projects.Add(new Project { Name = "Project 1" });
            projects.Add(new Project { Name = "Project 2" });

            buildsFromProjects.Add(new Build { Definition = "Full Build", Number = "FB Build 01", Project = "Project 1" });
            buildsFromProjects.Add(new Build { Definition = "Custom Build", Number = "CB Build 01", Project = "Project 1" });
            buildsFromProjects.Add(new Build { Definition = "Full Build", Number = "FB Build 01", Project = "Project 2" });
            buildsFromProjects.Add(new Build { Definition = "Custom Build", Number = "CB Build 01", Project = "Project 2" });

            mockProxy.Setup(p => p.GetBuildsByProjectCollection(It.IsAny<FilterNode>()))
                .Returns(buildsFromProjects)
                .Verifiable();

            var repository = new BuildRepository(mockProxy.Object);
            var parameters = new ODataSelectManyQueryOperation();
            parameters.FilterExpression = null;

            var results = repository.GetAll(parameters);

            Assert.IsTrue(results.SequenceEqual<Build>(buildsFromProjects), "The expected builds for a collection don't match the results");
            mockProxy.VerifyAll();
        }
    }
}
