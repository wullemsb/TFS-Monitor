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

namespace Microsoft.Samples.DPE.ODataTFS.Web
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Web;
    using System.Web.Routing;
    using Microsoft.Data.Services.Toolkit;

    public class Global : HttpApplication
    {
        protected void Application_Error(object sender, EventArgs e)
        {
            var lastError = this.Server.GetLastError();

            Trace.TraceError(lastError.ToString());
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            var requestPath = Request.AppRelativeCurrentExecutionFilePath;
            if (requestPath.Equals("~/", StringComparison.OrdinalIgnoreCase))
            {
                HttpContext.Current.Server.Transfer("~/Default.aspx");
            }

            var queryString = Request.QueryString;
            if (requestPath.EndsWith("Builds", StringComparison.InvariantCultureIgnoreCase))
            {
                var filterParameter = queryString.Get("$filter");
                if (!string.IsNullOrEmpty(filterParameter))
                {
                    var filtersByProject = this.FiltersByProperties(filterParameter, "Project");
                    var filtersByDefinition = this.FiltersByProperties(filterParameter, "Definition");
                    var filtersByNumber = this.FiltersByProperties(filterParameter, "Number");
                    var filtersByNonKeyAttributes = this.FiltersByProperties(filterParameter, "Reason", "Quality", "Status", "RequestedBy", "RequestedFor", "LastChangedBy", "StartTime", "FinishTime", "LastChangedOn", "BuildFinished", "DropLocation", "Errors", "Warnings");
                    if ((filtersByProject || filtersByDefinition || filtersByNumber) && (!filtersByProject || !filtersByDefinition || !filtersByNumber) && !filtersByNonKeyAttributes)
                    {
                        Response.RedirectPermanent(string.Format(CultureInfo.InvariantCulture, "{0}{1} and StartTime ne datetime'0001-01-01T00:00:00.000'", Request.Url.LocalPath, Request.Url.Query));
                    }
                }
            }
            else if (requestPath.EndsWith("Changes", StringComparison.InvariantCultureIgnoreCase))
            {
                var filterParameter = queryString.Get("$filter");
                if (!string.IsNullOrEmpty(filterParameter))
                {
                    var filtersByChangeset = this.FiltersByProperties(filterParameter, "Changeset");
                    var filtersByPath = this.FiltersByProperties(filterParameter, "Path");
                    var filtersByNonKeyAttributes = this.FiltersByProperties(filterParameter, "Collection", "ChangeType", "Type");
                    if ((filtersByChangeset ^ filtersByPath) && !filtersByNonKeyAttributes)
                    {
                        Response.RedirectPermanent(string.Format(CultureInfo.InvariantCulture, "{0}{1} and Collection ne ''", Request.Url.LocalPath, Request.Url.Query));
                    }
                }
            }
        }

        private bool FiltersByProperties(string filter, params string[] fields)
        {
            foreach (var fieldName in fields)
            {
                if (filter.Contains(fieldName))
                {
                    int nextFieldPos = filter.IndexOf(fieldName, 0, StringComparison.OrdinalIgnoreCase);
                    while (nextFieldPos >= 0)
                    {
                        var literalValue = false;
                        var nextQuoteStringPos = filter.IndexOf('\'', 0);
                        while (nextQuoteStringPos >= 0 && nextQuoteStringPos < nextFieldPos)
                        {
                            literalValue = !literalValue;
                            nextQuoteStringPos = filter.IndexOf('\'', nextQuoteStringPos + 1);
                        }

                        if (!literalValue)
                        {
                            return true;
                        }

                        nextFieldPos = filter.IndexOf(fieldName, nextFieldPos + 1, StringComparison.OrdinalIgnoreCase);
                    }
                }
            }

            return false;
        }

        private void Application_Start(object sender, EventArgs e)
        {
            RouteTable.Routes.AddDataServiceRoute<TFSService>();
        }
    }
}