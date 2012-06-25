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
    using Microsoft.Data.Services.Toolkit.QueryModel;
    using Microsoft.Samples.DPE.ODataTFS.Model.Entities;
    using Microsoft.Samples.DPE.ODataTFS.Model.ExpressionVisitors;
    using Microsoft.Samples.DPE.ODataTFS.Model.Serialization;

    public class BuildRepository
    {
        private readonly ITFSBuildProxy proxy;

        public BuildRepository(ITFSBuildProxy proxy)
        {
            this.proxy = proxy;
        }

        public Build GetOne(string project, string definition, string number)
        {
            return this.proxy.GetBuild(project, definition, number);
        }

        [RepositoryBehavior(HandlesFilter = true)]
        public IEnumerable<Build> GetBuildsByProject(ODataSelectManyQueryOperation operation)
        {
            if (operation == null)
            {
                throw new ArgumentNullException("operation");
            }

            var parameters = new BuildFilterExpressionVisitor(operation.FilterExpression).Eval();
            return this.proxy.GetBuildsByProject(operation.Key, parameters);
        }

        [RepositoryBehavior(HandlesFilter = true)]
        public IEnumerable<Build> GetAll(ODataQueryOperation operation)
        {
            if (operation == null)
            {
                throw new ArgumentNullException("operation");
            }

            var parameters = new BuildFilterExpressionVisitor(operation.FilterExpression).Eval();
            return this.proxy.GetBuildsByProjectCollection(parameters);
        }
    }
}