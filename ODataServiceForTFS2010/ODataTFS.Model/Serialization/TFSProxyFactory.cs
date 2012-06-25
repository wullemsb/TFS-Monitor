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
    using System.Net;

    public class TFSProxyFactory
    {
        private readonly ICredentials tfsCredentials;
        private readonly Uri tfsUri;

        public TFSProxyFactory(Uri tfsUri, ICredentials credentials)
        {
            this.tfsUri = tfsUri;
            this.tfsCredentials = credentials;
        }

        public ITFSAttachmentProxy TfsAttachmentProxy 
        {
            get { return new TFSAttachmentProxy(this.tfsUri, this.tfsCredentials); }
        }

        public ITFSBranchProxy TfsBranchProxy
        {
            get { return new TFSBranchProxy(this.tfsUri, this.tfsCredentials); }
        }

        public ITFSBuildProxy TfsBuildProxy
        {
            get { return new TFSBuildProxy(this.tfsUri, this.tfsCredentials); }
        }

        public ITFSBuildDefinitionProxy TfsBuildDefinitionProxy
        {
            get { return new TFSBuildDefinitionProxy(this.tfsUri, this.tfsCredentials); }
        }

        public ITFSChangeProxy TfsChangeProxy
        {
            get { return new TFSChangeProxy(this.tfsUri, this.tfsCredentials); }
        }

        public ITFSChangesetProxy TfsChangesetProxy
        {
            get { return new TFSChangesetProxy(this.tfsUri, this.tfsCredentials); }
        }

        public ITFSProjectProxy TfsProjectProxy
        {
            get { return new TFSProjectProxy(this.tfsUri, this.tfsCredentials); }
        }

        public ITFSQueryProxy TfsQueryProxy
        {
            get { return new TFSQueryProxy(this.tfsUri, this.tfsCredentials); }
        }

        public ITFSWorkItemProxy TfsWorkItemProxy
        {
            get { return new TFSWorkItemProxy(this.tfsUri, this.tfsCredentials); }
        }

        public ITFSAreaPathProxy TfsAreaPathProxy
        {
            get { return new TFSAreaPathProxy(this.tfsUri, this.tfsCredentials); }
        }
    }
}
