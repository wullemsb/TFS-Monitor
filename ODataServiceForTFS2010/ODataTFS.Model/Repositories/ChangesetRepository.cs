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
    using System.Globalization;
    using System.Linq;
    using Microsoft.Data.Services.Toolkit.QueryModel;
    using Microsoft.Samples.DPE.ODataTFS.Model.Entities;
    using Microsoft.Samples.DPE.ODataTFS.Model.ExpressionVisitors;
    using Microsoft.Samples.DPE.ODataTFS.Model.Serialization;

    public class ChangesetRepository
    {
        private readonly ITFSChangesetProxy proxy;

        public ChangesetRepository(ITFSChangesetProxy proxy)
        {
            this.proxy = proxy;
        }

        public Changeset GetOne(string id)
        {
            var changesetId = 0;
            if (!int.TryParse(id, NumberStyles.Integer, CultureInfo.InvariantCulture, out changesetId))
            {
                throw new ArgumentException("The parameter id should be numeric", "id");
            }

            return this.proxy.GetChangeset(changesetId);
        }

        [RepositoryBehavior(HandlesFilter = true)]
        public IEnumerable<Changeset> GetChangesetsByProject(ODataSelectManyQueryOperation operation)
        {
            if (operation == null)
            {
                throw new ArgumentNullException("operation");
            }

            var parameters = new ChangesetFilterExpressionVisitor(operation.FilterExpression).Eval();
            int topRequestValue = this.GetTopRequestValue(operation, parameters);

            return this.proxy.GetChangesetsByProject(operation.Key, parameters, topRequestValue);
        }

        [RepositoryBehavior(HandlesFilter = true)]
        public IEnumerable<Changeset> GetChangesetsByBranch(ODataSelectManyQueryOperation operation)
        {
            if (operation == null)
            {
                throw new ArgumentNullException("operation");
            }

            var parameters = new ChangesetFilterExpressionVisitor(operation.FilterExpression).Eval();
            int topRequestValue = this.GetTopRequestValue(operation, parameters);

            return this.proxy.GetChangesetsByBranch(operation.Key, parameters, topRequestValue);
        }

        public IEnumerable<Changeset> GetChangesetsByBuild(string project, string definition, string number)
        {
            return this.proxy.GetChangesetsByBuild(project, definition, number);
        }

        [RepositoryBehavior(HandlesFilter = true)]
        public IEnumerable<Changeset> GetAll(ODataQueryOperation operation)
        {
            if (operation == null)
            {
                throw new ArgumentNullException("operation");
            }

            var parameters = new ChangesetFilterExpressionVisitor(operation.FilterExpression).Eval();
            int topRequestValue = this.GetTopRequestValue(operation, parameters);

            return this.proxy.GetChangesetsByProjectCollection(parameters, topRequestValue);
        }

        /// <summary>
        /// Gets the count treshold to minimize the number of elements retrieved from TFS
        /// </summary>
        private int GetTopRequestValue(ODataQueryOperation operation, FilterNode parameters)
        {
            int topRequestValue = int.MaxValue;

            if (parameters == null || !parameters.Any(p => p.Key.Equals("Owner") || p.Key.Equals("CreationDate") || p.Key.Equals("Comment") || p.Key.Equals("ArtifactUri")))
            {
                if (operation.IsCountRequest)
                {
                    if (!string.IsNullOrEmpty(operation.ContinuationToken))
                    {
                        var parts = operation.ContinuationToken.Split(':').Select(int.Parse);
                        topRequestValue = parts.First() + parts.Last() + 1;
                    }
                }
                else
                {
                    if (operation.TopCount > 0)
                    {
                        topRequestValue = operation.TopCount + operation.SkipCount + 1;
                    }
                }
            }

            return topRequestValue;
        }
    }
}