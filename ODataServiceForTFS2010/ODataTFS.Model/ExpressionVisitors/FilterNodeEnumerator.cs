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

namespace Microsoft.Samples.DPE.ODataTFS.Model.ExpressionVisitors
{
    using System.Collections.Generic;

    public sealed class FilterNodeEnumerator : IEnumerator<FilterNode>
    {
        private readonly FilterNode rootNode;
        private FilterNode currentNode;

        public FilterNodeEnumerator(FilterNode rootNode)
        {
            this.rootNode = rootNode;
        }

        public FilterNode Current
        {
            get
            {
                return this.currentNode;
            }
        }

        object System.Collections.IEnumerator.Current
        {
            get
            {
                return this.currentNode;
            }
        }

        public bool MoveNext()
        {
            if (this.currentNode == null && this.rootNode != null)
            {
                this.currentNode = this.rootNode;
                return true;
            }
            else if (this.currentNode != null && this.currentNode.NextNode != null)
            {
                this.currentNode = this.currentNode.NextNode;
                return true;
            }

            return false;
        }

        public void Reset()
        {
            this.currentNode = this.rootNode;
        }

        public void Dispose()
        {
        }
    }
}
