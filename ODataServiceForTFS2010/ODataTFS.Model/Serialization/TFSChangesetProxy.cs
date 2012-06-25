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
    using System.Data.Services;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Web;
    using Microsoft.Samples.DPE.ODataTFS.Model.Entities;
    using Microsoft.Samples.DPE.ODataTFS.Model.ExpressionVisitors;

    public class TFSChangesetProxy : TFSBaseProxy, ITFSChangesetProxy
    {
        public TFSChangesetProxy(Uri uri, ICredentials credentials) : base(uri, credentials) 
        { 
        }

        public Changeset GetChangeset(int changesetId)
        {
            var versionControlServer = this.TfsConnection.GetService<TeamFoundation.VersionControl.Client.VersionControlServer>();

            var tfsChangeset = versionControlServer.GetChangeset(changesetId, false, false);
            return tfsChangeset.ToModel(this.GetTfsWebAccessArtifactUrl(tfsChangeset.ArtifactUri));
        }

        public IEnumerable<Changeset> GetChangesetsByBranch(string path, FilterNode rootFilterNode, int topRequestValue)
        {
            var key = this.GetChangesetsByBranchKey(path, rootFilterNode, topRequestValue);

            if (HttpContext.Current.Items[key] == null)
            {
                HttpContext.Current.Items[key] = this.RequestChangesetsByBranch(path, rootFilterNode, topRequestValue);
            }

            return (IEnumerable<Changeset>)HttpContext.Current.Items[key];
        }

        public IEnumerable<Changeset> GetChangesetsByProjectCollection(FilterNode rootFilterNode, int topRequestValue)
        {
            var key = this.GetChangesetsByProjectCollectionKey(rootFilterNode, topRequestValue);

            if (HttpContext.Current.Items[key] == null)
            {
                HttpContext.Current.Items[key] = this.RequestChangesetsByProjectCollection(rootFilterNode, topRequestValue);
            }

            return (IEnumerable<Changeset>)HttpContext.Current.Items[key];
        }

        public IEnumerable<Changeset> GetChangesetsByProject(string projectName, FilterNode rootFilterNode, int topRequestValue)
        {
            var key = this.GetChangesetsByProjectKey(projectName, rootFilterNode, topRequestValue);

            if (HttpContext.Current.Items[key] == null)
            {
                HttpContext.Current.Items[key] = this.RequestChangesetsByProject(projectName, rootFilterNode, topRequestValue);
            }

            return (IEnumerable<Changeset>)HttpContext.Current.Items[key];
        }

        public IEnumerable<Changeset> GetChangesetsByBuild(string projectName, string buildDefinition, string buildNumber)
        {
            var key = this.GetChangesetsByBuildKey(projectName, buildDefinition, buildNumber);

            if (HttpContext.Current.Items[key] == null)
            {
                HttpContext.Current.Items[key] = this.RequestChangesetsByBuild(projectName, buildDefinition, buildNumber);
            }

            return (IEnumerable<Changeset>)HttpContext.Current.Items[key];
        }

        private static DateTime RoundDateToSeconds(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, date.Kind);
        }

        private static string GetRequestedPath(string branch, string projectName, FilterNode rootFilterNode)
        {
            var path = "$/";
            var branchParameter = default(FilterNode);
            
            if (rootFilterNode != null)
            {
                branchParameter = rootFilterNode.SingleOrDefault(p => p.Key.Equals("Branch", StringComparison.OrdinalIgnoreCase));
            }
            
            if (!string.IsNullOrEmpty(branch))
            {
                var branchName = EntityTranslator.DecodePath(branch);
                path = string.Format(CultureInfo.InvariantCulture, branchName.StartsWith("$/", StringComparison.Ordinal) ? "{0}/*" : "$/{0}/*", branchName);
            }
            else if (branchParameter != null && !string.IsNullOrEmpty(branchParameter.Value))
            {
                if (branchParameter.Sign != FilterExpressionType.Equal)
                {
                    throw new DataServiceException(501, "Not Implemented", "Changeset Branch can only be filtered with an equality operator", "en-US", null);
                }

                var branchName = EntityTranslator.DecodePath(branchParameter.Value);
                path = string.Format(CultureInfo.InvariantCulture, branchName.StartsWith("$/", StringComparison.Ordinal) ? "{0}/*" : "$/{0}/*", branchName);
            }
            else if (!string.IsNullOrEmpty(projectName))
            {
                path = string.Format(CultureInfo.InvariantCulture, "$/{0}", projectName);
            }

            return path;
        }

        private static TeamFoundation.VersionControl.Client.VersionSpec GetRequestedVersionTo(FilterNode rootFilterNode)
        {
            if (rootFilterNode != null)
            {
                if (rootFilterNode.SingleOrDefault(p => p.Key.Equals("Id", StringComparison.OrdinalIgnoreCase)) != null)
                {
                    return GetRequestedIdVersionTo(rootFilterNode);
                }
                else if (rootFilterNode.SingleOrDefault(p => p.Key.Equals("CreationDate", StringComparison.OrdinalIgnoreCase)) != null)
                {
                    return GetRequestedDateVersionTo(rootFilterNode);
                }
            }

            return null;
        }

        private static TeamFoundation.VersionControl.Client.VersionSpec GetRequestedVersionFrom(FilterNode rootFilterNode)
        {
            if (rootFilterNode != null)
            {
                if (rootFilterNode.SingleOrDefault(p => p.Key.Equals("Id", StringComparison.OrdinalIgnoreCase)) != null)
                {
                    return GetRequestedIdVersionFrom(rootFilterNode);
                }
                else if (rootFilterNode.SingleOrDefault(p => p.Key.Equals("CreationDate", StringComparison.OrdinalIgnoreCase)) != null)
                {
                    return GetRequestedDateVersionFrom(rootFilterNode);
                }
            }

            return null;
        }

        private static TeamFoundation.VersionControl.Client.VersionSpec GetRequestedIdVersionTo(FilterNode rootFilterNode)
        {
            var versionTo = default(TeamFoundation.VersionControl.Client.ChangesetVersionSpec);

            if (rootFilterNode != null)
            {
                var idParameter = rootFilterNode.SingleOrDefault(p => p.Key.Equals("Id", StringComparison.OrdinalIgnoreCase));

                if (idParameter != null)
                {
                    switch (idParameter.Sign)
                    {
                        case FilterExpressionType.Equal:
                        case FilterExpressionType.LessThanOrEqual:
                            versionTo = new TeamFoundation.VersionControl.Client.ChangesetVersionSpec(idParameter.Value);
                            break;
                        case FilterExpressionType.LessThan:
                            int versionValue;
                            if (int.TryParse(idParameter.Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out versionValue))
                            {
                                versionTo = new TeamFoundation.VersionControl.Client.ChangesetVersionSpec(versionValue - 1);
                            }

                            break;
                        case FilterExpressionType.GreaterThan:
                        case FilterExpressionType.GreaterThanOrEqual:
                            versionTo = null;
                            break;
                        default:
                            throw new DataServiceException(501, "Not Implemented", "Changeset Id can only be filtered with an equality, greater than, less than, greater than or equal and less than or equal operators", "en-US", null);
                    }
                }
            }

            return versionTo;
        }

        private static TeamFoundation.VersionControl.Client.VersionSpec GetRequestedIdVersionFrom(FilterNode rootFilte)
        {
            var versionFrom = default(TeamFoundation.VersionControl.Client.ChangesetVersionSpec);

            if (rootFilte != null)
            {
                var idParameter = rootFilte.SingleOrDefault(p => p.Key.Equals("Id", StringComparison.OrdinalIgnoreCase));

                if (idParameter != null)
                {
                    switch (idParameter.Sign)
                    {
                        case FilterExpressionType.Equal:
                        case FilterExpressionType.GreaterThanOrEqual:
                            versionFrom = new TeamFoundation.VersionControl.Client.ChangesetVersionSpec(idParameter.Value);
                            break;
                        case FilterExpressionType.GreaterThan:
                            int versionValue;
                            if (int.TryParse(idParameter.Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out versionValue))
                            {
                                versionFrom = new TeamFoundation.VersionControl.Client.ChangesetVersionSpec(versionValue + 1);
                            }

                            break;
                        case FilterExpressionType.LessThan:
                        case FilterExpressionType.LessThanOrEqual:
                            break;
                        default:
                            throw new DataServiceException(501, "Not Implemented", "Changeset Id can only be filtered with an equality, greater than, less than, greater than or equal and less than or equal operators", "en-US", null);
                    }
                }
            }

            return versionFrom;
        }

        private static TeamFoundation.VersionControl.Client.VersionSpec GetRequestedDateVersionTo(FilterNode rootFilterNode)
        {
            var versionTo = default(TeamFoundation.VersionControl.Client.DateVersionSpec);

            if (rootFilterNode != null)
            {
                var dateParameter = rootFilterNode.SingleOrDefault(p => p.Key.Equals("CreationDate", StringComparison.OrdinalIgnoreCase));
                var dateTo = default(DateTime);

                if (dateParameter != null && DateTime.TryParse(dateParameter.Value, out dateTo))
                {
                    switch (dateParameter.Sign)
                    {
                        case FilterExpressionType.Equal:
                        case FilterExpressionType.LessThan:
                        case FilterExpressionType.LessThanOrEqual:
                            versionTo = new TeamFoundation.VersionControl.Client.DateVersionSpec(dateTo.AddSeconds(1));
                            break;
                        case FilterExpressionType.GreaterThan:
                        case FilterExpressionType.GreaterThanOrEqual:
                            versionTo = null;
                            break;
                        default:
                            throw new DataServiceException(501, "Not Implemented", "Changeset Creation Date can only be filtered with an equality, greater than, less than, greater than or equal and less than or equal operators", "en-US", null);
                    }
                }
            }

            return versionTo;
        }

        private static TeamFoundation.VersionControl.Client.VersionSpec GetRequestedDateVersionFrom(FilterNode rootFilte)
        {
            var versionFrom = default(TeamFoundation.VersionControl.Client.DateVersionSpec);

            if (rootFilte != null)
            {
                var dateParameter = rootFilte.SingleOrDefault(p => p.Key.Equals("CreationDate", StringComparison.OrdinalIgnoreCase));
                var dateFrom = default(DateTime);
                
                if (dateParameter != null && DateTime.TryParse(dateParameter.Value, out dateFrom))
                {
                    switch (dateParameter.Sign)
                    {
                        case FilterExpressionType.Equal:
                            versionFrom = new TeamFoundation.VersionControl.Client.DateVersionSpec(dateFrom);
                            break;
                        case FilterExpressionType.GreaterThan:
                        case FilterExpressionType.GreaterThanOrEqual:
                            versionFrom = new TeamFoundation.VersionControl.Client.DateVersionSpec(dateFrom.AddSeconds(-1));
                            break;
                        case FilterExpressionType.LessThan:
                        case FilterExpressionType.LessThanOrEqual:
                            versionFrom = null;
                            break;
                        default:
                            throw new DataServiceException(501, "Not Implemented", "Changeset Creation Date can only be filtered with an equality, greater than, less than, greater than or equal and less than or equal operators", "en-US", null);
                    }
                }
            }

            return versionFrom;
        }

        private static string GetRequestedCommitter(FilterNode rootFilterNode)
        {
            var committer = default(string);

            if (rootFilterNode != null)
            {
                var committerParameter = rootFilterNode.SingleOrDefault(p => p.Key.Equals("Committer", StringComparison.OrdinalIgnoreCase));
                if (committerParameter != null)
                {
                    if (committerParameter.Sign != FilterExpressionType.Equal)
                    {
                        throw new DataServiceException(501, "Not Implemented", "Changeset committer can only be filtered with an equality operator", "en-US", null);
                    }

                    committer = committerParameter.Value;
                }
            }

            return committer;
        }

        private static IEnumerable<Changeset> FilterChangesets(IEnumerable<Changeset> changesets, FilterNode rootFilterNode)
        {
            if (rootFilterNode != null)
            {
                changesets = FilterChangesetsByCreationDate(changesets, rootFilterNode);
                changesets = FilterChangesetsByOwner(changesets, rootFilterNode);
                changesets = FilterChangesetsByComment(changesets, rootFilterNode);
                changesets = FilterChangesetsByArtifactUri(changesets, rootFilterNode);
            }

            return changesets;
        }

        private static IEnumerable<Changeset> FilterChangesetsByArtifactUri(IEnumerable<Changeset> changesets, FilterNode rootFilterNode)
        {
            if (rootFilterNode.Count(p => p.Key.Equals("ArtifactUri")) > 0)
            {
                var artifactUriParameter = rootFilterNode.SingleOrDefault(p => p.Key.Equals("ArtifactUri"));
                switch (artifactUriParameter.Sign)
                {
                    case FilterExpressionType.Equal:
                        changesets = artifactUriParameter != null
                            ? changesets.Where(c => c.ArtifactUri != null && c.ArtifactUri.Equals(artifactUriParameter.Value, StringComparison.OrdinalIgnoreCase))
                            : changesets.Where(c => c.ArtifactUri == null);
                        break;
                    case FilterExpressionType.NotEqual:
                        changesets = artifactUriParameter != null
                            ? changesets.Where(c => c.ArtifactUri == null || !c.ArtifactUri.Equals(artifactUriParameter.Value, StringComparison.OrdinalIgnoreCase))
                            : changesets.Where(c => c.ArtifactUri != null);
                        break;
                    default:
                        throw new NotSupportedException("Changeset Artifact Uri can only be filtered with an equal or not equal operator");
                }
            }

            return changesets;
        }

        private static IEnumerable<Changeset> FilterChangesetsByComment(IEnumerable<Changeset> changesets, FilterNode rootFilterNode)
        {
            if (rootFilterNode.Count(p => p.Key.Equals("Comment")) > 0)
            {
                var commentParameter = rootFilterNode.SingleOrDefault(p => p.Key.Equals("Comment"));
                switch (commentParameter.Sign)
                {
                    case FilterExpressionType.Equal:
                        changesets = commentParameter != null
                            ? changesets.Where(c => c.Comment != null && c.Comment.Equals(commentParameter.Value, StringComparison.OrdinalIgnoreCase))
                            : changesets.Where(c => c.Comment == null);
                        break;
                    case FilterExpressionType.NotEqual:
                        changesets = commentParameter != null
                            ? changesets.Where(c => c.Comment == null || !c.Comment.Equals(commentParameter.Value, StringComparison.OrdinalIgnoreCase))
                            : changesets.Where(c => c.Comment != null);
                        break;
                    default:
                        throw new NotSupportedException("Changeset Comment can only be filtered with an equal or not equal operator");
                }
            }

            return changesets;
        }

        private static IEnumerable<Changeset> FilterChangesetsByOwner(IEnumerable<Changeset> changesets, FilterNode rootFilterNode)
        {
            if (rootFilterNode.Count(p => p.Key.Equals("Owner")) > 0)
            {
                var ownerParameter = rootFilterNode.SingleOrDefault(p => p.Key.Equals("Owner"));
                switch (ownerParameter.Sign)
                {
                    case FilterExpressionType.Equal:
                        changesets = ownerParameter != null
                            ? changesets.Where(c => c.Owner != null && c.Owner.Equals(ownerParameter.Value, StringComparison.OrdinalIgnoreCase))
                            : changesets.Where(c => c.Owner == null);
                        break;
                    case FilterExpressionType.NotEqual:
                        changesets = ownerParameter != null
                            ? changesets.Where(c => c.Owner == null || !c.Owner.Equals(ownerParameter.Value, StringComparison.OrdinalIgnoreCase))
                            : changesets.Where(c => c.Owner != null);
                        break;
                    default:
                        throw new NotSupportedException("Changeset Owner can only be filtered with an equal or not equal operator");
                }
            }

            return changesets;
        }

        private static IEnumerable<Changeset> FilterChangesetsByCreationDate(IEnumerable<Changeset> changesets, FilterNode rootFilterNode)
        {
            if (rootFilterNode.Count(p => p.Key.Equals("CreationDate")) > 0 && rootFilterNode.Count(p => p.Key.Equals("Id")) > 0)
            {
                // We could not filter by both parameters using the API. So we filter programmatically
                var dateParameter = rootFilterNode.SingleOrDefault(p => p.Key.Equals("CreationDate"));
                var dateValue = default(DateTime);

                if (DateTime.TryParse(dateParameter.Value, out dateValue))
                {
                    switch (dateParameter.Sign)
                    {
                        case FilterExpressionType.Equal:
                            changesets = dateParameter != null
                                ? changesets.Where(c => c.CreationDate != null && RoundDateToSeconds(c.CreationDate).Equals(dateValue))
                                : changesets.Where(c => c.CreationDate == null);
                            break;
                        case FilterExpressionType.GreaterThan:
                            changesets = dateParameter != null 
                                ? changesets.Where(c => c.CreationDate != null && RoundDateToSeconds(c.CreationDate) > dateValue)
                                : changesets.Where(c => c.CreationDate != null);
                            break;
                        case FilterExpressionType.GreaterThanOrEqual:
                            changesets = dateParameter != null 
                                ? changesets.Where(c => c.CreationDate != null && RoundDateToSeconds(c.CreationDate) >= dateValue)
                                : changesets.Where(c => c.CreationDate != null);
                            break;
                        case FilterExpressionType.LessThan:
                            changesets = dateParameter != null
                                ? changesets.Where(c => c.CreationDate != null && RoundDateToSeconds(c.CreationDate) < dateValue)
                                : changesets.Where(c => c.CreationDate != null);
                            break;
                        case FilterExpressionType.LessThanOrEqual:
                            changesets = dateParameter != null 
                                ? changesets.Where(c => c.CreationDate != null && RoundDateToSeconds(c.CreationDate) <= dateValue)
                                : changesets.Where(c => c.CreationDate != null);
                            break;
                        default:
                            throw new DataServiceException(501, "Not Implemented", "Changeset Creation Date can only be filtered with an equality, greater than, less than, greater than or equal and less than or equal operators", "en-US", null);
                    }
                }
            }

            return changesets;
        }

        private IEnumerable<Changeset> RequestChangesetsByProjectCollection(FilterNode rootFilterNode, int topRequestValue)
        {
            var css = this.TfsConnection.GetService<Microsoft.TeamFoundation.Server.ICommonStructureService3>();
            var teamProjects = css.ListAllProjects();

            return teamProjects.SelectMany(p => this.GetChangesetsByProject(p.Name, rootFilterNode, topRequestValue));
        }

        private IEnumerable<Changeset> RequestChangesetsByProject(string projectName, FilterNode rootFilterNode, int topRequestValue)
        {
            var versionControlServer = this.TfsConnection.GetService<TeamFoundation.VersionControl.Client.VersionControlServer>();
            var preliminaryChangesets = versionControlServer.QueryHistory(
                GetRequestedPath(null, projectName, rootFilterNode),
                TeamFoundation.VersionControl.Client.VersionSpec.Latest,
                0,
                TeamFoundation.VersionControl.Client.RecursionType.Full,
                GetRequestedCommitter(rootFilterNode),
                GetRequestedVersionFrom(rootFilterNode),
                GetRequestedVersionTo(rootFilterNode),
                topRequestValue,
                false,
                false,
                false);

            return FilterChangesets(preliminaryChangesets.Cast<TeamFoundation.VersionControl.Client.Changeset>().Select(c => c.ToModel(this.GetTfsWebAccessArtifactUrl(c.ArtifactUri))), rootFilterNode).ToArray();
        }

        private IEnumerable<Changeset> RequestChangesetsByBranch(string path, FilterNode rootFilterNode, int topRequestValue)
        {
            var versionControlServer = this.TfsConnection.GetService<TeamFoundation.VersionControl.Client.VersionControlServer>();

            var preliminaryChangesets = versionControlServer.QueryHistory(
                GetRequestedPath(path, null, rootFilterNode),
                TeamFoundation.VersionControl.Client.VersionSpec.Latest,
                0,
                TeamFoundation.VersionControl.Client.RecursionType.Full,
                GetRequestedCommitter(rootFilterNode),
                GetRequestedVersionFrom(rootFilterNode),
                GetRequestedVersionTo(rootFilterNode),
                topRequestValue,
                false,
                false,
                false);

            return FilterChangesets(preliminaryChangesets.Cast<TeamFoundation.VersionControl.Client.Changeset>().Select(c => c.ToModel(this.GetTfsWebAccessArtifactUrl(c.ArtifactUri))), rootFilterNode).ToArray();
        }

        private IEnumerable<Changeset> RequestChangesetsByBuild(string projectName, string buildDefinition, string buildNumber)
        {
            var buildServer = this.TfsConnection.GetService<TeamFoundation.Build.Client.IBuildServer>();

            var build = buildServer.QueryBuilds(projectName, buildDefinition)
                .SingleOrDefault(b => b.BuildNumber.Equals(buildNumber, StringComparison.OrdinalIgnoreCase));

            var versionControlServer = this.TfsConnection.GetService<TeamFoundation.VersionControl.Client.VersionControlServer>();

            return build != null
                    ? TeamFoundation.Build.Client.InformationNodeConverters.GetAssociatedChangesets(build)
                                    .Select(s => versionControlServer.GetChangeset(s.ChangesetId, false, false)).Select(c => c.ToModel(this.GetTfsWebAccessArtifactUrl(c.ArtifactUri))).ToArray()
                    : null;
        }

        private string GetChangesetsByProjectCollectionKey(FilterNode rootFilterNode, int topRequestValue)
        {
            return string.Format(CultureInfo.InvariantCulture, "TFSChangesetProxy.GetChangesetsByProjectCollection_{0}_{1}", this.GetFilterNodeKey(rootFilterNode), topRequestValue);
        }

        private string GetChangesetsByProjectKey(string projectName, FilterNode rootFilterNode, int topRequestValue)
        {
            return string.Format(CultureInfo.InvariantCulture, "TFSChangesetProxy.GetChangesetsByProject_{0}_{1}_{2}", projectName, this.GetFilterNodeKey(rootFilterNode), topRequestValue);
        }

        private string GetChangesetsByBranchKey(string path, FilterNode rootFilterNode, int topRequestValue)
        {
            return string.Format(CultureInfo.InvariantCulture, "TFSChangesetProxy.GetChangesetsByBranch_{0}_{1}_{2}", path, this.GetFilterNodeKey(rootFilterNode), topRequestValue);
        }

        private string GetChangesetsByBuildKey(string projectName, string buildDefinition, string buildNumber)
        {
            return string.Format(CultureInfo.InvariantCulture, "TFSChangesetProxy.GetChangesetsByBuild_{0}_{1}_{2}", projectName, buildDefinition, buildNumber);
        }
    }
}