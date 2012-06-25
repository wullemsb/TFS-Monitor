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
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Dispatcher;
    using System.Web;

    public class TFSInspector : IDispatchMessageInspector
    {
        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            if (!request.Properties.ContainsKey("UriTemplateMatchResults"))
            {
                return null;
            }

            var uriMatch = (UriTemplateMatch)request.Properties["UriTemplateMatchResults"];
            if (uriMatch.RelativePathSegments.Count > 0)
            {
                var collection = uriMatch.RelativePathSegments[0];
                request.Properties.Add("TfsCollectionName", collection);
                request.Properties["MicrosoftDataServicesRootUri"] = new Uri(uriMatch.BaseUri, collection);
            }

            return null;
        }

        public void BeforeSendReply(ref Message reply, object correlationState)
        {
            // nothing to do
        }
    }
}