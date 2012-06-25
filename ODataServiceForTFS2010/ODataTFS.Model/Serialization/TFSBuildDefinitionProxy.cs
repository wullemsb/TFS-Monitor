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
    using Entities;
    using Microsoft.TeamFoundation.Build.Client;

    public class TFSBuildDefinitionProxy : TFSBaseProxy, ITFSBuildDefinitionProxy
    {
        public TFSBuildDefinitionProxy(Uri uri, ICredentials credentials)
            : base(uri, credentials)
        {
        }

        public IEnumerable<BuildDefinition> GetBuildDefinitionsByProject(string projectName)
        {
            var key = GetBuildDefinitionsByProjectKey(projectName);

            if (HttpContext.Current.Items[key] == null)
            {
                HttpContext.Current.Items[key] = this.RequestBuildDefinitionsByProject(projectName);
            }

            return (IEnumerable<BuildDefinition>)HttpContext.Current.Items[key];
        }

        public void QueueBuild(string projectName, string defintionName)
        {
            var buildServer = TfsConnection.GetService<IBuildServer>();
            var spec = CreateBuildDefinitionSpec(buildServer, projectName);
            var preliminaryResults = buildServer.QueryBuildDefinitions(spec).Definitions.Where(bd => bd.Name == defintionName).FirstOrDefault();

            if (preliminaryResults == null)
            {
                throw new DataServiceException(404, "Not Found", string.Format(CultureInfo.InvariantCulture, "The Build Definition specified could not be found (Project: {0}, Definition: {1})", projectName, defintionName), "en-US", null);
            }

            buildServer.QueueBuild(preliminaryResults);
        }

        private static IBuildDefinitionSpec CreateBuildDefinitionSpec(IBuildServer buildServer, string projectName)
        {
            var detailSpec = buildServer.CreateBuildDefinitionSpec(projectName);
            return detailSpec;
        }

        private static string GetBuildDefinitionsByProjectKey(string projectName)
        {
            return string.Format(CultureInfo.InvariantCulture, "TFSBuildProxy.GetBuildDefinitionsByProject_{0}", projectName);
        }

        private IEnumerable<BuildDefinition> RequestBuildDefinitionsByProject(string projectName)
        {
            var buildServer = TfsConnection.GetService<IBuildServer>();

            var spec = CreateBuildDefinitionSpec(buildServer, projectName);
            var preliminaryResults = buildServer.QueryBuildDefinitions(spec).Definitions.Select(b => b.ToModel());

            return preliminaryResults.ToArray();
        }
    }
}
