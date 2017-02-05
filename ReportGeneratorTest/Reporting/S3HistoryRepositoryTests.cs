using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Palmmedia.ReportGenerator.Reporting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Palmmedia.ReportGenerator.Reporting.Tests
{
    [TestClass()]
    public class S3HistoryRepositoryTests
    {
        [TestMethod()]
        public void S3HistoryRepositoryTest()
        {
            var mockClient = new Mock<ITransferUtility>();
            var x = new S3HistoryRepository(mockClient.Object);
        }
        [TestMethod()]
        public void S3HistoryRepositoryTest2()
        {
            var mockClient = new Mock<IAmazonS3>();
            var x = new S3HistoryRepository(mockClient.Object);
        }

        [TestMethod()]
        public void RestoreTest()
        {
            var mockClient = new Mock<ITransferUtility>();
            var sut = new S3HistoryRepository(mockClient.Object);
            var request = new TransferUtilityDownloadDirectoryRequest
            {
                BucketName = "bucket1",
                S3Directory = "somedirectory",
                LocalDirectory = "C:\\temp"
            };
            mockClient.Setup(p => p.DownloadDirectory(It.IsAny<TransferUtilityDownloadDirectoryRequest>())).Verifiable();
            sut.Restore(request.BucketName, request.S3Directory, request.LocalDirectory);
            mockClient.Verify();
        }

        [TestMethod()]
        public void SaveTest()
        {
            var mockClient = new Mock<ITransferUtility>();
            var sut = new S3HistoryRepository(mockClient.Object);
            var request = new TransferUtilityUploadDirectoryRequest
            {
                BucketName = "bucket1",
                KeyPrefix = "somedirectory",
                Directory = "C:\\temp"
            };
            mockClient.Setup(p => p.UploadDirectory(It.IsAny<TransferUtilityUploadDirectoryRequest>())).Verifiable();
            sut.Save(request.Directory, request.BucketName, request.KeyPrefix);
            mockClient.Verify();
        }
    }
}