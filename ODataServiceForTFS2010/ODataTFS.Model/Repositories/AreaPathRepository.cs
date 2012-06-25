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
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Samples.DPE.ODataTFS.Model.Entities;
    using Microsoft.Samples.DPE.ODataTFS.Model.Serialization;

    public class AreaPathRepository
    {
        private readonly ITFSAreaPathProxy proxy;

        public AreaPathRepository(ITFSAreaPathProxy proxy)
        {
            this.proxy = proxy;
        }

        public AreaPath GetOne(string path)
        {
            return this.proxy.GetAllAreaPaths().SingleOrDefault(a => a.Path.Equals(path));
        }

        public IEnumerable<AreaPath> GetAll()
        {
            return this.proxy.GetAllAreaPaths();
        }

        public IEnumerable<AreaPath> GetSubAreasByAreaPath(string path)
        {
            return this.proxy.GetSubAreas(path);
        }

        public IEnumerable<AreaPath> GetAreaPathsByProject(string projectName)
        {
            return this.proxy.GetAreaPathsByProject(projectName);
        }
    }
}
