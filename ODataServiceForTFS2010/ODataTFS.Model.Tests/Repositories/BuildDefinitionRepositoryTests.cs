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

namespace ODataTFS.Model.Tests.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Data.Services.Toolkit.QueryModel;
    using Microsoft.Samples.DPE.ODataTFS.Model.Entities;
    using Microsoft.Samples.DPE.ODataTFS.Model.Repositories;
    using Microsoft.Samples.DPE.ODataTFS.Model.Serialization;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class BuildDefinitionRepositoryTests
    {
        [TestMethod]
        public void ItShouldGetOneBuildDefinition()
        {
            var mockProxy = new Mock<ITFSBuildDefinitionProxy>();
            var buildDefinitions = new List<BuildDefinition>();

            buildDefinitions.Add(new BuildDefinition { Definition = "Full Build", Project = "SampleProject" });
            buildDefinitions.Add(new BuildDefinition { Definition = "Custom Build", Project = "SampleProject" });

            mockProxy.Setup(b => b.GetBuildDefinitionsByProject(It.Is<string>(s => s == "SampleProject")))
                 .Returns(buildDefinitions)
                 .Verifiable();

            var repository = new BuildDefinitionRepository(mockProxy.Object);

            var buildDefinition = repository.GetOne("Full Build", "SampleProject");

            Assert.IsNotNull(buildDefinition);
            Assert.AreEqual(buildDefinition.Definition, "Full Build");
            Assert.AreEqual(buildDefinition.Project, "SampleProject");
            mockProxy.VerifyAll();
        }

        [TestMethod]
        public void ItShouldGetAllBuildDefinitionsForAGivenProject()
        {
            var mockProxy = new Mock<ITFSBuildDefinitionProxy>();
            var buildDefinitions = new List<BuildDefinition>();

            buildDefinitions.Add(new BuildDefinition { Definition = "Full Build", Project = "SampleProject" });
            buildDefinitions.Add(new BuildDefinition { Definition = "Custom Build", Project = "SampleProject" });

            mockProxy.Setup(b => b.GetBuildDefinitionsByProject(It.Is<string>(s => s == "SampleProject")))
                 .Returns(buildDefinitions)
                 .Verifiable();

            var repository = new BuildDefinitionRepository(mockProxy.Object);

            var parameters = new ODataSelectManyQueryOperation();
            parameters.FilterExpression = null;
            parameters.Keys = new Dictionary<string, string>();
            parameters.Keys.Add("Project", "SampleProject");

            var results = repository.GetBuildDefinitionsByProject(parameters);

            Assert.IsTrue(results.SequenceEqual<BuildDefinition>(buildDefinitions), "The expected build definitions for a project don't match the results");
            mockProxy.VerifyAll();
        }
    }
}
