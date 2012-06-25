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

namespace Microsoft.Samples.DPE.ODataTFS.Mobile.Helpers
{
    using System.IO;
    using System.IO.IsolatedStorage;
    using System.Xml.Serialization;
    using Microsoft.Samples.DPE.ODataTFS.Mobile.ViewModels;

    public static class IsolatedStorageHelper
    {
        public static void ClearCredentials(string fileName)
        {            
            using (var isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                // Check if file exits
                if (isf.FileExists(fileName))
                {
                    isf.DeleteFile(fileName);
                }
            }
        }

        public static void SaveCredentials(LoginViewModel viewModel, string fileName)
        {
            using (var isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                // If user choose to save, create a new file
                using (var fs = isf.CreateFile(fileName))
                {
                    // and serialize data
                    XmlSerializer serializer = new XmlSerializer(typeof(LoginViewModel));
                    serializer.Serialize(fs, viewModel);
                }
            }
        }

        public static LoginViewModel ReadCredentials(string fileName)
        {
            // Try to load previously saved data from IsolatedStorage
            using (var isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                // check if file exits
                if (isf.FileExists(fileName))
                {
                    using (var fs = isf.OpenFile(fileName, FileMode.Open))
                    {
                        // Read the file contents and try to deserialize it back to data object
                        XmlSerializer ser = new XmlSerializer(typeof(LoginViewModel));
                        object obj = ser.Deserialize(fs);

                        // If successfully deserialized, initialize data object variable with it
                        if ((obj != null) && (obj is LoginViewModel))
                        {
                            return obj as LoginViewModel;
                        }
                        else
                        {
                            return new LoginViewModel();
                        }
                    }
                }
                else
                {
                    // If previous data not found, create new instance
                    return new LoginViewModel();
                }
            }
        }
    }
}
