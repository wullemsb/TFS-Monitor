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
    using System.Xml;
    using Microsoft.Samples.DPE.ODataTFS.Model.Entities;
    using Microsoft.TeamFoundation.Proxy;
    using Microsoft.TeamFoundation.Server;

    public class TFSAreaPathProxy : TFSBaseProxy, ITFSAreaPathProxy
    {
        public TFSAreaPathProxy(Uri uri, ICredentials credentials)
            : base(uri, credentials)
        {
        }

        public IEnumerable<AreaPath> GetAllAreaPaths()
        {
            if (HttpContext.Current.Items[this.GetAllAreaPathsKey()] == null)
            {
                HttpContext.Current.Items[this.GetAllAreaPathsKey()] = this.RequestAllAreaPaths();
            }

            return (IEnumerable<AreaPath>)HttpContext.Current.Items[this.GetAllAreaPathsKey()];
        }

        public IEnumerable<AreaPath> GetAreaPathsByProject(string projectName)
        {
            if (HttpContext.Current.Items[this.GetAreaPathsByProjectKey(projectName)] == null)
            {
                HttpContext.Current.Items[this.GetAreaPathsByProjectKey(projectName)] = this.RequestAllAreaPathsByProject(projectName);
            }

            return (IEnumerable<AreaPath>)HttpContext.Current.Items[this.GetAreaPathsByProjectKey(projectName)];
        }

        public IEnumerable<AreaPath> GetSubAreas(string rootAreaName)
        {
            if (HttpContext.Current.Items[rootAreaName] == null)
            {
                HttpContext.Current.Items[rootAreaName] = this.RequestSubAreas(rootAreaName);
            }

            return (IEnumerable<AreaPath>)HttpContext.Current.Items[rootAreaName];
        }

        public IEnumerable<AreaPath> RequestAllAreaPaths()
        {
            var css = this.TfsConnection.GetService<ICommonStructureService3>();
            var allStructures = css.ListAllProjects().SelectMany(p => css.ListStructures(p.Uri));
            var areaPathsXml = css.GetNodesXml(allStructures.Where(s => s.StructureType.Equals(StructureType.ProjectModelHierarchy)).Select(a => a.Uri).ToArray(), true);
            var rootAreaPaths = areaPathsXml.ChildNodes.Cast<XmlNode>().Where(a => a.FirstChild != null).SelectMany(a => a.FirstChild.ChildNodes.Cast<XmlNode>().SelectMany(c => this.ParseAreaPathFromNodes(c)));

            return ExtractAllAreaPaths(rootAreaPaths).ToArray();
        }

        public IEnumerable<AreaPath> RequestAllAreaPathsByProject(string projectName)
        {
            var css = this.TfsConnection.GetService<ICommonStructureService3>();
            var allStructures = css.ListStructures(css.GetProjectFromName(projectName).Uri);
            var areaPathsXml = css.GetNodesXml(allStructures.Where(s => s.StructureType.Equals(StructureType.ProjectModelHierarchy)).Select(a => a.Uri).ToArray(), true);
            var rootAreaPaths = areaPathsXml.ChildNodes.Cast<XmlNode>().Where(a => a.FirstChild != null).SelectMany(a => a.FirstChild.ChildNodes.Cast<XmlNode>().SelectMany(c => this.ParseAreaPathFromNodes(c)));

            return ExtractAllAreaPaths(rootAreaPaths).ToArray();
        }

        public IEnumerable<AreaPath> RequestSubAreas(string rootAreaName)
        {
            var css = this.TfsConnection.GetService<ICommonStructureService3>();
            var allStructures = css.ListAllProjects().SelectMany(p => css.ListStructures(p.Uri));
            var areaPathsXml = css.GetNodesXml(allStructures.Where(s => s.StructureType.Equals(StructureType.ProjectModelHierarchy)).Select(a => a.Uri).ToArray(), true);
            var areas = ExtractAllAreaPaths(areaPathsXml.ChildNodes.Cast<XmlNode>().Where(a => a.FirstChild != null).SelectMany(a => a.FirstChild.ChildNodes.Cast<XmlNode>().SelectMany(c => this.ParseAreaPathFromNodes(c))));
  
            var encodedPath = EntityTranslator.EncodePath(string.Format(CultureInfo.InvariantCulture, "{0}\\", rootAreaName.TrimEnd('\\')));
            if (areas.SingleOrDefault(a => a.Path.Equals(rootAreaName.TrimEnd('\\'))) == null)
            {
                throw new DataServiceException(404, "Not Found", string.Format(CultureInfo.InvariantCulture, "The AreaPath specified could not be found: {0}", rootAreaName), "en-US", null);
            }

            return areas.Where(a => a.Path.StartsWith(encodedPath, StringComparison.OrdinalIgnoreCase)).ToArray();
        }             

        private static IEnumerable<AreaPath> ExtractAllAreaPaths(IEnumerable<AreaPath> areas)
        {
            var allAreas = new List<AreaPath>();
            if (areas != null)
            {
                allAreas.AddRange(areas.SelectMany(a => ExtractAllAreaPaths(a.SubAreas)));
                allAreas.AddRange(areas);
            }

            return allAreas;
        }

        private IEnumerable<AreaPath> ParseAreaPathFromNodes(XmlNode currentNode)
        {
            var results = new List<AreaPath>();
            var subAreas = default(IEnumerable<AreaPath>);

            if (currentNode.ChildNodes != null)
            {
                var childrenNode = currentNode.ChildNodes.Cast<XmlNode>().SingleOrDefault(n => n.Name.Equals("Children"));
                if (childrenNode != null)
                {
                    subAreas = childrenNode.ChildNodes.Cast<XmlNode>().SelectMany(n => this.ParseAreaPathFromNodes(n));
                }
            }

            if (currentNode.Attributes["Name"] != null && currentNode.Attributes["Path"] != null)
            {
                results.Add(currentNode.ToModel(subAreas));
            }

            return results;
        }

        private string GetAllAreaPathsKey()
        {
            return string.Format(CultureInfo.InvariantCulture, "TFSAreaPathProxy.GetAllAreaPaths");
        }

        private string GetAreaPathsByProjectKey(string projectName)
        {
            return string.Format(CultureInfo.InvariantCulture, "TFSAreaPathProxy.GetAreaPathByProject_{0}", projectName);            
        }
    }
}
