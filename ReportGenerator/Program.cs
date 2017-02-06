using System;
using System.Linq;
using Palmmedia.ReportGenerator.Reporting;
using Amazon.S3;
using Amazon;
using Palmmedia.ReportGenerator.AWS;

namespace Palmmedia.ReportGenerator
{
    /// <summary>
    /// Command line access to the ReportBuilder.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// The main method.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        /// <returns>Return code indicating success/failure.</returns>
        internal static int Main(string[] args)
        {
            var reportConfigurationBuilder = new ReportConfigurationBuilder(new MefReportBuilderFactory());
            if (args.Length < 2)
            {
                reportConfigurationBuilder.ShowHelp();
                return 1;
            }
            args = args.Select(a => a.EndsWith("\"", StringComparison.OrdinalIgnoreCase) ? a.TrimEnd('\"') + "\\" : a).ToArray();

            var config = reportConfigurationBuilder.Create(args);
            if (config.IsS3HistoryNeeded)
            {
                var client = string.IsNullOrEmpty(config.AWSAccessKey) ? new AmazonS3Client(config.AWSRegion) : new AmazonS3Client(config.AWSAccessKey, config.AWSSecret, config.AWSRegion);
                var s3HistoryRepository = new S3HistoryRepository(client);
                s3HistoryRepository.Restore(config.S3HistoryBucketName, config.S3HistoryBucketName, config.HistoryDirectory);
            }
            var generator = new Generator();
            var rtn = generator.GenerateReport(config);
            if (config.IsS3HistoryNeeded)
            {
                var client = string.IsNullOrEmpty(config.AWSAccessKey) ? new AmazonS3Client(config.AWSRegion) : new AmazonS3Client(config.AWSAccessKey, config.AWSSecret, config.AWSRegion);
                var s3HistoryRepository = new S3HistoryRepository(client);
                s3HistoryRepository.Save(config.HistoryDirectory, config.S3HistoryBucketName, config.S3HistoryDirectoryName);
            }
            return rtn ? 0 : 1;
        }
    }
}
