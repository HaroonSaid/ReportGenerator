using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amazon.S3;
using Amazon;
using System.Threading.Tasks;
using Amazon.S3.Transfer;
using Amazon.Runtime;

namespace Palmmedia.ReportGenerator.Reporting
{
    public class S3HistoryRepository
    {
        private readonly ITransferUtility _transferUtility;
        public S3HistoryRepository(ITransferUtility transferUtility)
        {
            _transferUtility = transferUtility;
        }
        public S3HistoryRepository(IAmazonS3 client)
        {
            _transferUtility = new TransferUtility(client);
        }
        public void Save(string directory, string bucketName, string s3DirectoryName)
        {
            if (string.IsNullOrWhiteSpace(directory))
                return;

            if (string.IsNullOrWhiteSpace(bucketName))
                throw new ArgumentNullException(nameof(bucketName));

            if (string.IsNullOrWhiteSpace(s3DirectoryName))
                throw new ArgumentNullException(nameof(s3DirectoryName));

            var request = new TransferUtilityUploadDirectoryRequest
            {
                BucketName = bucketName,
                Directory = directory,
                KeyPrefix = s3DirectoryName
            };
            _transferUtility.UploadDirectory(request);
        }
        internal void Restore(string bucketNaeme, string s3Directory, string directory)
        {
            if (string.IsNullOrWhiteSpace(bucketNaeme))
                throw new ArgumentNullException(nameof(bucketNaeme));
            if (string.IsNullOrWhiteSpace(directory))
                throw new ArgumentNullException(nameof(directory));
            if (string.IsNullOrWhiteSpace(s3Directory))
                throw new ArgumentNullException(nameof(s3Directory));
            var request = new TransferUtilityDownloadDirectoryRequest
            {
                BucketName = bucketNaeme,
                S3Directory = s3Directory,
                LocalDirectory = directory,
            };
            _transferUtility.DownloadDirectory(request);
        }
    }
}
