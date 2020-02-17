using Amazon;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Amazon.SQS;
using EncoreTickets.SDK.Aws.Utilities;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.UnitTests.Aws
{
    internal class AmazonSqsClientFactoryTests : AmazonSqsClientFactory
    {
        private AWSOptions optionsUsedForCreatingAmazonSqs;

        protected override IAmazonSQS CreateAmazonSqsClient(AWSOptions options)
        {
            optionsUsedForCreatingAmazonSqs = options;
            return new AmazonSQSClient(new AnonymousAWSCredentials(), RegionEndpoint.APEast1);
        }

        [TestCase("dev", "eu-west-1")]
        public void CreateClient_InitializesCorrectly(string profileName, string regionName)
        {
            var actual = CreateClient(profileName, regionName);

            Assert.NotNull(optionsUsedForCreatingAmazonSqs);
            Assert.AreEqual(profileName, optionsUsedForCreatingAmazonSqs.Profile);
            Assert.AreEqual(regionName, optionsUsedForCreatingAmazonSqs.Region.SystemName);
        }
    }
}
