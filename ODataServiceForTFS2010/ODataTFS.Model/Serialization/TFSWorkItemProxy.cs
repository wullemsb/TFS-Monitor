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

namespace Microsoft.Samples.DPE.ODataTFS.Model.Serialization
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;
    using Microsoft.Samples.DPE.ODataTFS.Model.Entities;
    using Microsoft.Samples.DPE.ODataTFS.Model.ExpressionVisitors;

    public class TFSWorkItemProxy : TFSBaseProxy, ITFSWorkItemProxy
    {
        public TFSWorkItemProxy(Uri uri, ICredentials credentials)
            : base(uri, credentials)
        {
        }

        public WorkItem GetWorkItem(int workItemId)
        {
            var wiql = string.Format(CultureInfo.InvariantCulture, "SELECT [System.Id] FROM WorkItems WHERE [System.Id] = {0}", workItemId);

            return this.QueryWorkItems(wiql)
                        .Cast<TeamFoundation.WorkItemTracking.Client.WorkItem>()
                        .Select(w => w.ToModel(this.GetTfsWebAccessArtifactUrl(w.Uri)))
                        .FirstOrDefault();
        }

        public IEnumerable<WorkItem> GetWorkItemsByProject(string projectName, FilterNode rootFilterNode)
        {            
            var key = string.Format(CultureInfo.InvariantCulture, "TFSWorkItemProxy.GetWorkItemsByProject_{0}_{1}", projectName, this.GetFilterNodeKey(rootFilterNode));

            if (HttpContext.Current.Items[key] == null)
            {
                HttpContext.Current.Items[key] = this.RequestWorkItemsByProject(projectName, rootFilterNode);
            }

            return (IEnumerable<WorkItem>)HttpContext.Current.Items[key];
        }

        public IEnumerable<WorkItem> GetWorkItemsByProjectCollection(FilterNode rootFilterNode)
        {
            var key = string.Format(CultureInfo.InvariantCulture, "TFSWorkItemProxy.GetWorkItemsByProjectCollection_{0}", this.GetFilterNodeKey(rootFilterNode));
            
            if (HttpContext.Current.Items[key] == null)
            {
                HttpContext.Current.Items[key] = this.RequestWorkItemsByProjectCollection(rootFilterNode);
            }

            return (IEnumerable<WorkItem>)HttpContext.Current.Items[key];
        }       

        public IEnumerable<WorkItem> GetWorkItemsByQuery(Guid queryId)
        {
            var key = string.Format(CultureInfo.InvariantCulture, "TFSWorkItemProxy.GetWorkItemsByQuery_{0}", queryId);

            if (HttpContext.Current.Items[key] == null)
            {
                HttpContext.Current.Items[key] = this.RequestWorkItemsByQuery(queryId);
            }

            return (IEnumerable<WorkItem>)HttpContext.Current.Items[key];            
        }

        public IEnumerable<WorkItem> GetWorkItemsByChangeset(int changesetId)
        {
            var key = string.Format(CultureInfo.InvariantCulture, "TFSWorkItemProxy.GetWorkItemsByChangeset_{0}", changesetId);

            if (HttpContext.Current.Items[key] == null)
            {
                HttpContext.Current.Items[key] = this.RequestWorkItemsByChangeset(changesetId);
            }

            return (IEnumerable<WorkItem>)HttpContext.Current.Items[key];
        }

        public IEnumerable<WorkItem> GetWorkItemsByBuild(string projectName, string buildNumber, FilterNode rootFilterNode)
        {
            var key = string.Format(CultureInfo.InvariantCulture, "TFSWorkItemProxy.GetWorkItemsByBuild_{0}_{1}_{2}", projectName, buildNumber, this.GetFilterNodeKey(rootFilterNode));

            if (HttpContext.Current.Items[key] == null)
            {
                HttpContext.Current.Items[key] = this.RequestWorkItemsByBuild(projectName, buildNumber, rootFilterNode);
            }

            return (IEnumerable<WorkItem>)HttpContext.Current.Items[key];
        }

        public void CreateWorkItem(WorkItem workItem)
        {
            var workItemServer = this.TfsConnection.GetService<TeamFoundation.WorkItemTracking.Client.WorkItemStore>();
            
            var workItemEntity = workItem.ToEntity(workItemServer.Projects.Cast<Microsoft.TeamFoundation.WorkItemTracking.Client.Project>()
                                                   .SingleOrDefault(p => p.Name.Equals(workItem.Project, StringComparison.OrdinalIgnoreCase)));

            if (workItemEntity.Fields.Cast<TeamFoundation.WorkItemTracking.Client.Field>().Where(field => !field.IsValid).Any())
            {
                var errors = new StringBuilder();
                errors.AppendLine("The WorkItem cannot be saved because the following fields are invalid:");
                foreach (var field in workItemEntity.Fields.Cast<TeamFoundation.WorkItemTracking.Client.Field>().Where(field => !field.IsValid))
                {
                    errors.AppendLine(string.Format(CultureInfo.InvariantCulture, "Invalid field '{0}': {1} (Current Value '{2}')", field.Name, field.Status, field.Value));
                }

                throw new ArgumentException(errors.ToString(), "workItem");
            }

            workItemEntity.Save();
        }

        public void UpdateWorkItem(WorkItem workItem)
        {
            if (workItem == null)
            {
                throw new ArgumentNullException("workItem");
            }

            var wiql = string.Format(CultureInfo.InvariantCulture, "SELECT [System.Id] FROM WorkItems WHERE [System.Id] = {0}", workItem.Id);

            var workItemEntity = this.QueryWorkItems(wiql)
                                        .Cast<TeamFoundation.WorkItemTracking.Client.WorkItem>()
                                        .SingleOrDefault();

            workItemEntity.UpdateFromModel(workItem);

            if (workItemEntity.Fields.Cast<TeamFoundation.WorkItemTracking.Client.Field>().Where(field => !field.IsValid).Any())
            {
                var errors = new StringBuilder();
                errors.AppendLine("The WorkItem cannot be updated because the following fields are invalid:");
                foreach (var field in workItemEntity.Fields.Cast<TeamFoundation.WorkItemTracking.Client.Field>().Where(field => !field.IsValid))
                {
                    errors.AppendLine(string.Format(CultureInfo.InvariantCulture, "Invalid field '{0}': {1} (Current Value '{2}')", field.Name, field.Status, field.Value));
                }

                throw new ArgumentException(errors.ToString(), "workItem");
            }

            workItemEntity.Save();
        }

        private static string BuildWiql(FilterNode rootFilterNode)
        {
            var constrains = string.Empty;
            if (rootFilterNode != null)
            {
                foreach (var filterNode in rootFilterNode)
                {
                    switch (filterNode.Key)
                    {
                        case "Id":
                            constrains += AddComparisonConstrainToWiql(filterNode, "[System.Id]");
                            break;
                        case "Project":
                            constrains += AddComparisonConstrainToWiql(filterNode, "[System.TeamProject]");
                            break;
                        case "AreaPath":
                            constrains += AddComparisonConstrainToWiql(filterNode, "[System.AreaPath]");
                            break;
                        case "IterationPath":
                            constrains += AddComparisonConstrainToWiql(filterNode, "[System.IterationPath]");
                            break;
                        case "Revision":
                            constrains += AddComparisonConstrainToWiql(filterNode, "[System.Rev]");
                            break;
                        case "Priority":
                            constrains += AddComparisonConstrainToWiql(filterNode, "[Microsoft.VSTS.Common.Priority]");
                            break;
                        case "Severity":
                            constrains += AddComparisonConstrainToWiql(filterNode, "[Microsoft.VSTS.Common.Severity]");
                            break;
                        case "StackRank":
                            constrains += AddComparisonConstrainToWiql(filterNode, "[Microsoft.VSTS.Common.StackRank]");
                            break;
                        case "AssignedTo":
                            constrains += AddComparisonConstrainToWiql(filterNode, "[System.AssignedTo]");
                            break;
                        case "CreatedDate":
                            constrains += AddComparisonConstrainToWiql(filterNode, "[System.CreatedDate]");
                            break;
                        case "CreatedBy":
                            constrains += AddComparisonConstrainToWiql(filterNode, "[System.CreatedBy]");
                            break;
                        case "ChangedDate":
                            constrains += AddComparisonConstrainToWiql(filterNode, "[System.ChangedDate]");
                            break;
                        case "ChangedBy":
                            constrains += AddComparisonConstrainToWiql(filterNode, "[System.ChangedBy]");
                            break;
                        case "ResolvedBy":
                            constrains += AddComparisonConstrainToWiql(filterNode, "[Microsoft.VSTS.Common.ResolvedBy]");
                            break;
                        case "State":
                            constrains += AddComparisonConstrainToWiql(filterNode, "[System.State]");
                            break;
                        case "Type":
                            constrains += AddComparisonConstrainToWiql(filterNode, "[System.WorkItemType]");
                            break;
                        case "Reason":
                            constrains += AddComparisonConstrainToWiql(filterNode, "[System.Reason]");
                            break;
                        case "FoundInBuild":
                            constrains += AddComparisonConstrainToWiql(filterNode, "[Microsoft.VSTS.Build.FoundIn]");
                            break;
                        case "IntegratedInBuild":
                            constrains += AddComparisonConstrainToWiql(filterNode, "[Microsoft.VSTS.Build.IntegrationBuild]");
                            break;
                        case "Title":
                            constrains += AddComparisonConstrainToWiql(filterNode, "[System.Title]");
                            break;
                        case "Description":
                            constrains += AddComparisonConstrainToWiql(filterNode, "[System.Description]");
                            break;
                        case "ReproSteps":
                            constrains += AddComparisonConstrainToWiql(filterNode, "[Microsoft.VSTS.TCM.ReproSteps]");
                            break;
                    }
                }
            }

            if (constrains.StartsWith(" OR ", StringComparison.OrdinalIgnoreCase))
            {
                constrains = constrains.Substring(" OR ".Length);
            }

            if (constrains.StartsWith(" AND ", StringComparison.OrdinalIgnoreCase))
            {
                constrains = constrains.Substring(" OR ".Length);
            }

            var wiql = string.Format(CultureInfo.InvariantCulture, "SELECT [System.Id] FROM WorkItems {0} {1}", string.IsNullOrEmpty(constrains) ? string.Empty : "WHERE", constrains).Trim();

            return wiql;
        }

        private static string AddComparisonConstrainToWiql(FilterNode filterNode, string tfsFieldName)
        {
            if (filterNode != null)
            {
                var sign = default(string);

                switch (filterNode.Sign)
                {
                    case FilterExpressionType.Equal:
                        sign = "=";
                        break;
                    case FilterExpressionType.NotEqual:
                        sign = "<>";
                        break;
                    case FilterExpressionType.GreaterThan:
                        sign = ">";
                        break;
                    case FilterExpressionType.GreaterThanOrEqual:
                        sign = ">=";
                        break;
                    case FilterExpressionType.LessThan:
                        sign = "<";
                        break;
                    case FilterExpressionType.LessThanOrEqual:
                        sign = "<=";
                        break;
                    case FilterExpressionType.Contains:
                        sign = filterNode.Key.Equals("AreaPath", StringComparison.OrdinalIgnoreCase) || filterNode.Key.Equals("IterationPath", StringComparison.OrdinalIgnoreCase) ? "UNDER" : "CONTAINS";
                        break;
                    case FilterExpressionType.NotContains:
                        sign = filterNode.Key.Equals("AreaPath", StringComparison.OrdinalIgnoreCase) || filterNode.Key.Equals("IterationPath", StringComparison.OrdinalIgnoreCase) ? "NOT UNDER" : "NOT CONTAINS";
                        break;

                    default:
                        throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, "WorkItem {0} can only be filtered with equal, not equal, greater than, lower than, greater than or equal, lower than or equal operators", filterNode.Key));
                }

                return string.Format(CultureInfo.InvariantCulture, " {0} {1} {2} '{3}' ", filterNode.NodeRelationship.ToString(), tfsFieldName, sign, filterNode.Value);
            }

            return string.Empty;
        }

        private IEnumerable<WorkItem> RequestWorkItemsByQuery(Guid queryId)
        {
            var workItemServer = this.TfsConnection.GetService<TeamFoundation.WorkItemTracking.Client.WorkItemStore>();
            string queryText;
            string projectName;

            try
            {
                var query = workItemServer.GetQueryDefinition(queryId);
                queryText = query.QueryText;
                projectName = query.Project.Name;
            }
            catch (ArgumentException)
            {
                // Weird bug in the TFS API, queries stored in TFS2008 cannot be accessed
                // using the new 2010 API, must use the old deprecated method.
#pragma warning disable
                var query = workItemServer.GetStoredQuery(queryId);
                queryText = query.QueryText;
                projectName = query.Project.Name;
#pragma warning restore
            }

            var wiql = Regex.Replace(queryText, "@project", string.Format(CultureInfo.InvariantCulture, "'{0}'", projectName), RegexOptions.IgnoreCase);
            var list = workItemServer.Query(wiql)
                        .Cast<TeamFoundation.WorkItemTracking.Client.WorkItem>()
                        .Select(w => w.ToModel(w.Uri)).ToArray();

            return list;
        }

        private IEnumerable<WorkItem> RequestWorkItemsByBuild(string projectName, string buildNumber, FilterNode rootFilterNode)
        {
            if (rootFilterNode == null)
            {
                rootFilterNode = new FilterNode() { Key = "Project", Sign = FilterExpressionType.Equal, Value = projectName };
            }
            else if (rootFilterNode.SingleOrDefault(p => p.Key.Equals("Project", StringComparison.OrdinalIgnoreCase)) == null)
            {
                rootFilterNode.AddNode(new FilterNode() { Key = "Project", Sign = FilterExpressionType.Equal, Value = projectName, NodeRelationship = FilterNodeRelationship.And });
            }

            if (rootFilterNode.SingleOrDefault(p => p.Key.Equals("FoundInBuild", StringComparison.OrdinalIgnoreCase)) == null)
            {
                rootFilterNode.AddNode(new FilterNode() { Key = "FoundInBuild", Sign = FilterExpressionType.Equal, Value = buildNumber, NodeRelationship = FilterNodeRelationship.And });
            }

            return this.QueryWorkItems(BuildWiql(rootFilterNode))
                        .Cast<TeamFoundation.WorkItemTracking.Client.WorkItem>()
                        .Select(w => w.ToModel(w.Uri)).ToArray();
        }

        private IEnumerable<WorkItem> RequestWorkItemsByChangeset(int changesetId)
        {
            var versionControlServer = this.TfsConnection.GetService<TeamFoundation.VersionControl.Client.VersionControlServer>();

            var list = versionControlServer.GetChangeset(changesetId, false, false).WorkItems.Select(w => w.ToModel(w.Uri)).ToArray();

            return list;
        }

        private IEnumerable<WorkItem> RequestWorkItemsByProjectCollection(FilterNode rootFilterNode)
        {
            var wiql = BuildWiql(rootFilterNode);
            return this.QueryWorkItems(wiql)
                        .Cast<TeamFoundation.WorkItemTracking.Client.WorkItem>()
                        .Select(w => w.ToModel(this.GetTfsWebAccessArtifactUrl(w.Uri))).ToArray();
        }

        private IEnumerable<WorkItem> RequestWorkItemsByProject(string projectName, FilterNode rootFilterNode)
        {
            if (rootFilterNode != null)
            {
                if (rootFilterNode.SingleOrDefault(p => p.Key.Equals("Project", StringComparison.OrdinalIgnoreCase)) == null)
                {
                    rootFilterNode.AddNode(new FilterNode() { Key = "Project", Sign = FilterExpressionType.Equal, Value = projectName, NodeRelationship = FilterNodeRelationship.And });
                }
            }
            else
            {
                rootFilterNode = new FilterNode() { Key = "Project", Sign = FilterExpressionType.Equal, Value = projectName };
            }

            var list = this.QueryWorkItems(BuildWiql(rootFilterNode))
                        .Cast<TeamFoundation.WorkItemTracking.Client.WorkItem>()
                        .Select(w => w.ToModel(this.GetTfsWebAccessArtifactUrl(w.Uri))).ToArray();

            return list;
        }
    }
}
