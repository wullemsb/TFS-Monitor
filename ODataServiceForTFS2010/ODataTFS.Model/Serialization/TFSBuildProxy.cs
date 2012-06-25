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
    using Microsoft.TeamFoundation.Framework.Common;

    public class TFSBuildProxy : TFSBaseProxy, ITFSBuildProxy
    {
        public TFSBuildProxy(Uri uri, ICredentials credentials)
            : base(uri, credentials)
        {
        }

        public Build GetBuild(string projectName, string buildDefinition, string buildNumber)
        {
            var buildServer = this.TfsConnection.GetService<TeamFoundation.Build.Client.IBuildServer>();
            var definitionBuilds = buildServer.QueryBuilds(projectName, buildDefinition).Select(b => b.ToModel());

            return definitionBuilds.SingleOrDefault(b => b.Number.Equals(buildNumber, StringComparison.OrdinalIgnoreCase));
        }

        public IEnumerable<Build> GetBuildsByProject(string projectName, FilterNode rootFilterNode)
        {
            var key = this.GetBuildsByProjectKey(projectName, rootFilterNode);

            if (HttpContext.Current.Items[key] == null)
            {
                HttpContext.Current.Items[key] = this.RequestBuildsByProject(projectName, rootFilterNode);
            }

            return (IEnumerable<Build>)HttpContext.Current.Items[key];
        }

        public IEnumerable<Build> GetBuildsByProjectCollection(FilterNode rootFilterNode)
        {
            var key = this.GetBuildsByProjectCollectionKey(rootFilterNode);

            if (HttpContext.Current.Items[key] == null)
            {
                HttpContext.Current.Items[key] = this.RequestBuildsByProjectCollection(rootFilterNode);
            }

            return (IEnumerable<Build>)HttpContext.Current.Items[key];
        }

        private static TeamFoundation.Build.Client.IBuildDetailSpec CreateBuildDetailSpec(TeamFoundation.Build.Client.IBuildServer buildServer, string projectName, FilterNode rootFilterNode)
        {
            var detailSpec = default(TeamFoundation.Build.Client.IBuildDetailSpec);

            if (rootFilterNode != null)
            {   
                var definitionSpec = buildServer.CreateBuildDefinitionSpec(projectName);
                var definition = rootFilterNode.SingleOrDefault(p => p.Key.Equals("Definition", StringComparison.OrdinalIgnoreCase));
                var number = rootFilterNode.SingleOrDefault(p => p.Key.Equals("Number", StringComparison.OrdinalIgnoreCase));
                var quality = rootFilterNode.SingleOrDefault(p => p.Key.Equals("Quality", StringComparison.OrdinalIgnoreCase));
                var reason = rootFilterNode.SingleOrDefault(p => p.Key.Equals("Reason", StringComparison.OrdinalIgnoreCase));
                var status = rootFilterNode.SingleOrDefault(p => p.Key.Equals("Status", StringComparison.OrdinalIgnoreCase));
                var requestedFor = rootFilterNode.SingleOrDefault(p => p.Key.Equals("RequestedFor", StringComparison.OrdinalIgnoreCase));

                if (definition != null)
                {
                    if (definition.Sign != FilterExpressionType.Equal)
                    {
                        throw new DataServiceException(501, "Not Implemented", "Build Definition can only be filtered with an equality operator", "en-US", null);
                    }

                    definitionSpec.Name = definition.Value;
                }

                detailSpec = buildServer.CreateBuildDetailSpec(definitionSpec);
                if (number != null)
                {
                    if (number.Sign != FilterExpressionType.Equal)
                    {
                        throw new DataServiceException(501, "Not Implemented", "Build Number can only be filtered with an equality operator", "en-US", null);
                    }

                    detailSpec.BuildNumber = number.Value;
                }

                if (quality != null)
                {
                    if (quality.Sign != FilterExpressionType.Equal)
                    {
                        throw new DataServiceException(501, "Not Implemented", "Build Quality can only be filtered with an equality operator", "en-US", null);
                    }

                    detailSpec.Quality = quality.Value;
                }

                if (requestedFor != null)
                {
                    if (requestedFor.Sign != FilterExpressionType.Equal)
                    {
                        throw new DataServiceException(501, "Not Implemented", "Requested For can only be filtered with an equality operator", "en-US", null);
                    }

                    detailSpec.RequestedFor = requestedFor.Value;
                }

                if (status != null)
                {
                    if (status.Sign != FilterExpressionType.Equal)
                    {
                        throw new DataServiceException(501, "Not Implemented", "Build Status can only be filtered with an equality operator", "en-US", null);
                    }

                    var statusValue = default(TeamFoundation.Build.Client.BuildStatus);
                    if (Enum.TryParse<TeamFoundation.Build.Client.BuildStatus>(status.Value, out statusValue))
                    {
                        detailSpec.Status = statusValue;
                    }
                }

                if (reason != null)
                {
                    if (reason.Sign != FilterExpressionType.Equal)
                    {
                        throw new DataServiceException(501, "Not Implemented", "Build Reason can only be filtered with an equality operator", "en-US", null);
                    }

                    var reasonValue = default(TeamFoundation.Build.Client.BuildReason);
                    if (Enum.TryParse<TeamFoundation.Build.Client.BuildReason>(reason.Value, out reasonValue))
                    {
                        detailSpec.Reason = reasonValue;
                    }
                }

                return detailSpec;
            }
            else
            {
                detailSpec = buildServer.CreateBuildDetailSpec(projectName);
            }

            return detailSpec;
        }

        private static IEnumerable<Build> FilterBuilds(IEnumerable<Build> builds, FilterNode rootFilterNode)
        {
            if (rootFilterNode != null)
            {
                var startTimeParameter = rootFilterNode.SingleOrDefault(p => p.Key.Equals("StartTime"));
                var finishTimeParameter = rootFilterNode.SingleOrDefault(p => p.Key.Equals("FinishTime"));
                var buildFinishedParameter = rootFilterNode.SingleOrDefault(p => p.Key.Equals("BuildFinished"));
                var requestedByParameter = rootFilterNode.SingleOrDefault(p => p.Key.Equals("RequestedBy"));
                var startTimeValue = default(DateTime);

                if (startTimeParameter != null && DateTime.TryParse(startTimeParameter.Value, out startTimeValue))
                {
                    switch (startTimeParameter.Sign)
                    {
                        case FilterExpressionType.Equal:
                            builds = builds.Where(b => b.StartTime.Equals(startTimeValue));
                            break;

                        case FilterExpressionType.NotEqual:
                            builds = builds.Where(b => !b.StartTime.Equals(startTimeValue));
                            break;

                        case FilterExpressionType.GreaterThan:
                            builds = builds.Where(b => b.StartTime > startTimeValue);
                            break;

                        case FilterExpressionType.LessThan:
                            builds = builds.Where(b => b.StartTime < startTimeValue);
                            break;

                        default:
                            throw new NotSupportedException("Start time field can only be filtered with equality, inequality, greater than and less than operators");
                    }
                }

                var finishTimeValue = default(DateTime);
                if (finishTimeParameter != null && DateTime.TryParse(finishTimeParameter.Value, out finishTimeValue))
                {
                    switch (finishTimeParameter.Sign)
                    {
                        case FilterExpressionType.Equal:
                            builds = builds.Where(b => b.FinishTime.Equals(finishTimeValue));
                            break;

                        case FilterExpressionType.NotEqual:
                            builds = builds.Where(b => !b.FinishTime.Equals(finishTimeValue));
                            break;

                        case FilterExpressionType.GreaterThan:
                            builds = builds.Where(b => b.FinishTime > finishTimeValue);
                            break;

                        case FilterExpressionType.LessThan:
                            builds = builds.Where(b => b.FinishTime < finishTimeValue);
                            break;

                        default:
                            throw new NotSupportedException("Finish time field can only be filtered with equality, inequality, greater than and less than operators");
                    }
                }

                var buildFinishedValue = default(bool);
                if (buildFinishedParameter != null && bool.TryParse(buildFinishedParameter.Value, out buildFinishedValue))
                {
                    switch (buildFinishedParameter.Sign)
                    {
                        case FilterExpressionType.Equal:
                            builds = builds.Where(b => b.BuildFinished == buildFinishedValue);
                            break;

                        case FilterExpressionType.NotEqual:
                            builds = builds.Where(b => b.BuildFinished != buildFinishedValue);
                            break;

                        default:
                            throw new NotSupportedException("Build Finished field can only be filtered with equality and inequality operators");
                    }
                }

                if (requestedByParameter != null)
                {
                    switch (requestedByParameter.Sign)
                    {
                        case FilterExpressionType.Equal:
                            builds = builds.Where(b => b.RequestedBy.Equals(requestedByParameter.Value));
                            break;

                        case FilterExpressionType.NotEqual:
                            builds = builds.Where(b => !b.RequestedBy.Equals(requestedByParameter.Value));
                            break;

                        default:
                            throw new NotSupportedException("Requested By field can only be filtered with equality and inequality operators");
                    }
                }
            }

            return builds;
        }

        private IEnumerable<Build> RequestBuildsByProjectCollection(FilterNode rootFilterNode)
        {
            var teamProjects = this.TfsConnection.CatalogNode.QueryChildren(
                    new Guid[] { CatalogResourceTypes.TeamProject },
                    false,
                    CatalogQueryOptions.None);

            IEnumerable<Build> preliminaryResults = null;
            var buildServer = this.TfsConnection.GetService<TeamFoundation.Build.Client.IBuildServer>();

            if (rootFilterNode != null && rootFilterNode.Count(p => p.Key.Equals("Project", StringComparison.OrdinalIgnoreCase)) > 0)
            {
                var projectParameter = rootFilterNode.SingleOrDefault(p => p.Key.Equals("Project", StringComparison.OrdinalIgnoreCase));
                var spec = CreateBuildDetailSpec(buildServer, projectParameter.Value, rootFilterNode);
                preliminaryResults = buildServer.QueryBuilds(spec).Builds.Select(b => b.ToModel());
            }
            else
            {
                var specs = teamProjects.Select(t => CreateBuildDetailSpec(buildServer, t.Resource.DisplayName, rootFilterNode));
                preliminaryResults = specs.Select(s => buildServer.QueryBuilds(s)).SelectMany(r => r.Builds).Select(b => b.ToModel());
            }

            return FilterBuilds(preliminaryResults, rootFilterNode).ToArray();
        }

        private IEnumerable<Build> RequestBuildsByProject(string projectName, FilterNode rootFilterNode)
        {
            var buildServer = this.TfsConnection.GetService<TeamFoundation.Build.Client.IBuildServer>();

            var spec = CreateBuildDetailSpec(buildServer, projectName, rootFilterNode);
            var preliminaryResults = buildServer.QueryBuilds(spec).Builds.Select(b => b.ToModel());

            return FilterBuilds(preliminaryResults, rootFilterNode).ToArray();
        }

        private string GetBuildsByProjectCollectionKey(FilterNode rootFilterNode)
        {
            return string.Format(CultureInfo.InvariantCulture, "TFSBuildProxy.GetBuildsByProjectCollection_{0}", this.GetFilterNodeKey(rootFilterNode));
        }

        private string GetBuildsByProjectKey(string projectName, FilterNode rootFilterNode)
        {
            return string.Format(CultureInfo.InvariantCulture, "TFSBuildProxy.GetBuildsByProject_{0}_{1}", projectName, this.GetFilterNodeKey(rootFilterNode));
        }
    }
}
