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

namespace Microsoft.Samples.DPE.ODataTFS.Model.Tests.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Services;
    using System.Linq;
    using Microsoft.Samples.DPE.ODataTFS.Model.Entities;
    using Microsoft.Samples.DPE.ODataTFS.Model.Repositories;
    using Microsoft.Samples.DPE.ODataTFS.Model.Serialization;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class AttachmentRepositoryTests
    {
        [TestMethod]
        [ExpectedException(typeof(DataServiceException))]
        public void ItShouldThrowNotSupportedInGetAll()
        {
            var mockProxy = new Mock<ITFSAttachmentProxy>();
            var repository = new AttachmentRepository(mockProxy.Object);

            repository.GetAll();
        }

        [TestMethod]
        public void ItShouldGetOneAttachment()
        {
            var mockProxy = new Mock<ITFSAttachmentProxy>();

            var expectedResult = new Attachment
            {
                Id = "29161-0",
                WorkItemId = 29161,
                Index = 0,
                Name = "Sample File 1.txt",
                Extension = "txt",
                Comment = "Sample Comment 1",
                Length = 1024,
                AttachedTime = DateTime.Now,
                CreationTime = DateTime.Now,
                LastWriteTime = DateTime.Now,
                Uri = "http://sampleuri/"
            };

            mockProxy.Setup(p => p.GetAttachment(It.Is<int>(id => id == 29161), It.Is<int>(index => index == 0)))
                .Returns(expectedResult)
                .Verifiable();

            var repository = new AttachmentRepository(mockProxy.Object);

            var result = repository.GetOne("29161-0");

            Assert.AreEqual(result, expectedResult);
            mockProxy.VerifyAll();
        }

        [TestMethod]
        public void ItShouldGetOneEmptyAttachmentIsIdStartsWithTemp()
        {
            var mockProxy = new Mock<ITFSAttachmentProxy>();
            var repository = new AttachmentRepository(mockProxy.Object);
            var tempId = "temp-123456789";

            var result = repository.GetOne(tempId);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Id, tempId);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ItShouldThrowIfAttachmenIdIsNotWellFormatted1()
        {
            var mockProxy = new Mock<ITFSAttachmentProxy>();
            var repository = new AttachmentRepository(mockProxy.Object);
            var badId = "123456789";

            var result = repository.GetOne(badId);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ItShouldThrowIfAttachmenIdIsNotWellFormatted2()
        {
            var mockProxy = new Mock<ITFSAttachmentProxy>();
            var repository = new AttachmentRepository(mockProxy.Object);
            var badId = "123-456-789";

            var result = repository.GetOne(badId);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ItShouldThrowIfWorkItemIdIsNotANumberInGetOneAttachment()
        {
            var mockProxy = new Mock<ITFSAttachmentProxy>();
            var repository = new AttachmentRepository(mockProxy.Object);
            var badId = "NAN-0";
            
            repository.GetOne(badId);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ItShouldThrowIfIndexIsNotANumberInGetOneAttachment()
        {
            var mockProxy = new Mock<ITFSAttachmentProxy>();
            var repository = new AttachmentRepository(mockProxy.Object);
            var badId = "29161-NAN";

            repository.GetOne(badId);
        }

        [TestMethod]
        public void ItShouldGetAttachmentsForAGivenWorkItem()
        {
            var mockProxy = new Mock<ITFSAttachmentProxy>();
            var expectedResult = new List<Attachment>
                {
                    new Attachment { Id = "29161-0", WorkItemId = 29161, Index = 0, Name = "Sample File 1.txt", Extension = "txt", Comment = "Sample Comment 1", Length = 1024 },
                    new Attachment { Id = "29161-1", WorkItemId = 29161, Index = 1, Name = "Sample File 2.txt", Extension = "txt", Comment = "Sample Comment 2", Length = 1024 }
                };

            mockProxy.Setup(p => p.GetAttachmentsByWorkItem(It.Is<int>(id => id == 29161)))
                .Returns(expectedResult)
                .Verifiable();

            var repository = new AttachmentRepository(mockProxy.Object);

            var results = repository.GetAttachmentsByWorkItem("29161");

            Assert.AreEqual(expectedResult.Count, results.Count());
            Assert.IsTrue(results.SequenceEqual(expectedResult));
            mockProxy.VerifyAll();
        }
    }
}
