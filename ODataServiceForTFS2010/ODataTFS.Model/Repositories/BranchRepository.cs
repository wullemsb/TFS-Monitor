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
    using Microsoft.Samples.DPE.ODataTFS.Model.Entities;
    using Microsoft.Samples.DPE.ODataTFS.Model.Serialization;

    public class BranchRepository
    {
        private readonly ITFSBranchProxy proxy;

        public BranchRepository(ITFSBranchProxy proxy)
        {
            this.proxy = proxy;
        }

        public Branch GetOne(string path)
        {
            return this.proxy.GetBranch(EntityTranslator.DecodePath(path));
        }

        public IEnumerable<Branch> GetBranchesByProject(string name)
        {
            return this.proxy.GetBranchesByProject(name);
        }

        public IEnumerable<Branch> GetAll()
        {
            return this.proxy.GetBranchesByProjectCollection();
        }
    }
}
