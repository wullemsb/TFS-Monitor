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

namespace Microsoft.Samples.DPE.ODataTFS.Mobile.ViewModels
{
    using Microsoft.Samples.DPE.ODataTFS.Model.Entities;

    public class WorkItemListViewModel : ListViewModel<WorkItem>
    {
        public WorkItemListViewModel()
        {
        }

        public WorkItemListViewModel(string path)
            : base(path)
        {
        }

        public override string GetNoResultsFoundMessage()
        {
            return "No Work Items were found. Please, update your query and try again.";
        }

        public override string GetEntityCollectionName()
        {
            return "WorkItems";
        }
    }
}
