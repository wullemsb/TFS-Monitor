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

namespace Microsoft.Samples.DPE.ODataTFS.Web.Infrastructure
{
    using System;
    using System.Net;
    using System.Web;
    using Microsoft.Samples.DPE.ODataTFS.Model.Serialization;

    public sealed class TFSAuthProvider : IAuthProvider
    {
        private readonly Uri tfsUri;

        public TFSAuthProvider(Uri tfsServerUri)
        {
            this.tfsUri = tfsServerUri;
        }

        public bool IsValidUser(string userName, string password, string domain, out IBasicUser user)
        {
            user = new BasicUser
                {
                    Domain = domain,
                    UserName = userName,
                    Password = password
                };

            return true;
        }

        public bool IsRequestAllowed(HttpRequest request, IBasicUser user)
        {
            if (user != null)
            {
                return true;
            }

            return false;
        }

        public void Dispose()
        {
            // This is intentional, since we don't have any resources to free in this very simple sample IAuthProvider
        }
    }
}
