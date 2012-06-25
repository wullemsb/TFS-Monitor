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
    using System;
    using System.Xml.Linq;

    /// <summary>
    /// Helper class to de-serialize DataServiceExceptions thrown by an ADO.NET Data Service
    /// </summary>
    public static class DataServiceExceptionUtil
    {
        private static readonly string DataServicesMetadataNamespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata";

        private static XName codeXName = XName.Get("code", DataServicesMetadataNamespace);
        private static XName typeXName = XName.Get("type", DataServicesMetadataNamespace);
        private static XName messageXName = XName.Get("message", DataServicesMetadataNamespace);
        private static XName stackTraceXName = XName.Get("stacktrace", DataServicesMetadataNamespace);
        private static XName internalExceptionXName = XName.Get("internalexception", DataServicesMetadataNamespace);
        private static XName innerErrorXName = XName.Get("innererror", DataServicesMetadataNamespace);

        /// <summary>
        /// Pass in the Exception recieved from an Execute / SaveChanges call
        /// to rethrow the actual DataServiceException thrown by the ADO.NET Data Service
        /// </summary>
        /// <param name="exception">The Exception thrown by the client library
        /// in response to an Execute/SaveChanges call</param>
        public static void Throw(Exception exception)
        {
            var baseException = exception.GetBaseException();
            var doc = XDocument.Parse(baseException.Message);

            if (doc != null)
            {
                throw ParseException(doc.Root, true);
            }
        }

        /// <summary>
        /// Parses the Exception object to determine if it contains a DataServiceException
        /// and de-serializes the Exception message into a DataServiceException.
        /// </summary>
        /// <param name="exception">The Exception thrown by the client library
        /// in response to an Execute/SaveChanges call </param>
        /// <param name="dataServiceException">The DataServiceException thrown by the ADO.NET Data Service</param>
        /// <returns>true if we are able to parse the response into a DataServiceException,false if not</returns>
        public static bool TryParse(Exception exception, out Exception dataServiceException)
        {
            var parseSucceeded = false;

            try
            {
                var baseException = exception.GetBaseException();
                var doc = XDocument.Parse(baseException.Message);

                dataServiceException = ParseException(doc.Root, false);
                parseSucceeded = dataServiceException != null;
            }
            catch
            {
                dataServiceException = exception;
                parseSucceeded = false;
            }

            return parseSucceeded;
        }

        private static DataServiceException ParseException(XElement errorElement, bool throwOnFailure)
        {
            switch (errorElement.Name.LocalName)
            {
                case "error":
                case "innererror":
                    DataServiceException internalException = errorElement.Element(innerErrorXName) != null
                                        ? ParseException(errorElement.Element(innerErrorXName), throwOnFailure)
                                        : null;
                    string message = errorElement.Element(messageXName) != null
                                        ? errorElement.Element(messageXName).Value.ToString()
                                        : string.Empty;
                    string stackTrace = errorElement.Element(stackTraceXName) != null
                                        ? errorElement.Element(stackTraceXName).Value.ToString()
                                        : string.Empty;

                    return new DataServiceException(message, stackTrace, internalException);

                default:
                    if (throwOnFailure)
                    {
                        throw new InvalidOperationException("Could not parse Exception");
                    }
                    else
                    {
                        return null;
                    }
            }
        }
    }

    public class DataServiceException : Exception
    {
        private readonly string stackTrace;

        public DataServiceException(string message, string stackTrace, Exception internalException)
            : base(message, internalException)
        {
            this.stackTrace = stackTrace;
        }

        public override string StackTrace
        {
            get
            {
                return this.stackTrace;
            }
        }
    }
}
