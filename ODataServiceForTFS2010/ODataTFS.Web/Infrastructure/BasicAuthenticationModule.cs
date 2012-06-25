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
    using System.Globalization;
    using System.Security.Principal;
    using System.Text;
    using System.Web;
    using Microsoft.Data.Services.Toolkit;

    public sealed class BasicAuthenticationModule : IHttpModule
    {
        private readonly IAuthProvider authProvider;

        public BasicAuthenticationModule()
        {
            var provider = ConfigReader.GetConfigValue("ODataTFS.AuthProvider");
            var tfsServer = ConfigReader.GetConfigValue("ODataTFS.TfsServer");

            var providerType = Type.GetType(provider, true);
            var tfsServerUri = new Uri(tfsServer, UriKind.Absolute);

            this.authProvider = Activator.CreateInstance(providerType, new[] { tfsServerUri }) as IAuthProvider;
        }

        public void Init(HttpApplication context)
        {
            if (context != null)
            {
                context.AuthenticateRequest += this.AuthenticateRequest;
                context.BeginRequest += this.BeginRequest;
            }
        }

        public void Dispose()
        {
            if (this.authProvider != null)
            {
                this.authProvider.Dispose();
            }
        }

        private static void SendAuthHeader(HttpApplication context)
        {
            var tfsServerUri = new Uri(ConfigReader.GetConfigValue("ODataTFS.TfsServer"), UriKind.Absolute);

            context.Response.Clear();
            context.Response.ContentType = "application/xml";
            context.Response.StatusCode = 401;
            context.Response.StatusDescription = "Unauthorized";
            context.Response.AddHeader("WWW-Authenticate", string.Format(CultureInfo.InvariantCulture, "Basic realm=\"{0}\"", tfsServerUri.AbsoluteUri));
            context.Response.AddHeader("DataServiceVersion", "1.0");
            context.Response.Write(@"<?xml version=""1.0"" encoding=""utf-8"" standalone=""yes"" ?>
                                     <error xmlns=""http://schemas.microsoft.com/ado/2007/08/dataservices/metadata"">
                                         <code>Not authorized</code>
                                         <message xml:lang=""en-US"">Please provide valid TFS credentials (domain\\username and password)</message>
                                     </error>");
            context.Response.End();
        }

        private void BeginRequest(object sender, EventArgs e)
        {
            var context = sender as HttpApplication;
            if (context.User == null)
            {
                if (!context.Request.IsMetadataRequest() && !context.Request.IsRootCollectionListRequest(1) && !this.TryAuthenticate(context))
                {
                    SendAuthHeader(context);
                }
            }
        }

        private void AuthenticateRequest(object sender, EventArgs e)
        {
            var context = sender as HttpApplication;

            this.TryAuthenticate(context);
        }

        private bool TryAuthenticate(HttpApplication context)
        {
            string user = null;
            string password = null;
            string domain = null;
            string authHeader = context.Request.Headers["Authorization"];
            if (!string.IsNullOrEmpty(authHeader))
            {
                if (authHeader.StartsWith("basic ", StringComparison.InvariantCultureIgnoreCase))
                {
                    var userNameAndPassword = Encoding.Default.GetString(Convert.FromBase64String(authHeader.Substring(6)));
                    var userAndPasswordArray = userNameAndPassword.Split(new[] { ':' }, 2, StringSplitOptions.RemoveEmptyEntries);
                    if (userAndPasswordArray != null && userAndPasswordArray.Length >= 2)
                    {
                        var userAndDomainArray = userAndPasswordArray[0].Trim().Split(new[] { '\\' }, 2, StringSplitOptions.RemoveEmptyEntries);
                        password = userAndPasswordArray[1];

                        if (userAndDomainArray.Length >= 2)
                        {
                            domain = userAndDomainArray[0];
                            user = userAndDomainArray[1];
                        }
                        else
                        {
                            user = userAndDomainArray[0];
                        }
                    }
                    else
                    {
                        return false;
                    }

                    IBasicUser bu = null;
                    if (this.authProvider.IsValidUser(user, password, domain, out bu))
                    {
                        context.Context.User = new GenericPrincipal(bu, new string[] { });
                        if (!this.authProvider.IsRequestAllowed(context.Request, bu))
                        {
                            return false;
                        }

                        return true;
                    }
                }
            }

            return false;
        }
    }
}