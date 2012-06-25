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
    using System.Collections.Generic;
    using System.Data.Services;
    using System.Linq;
    using System.Web;
    using Data.Services.Toolkit.QueryModel;
    using Entities;
    using Serialization;

    public class BuildDefinitionRepository
    {
        private readonly ITFSBuildDefinitionProxy proxy;

        public BuildDefinitionRepository(ITFSBuildDefinitionProxy proxy)
        {
            this.proxy = proxy;
        }

        [RepositoryBehavior(HandlesFilter = true)]
        public IEnumerable<BuildDefinition> GetBuildDefinitionsByProject(ODataSelectManyQueryOperation operation)
        {
            if (operation == null)
            {
                throw new ArgumentNullException("operation");
            }

            return this.proxy.GetBuildDefinitionsByProject(operation.Key);
        }

        public BuildDefinition GetOne(string definition, string project)
        {
            return this.proxy.GetBuildDefinitionsByProject(project).SingleOrDefault(t => t.Definition.Equals(definition, StringComparison.OrdinalIgnoreCase));
        }

        public IEnumerable<BuildDefinition> GetAll()
        {
            throw new DataServiceException(501, "Not Implemented", "The 'Build Definitions' collection cannot be enumerated as a root collection. It should depend on a Project. (e.g. /Projects('12345')/Changes)", "en-US", null);
        }
    }
}