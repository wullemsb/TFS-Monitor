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

namespace Microsoft.Samples.DPE.ODataTFS.ConsoleClient
{
    using System;
    using System.Collections.Specialized;
    using System.Data.Services.Client;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Windows.Forms;
    using Microsoft.Samples.DPE.ODataTFS.ConsoleClient.TFSProxy;

    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => { return true; };

            string baseUrl;
            string teamProject;
            string workItemType;
            var proxy = CreateTFSServiceProxy(out baseUrl, out teamProject, out workItemType);

            try
            {
                var workItemTitle = "Sample title for a new Work Item created through the OData service for TFS";

                Console.WriteLine("Creating a new Work Item...");
                if (!CreateNewWorkItem(proxy, teamProject, workItemType, workItemTitle))
                {
                    Console.WriteLine("There was an error when creating the new Work Item.");
                }
                else
                {
                    Console.WriteLine("New Work Item successfully created!");
                    Console.WriteLine();

                    var workItem = proxy.Execute<WorkItem>(new Uri(string.Format(
                                                                        CultureInfo.InvariantCulture,
                                                                        "{0}/Projects('{1}')/WorkItems?$filter=Type eq '{2}' and Title eq '{3}'&$orderby=Id desc",
                                                                        baseUrl,
                                                                        teamProject,
                                                                        workItemType,
                                                                        workItemTitle)))
                                          .First();

                    Console.WriteLine("Updating the Work Item with Id {0}...", workItem.Id);
                    if (!UpdateWorkItem(proxy, workItem))
                    {
                        Console.WriteLine("There was an error when updating the Work Item.");
                    }
                    else
                    {
                        Console.WriteLine("Work Item successfully updated!");
                        Console.WriteLine();

                        var comment = string.Format(CultureInfo.InvariantCulture, "Sample comment for a new Attachment for the Work Item {0}", workItem.Id);

                        Console.WriteLine("Creating a new Attachemnt for the Work Item {0}...", workItem.Id);
                        if (!CreateNewAttachment(proxy, workItem.Id, comment, PickFile()))
                        {
                            Console.WriteLine("There was an error when creating the new Attachment.");
                        }
                        else
                        {
                            Console.WriteLine("New Attachment successfully created!");
                        }
                    }
                }
                
                Console.WriteLine();
                Console.WriteLine("Press any key to finish...");
            }
            catch (Exception exception)
            {
                Console.WriteLine();
                Console.WriteLine(exception.Message);

                if (exception.InnerException != null)
                {
                    Console.WriteLine("Inner exception: {0}", exception.InnerException.Message);
                }
            }
            finally
            {
                Console.ReadKey();
            }
        }

        [STAThread]
        public static void MainTriggerBuild(string[] args)
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => { return true; };

            var projectCollection = ConfigReader.GetConfigValue("ODataTFS.TfsProjectCollection");
            var project = ConfigReader.GetConfigValue("ODataTFS.TfsTeamProject");

            var baseUrl = ConfigReader.GetConfigValue("ODataTFS.BaseUrl");
            if (baseUrl.EndsWith("/"))
            {
                baseUrl = baseUrl.Remove(baseUrl.Length - 1);
            }

            baseUrl = string.Format(CultureInfo.InvariantCulture, "{0}/{1}/TriggerBuild", baseUrl, projectCollection);

            Console.WriteLine("Enter the Build Definition to invoke for project {0}: ", project);
            var buildDefinition = Console.ReadLine();

            var client = new WebClient();

            var credentials = string.Format(
                CultureInfo.InvariantCulture,
                @"{0}\{1}:{2}",
                ConfigReader.GetConfigValue("ODataTFS.TfsDomain"),
                ConfigReader.GetConfigValue("ODataTFS.TfsUsername"),
                ConfigReader.GetConfigValue("ODataTFS.TfsPassword"));
            client.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(new ASCIIEncoding().GetBytes(credentials)));

            var parameters = new NameValueCollection();
            parameters.Add("project", project);
            parameters.Add("definition", buildDefinition);

            try
            {
                client.UploadValues(baseUrl, "POST", parameters);
                Console.WriteLine("The build definition was successfully triggered!");
            }
            catch (WebException e)
            {
                Console.WriteLine("There was an error when triggering the build definition: {0}", e.Message);
            }
            finally
            {
                Console.WriteLine();
                Console.WriteLine("Press any key to finish...");
                Console.ReadKey();
            }           
        }

        private static TFSData CreateTFSServiceProxy(out string baseUrl, out string teamProject, out string workItemType)
        {
            var projectCollection = ConfigReader.GetConfigValue("ODataTFS.TfsProjectCollection");

            baseUrl = ConfigReader.GetConfigValue("ODataTFS.BaseUrl");
            if (baseUrl.EndsWith("/"))
            {
                baseUrl = baseUrl.Remove(baseUrl.Length - 1);
            }

            baseUrl = string.Format(CultureInfo.InvariantCulture, "{0}/{1}", baseUrl, projectCollection);

            teamProject = ConfigReader.GetConfigValue("ODataTFS.TfsTeamProject");
            workItemType = ConfigReader.GetConfigValue("ODataTFS.WorkItemType");

            var proxy = new TFSData(new Uri(baseUrl));
            proxy.SendingRequest += (s, e) =>
            {
                var credentials = string.Format(
                    CultureInfo.InvariantCulture,
                    @"{0}\{1}:{2}",
                    ConfigReader.GetConfigValue("ODataTFS.TfsDomain"),
                    ConfigReader.GetConfigValue("ODataTFS.TfsUsername"),
                    ConfigReader.GetConfigValue("ODataTFS.TfsPassword"));
                e.RequestHeaders.Add("Authorization", "Basic " + Convert.ToBase64String(new ASCIIEncoding().GetBytes(credentials)));
            };

            return proxy;
        }

        private static bool CreateNewWorkItem(TFSData proxy, string teamProject, string workItemType, string title)
        {
            var success = true;
            var workItem = WorkItem.CreateWorkItem(0, 0, DateTime.Now, DateTime.Now);
            workItem.Project = teamProject;
            workItem.Type = workItemType;
            workItem.Title = title;
            workItem.AreaPath = teamProject;
            workItem.IterationPath = teamProject;
            workItem.Reason = "New";
            workItem.StackRank = "3.5";
            workItem.Priority = "2";
            workItem.Severity = "1 - Critical";
            workItem.Description = "Sample description for a new Work Item created through the OData service for TFS";
            workItem.ReproSteps = "Sample repro steps for a new Work Item created through the OData service for TFS";

            proxy.AddToWorkItems(workItem);
            var result = proxy.SaveChanges();

            Console.WriteLine("Batch status code: {0}", result.BatchStatusCode);

            foreach (var opeartionResponse in result)
            {
                Console.WriteLine("Operation status code: {0}", opeartionResponse.StatusCode);
                if (opeartionResponse.Error != null)
                {
                    Console.WriteLine("Operation error: {0}", opeartionResponse.Error.Message);
                    success = false;
                }
            }

            return success;
        }

        private static bool UpdateWorkItem(TFSData proxy, WorkItem workItem)
        {
            var success = true;
            workItem.Title = "Updated Title from the OData service for TFS";
            workItem.StackRank = "1.5";
            workItem.Priority = "2";
            workItem.Severity = "1 - Critical";
            workItem.Description = "Updated description from the OData service for TFS";
            workItem.ReproSteps = "Updated repro steps from the OData service for TFS";

            proxy.UpdateObject(workItem);
            var result = proxy.SaveChanges(System.Data.Services.Client.SaveChangesOptions.ReplaceOnUpdate);

            Console.WriteLine("Batch status code: {0}", result.BatchStatusCode);

            foreach (var opeartionResponse in result)
            {
                Console.WriteLine("Operation status code: {0}", opeartionResponse.StatusCode);
                if (opeartionResponse.Error != null)
                {
                    Console.WriteLine("Operation error: {0}", opeartionResponse.Error.Message);
                    success = false;
                }
            }

            return success;
        }

        private static bool CreateNewAttachment(TFSData proxy, int workItemId, string comment, string path)
        {
            var success = true;
            var attachment = Attachment.CreateAttachment(string.Empty, workItemId, 0, DateTime.Now, DateTime.Now, DateTime.Now, 0);

            attachment.Comment = comment;
            attachment.Name = Path.GetFileNameWithoutExtension(path);
            attachment.Extension = Path.GetExtension(path);

            DataServiceResponse response;
            using (var fileStream = new FileStream(path, FileMode.Open))
            {
                proxy.AddToAttachments(attachment);
                proxy.SetSaveStream(attachment, fileStream, false, RegistryHelper.GetMimeType(Path.GetExtension(path)), string.Empty);
                response = proxy.SaveChanges();
            }

            Console.WriteLine("Batch status code: {0}", response.BatchStatusCode);
            foreach (var opeartionResponse in response)
            {
                Console.WriteLine("Operation status code: {0}", opeartionResponse.StatusCode);
                if (opeartionResponse.Error != null)
                {
                    Console.WriteLine("Operation error: {0}", opeartionResponse.Error.Message);
                    success = false;
                }
            }

            return success;
        }

        private static string PickFile()
        {
            var path = string.Empty;
            using (var dialog = new OpenFileDialog { Title = "Select a file to attach", AddExtension = true, CheckPathExists = true, CheckFileExists = true })
            {
                while (dialog.ShowDialog() != DialogResult.OK)
                {
                }

                path = dialog.FileName;
            }

            return path;
        }
    }
}
