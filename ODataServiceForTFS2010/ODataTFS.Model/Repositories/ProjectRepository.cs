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
    using System.Linq;
    using Microsoft.Samples.DPE.ODataTFS.Model.Entities;
    using Microsoft.Samples.DPE.ODataTFS.Model.Serialization;

    public class ProjectRepository
    {
        private readonly ITFSProjectProxy proxy;

        public ProjectRepository(ITFSProjectProxy proxy)
        {
            this.proxy = proxy;
        }

        public Project GetOne(string name)
        {
            return this.proxy.GetProjectsByProjectCollection().SingleOrDefault(t => t.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public IEnumerable<Project> GetAll()
        {
            return this.proxy.GetProjectsByProjectCollection();
        }
    }
}
