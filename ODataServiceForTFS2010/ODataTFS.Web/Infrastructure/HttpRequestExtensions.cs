﻿// ----------------------------------------------------------------------------------
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

namespace Microsoft.Samples.DPE.ODataTFS.Web.Infrastructure
{
    using System;
    using System.Web;

    public static class HttpRequestExtensions
    {
        public static bool IsRootCollectionListRequest(this HttpRequest request, int segmentCount)
        {
            var path = request.Path;
            if (!request.ApplicationPath.Equals("/", StringComparison.OrdinalIgnoreCase))
            {
                path = path.Replace(request.ApplicationPath, string.Empty).Replace("//", "/");
            }

            if (string.IsNullOrEmpty(path) || path.Equals("/"))
            {
                return true;
            }

            return path.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries).Length == segmentCount;
        }
    }
}