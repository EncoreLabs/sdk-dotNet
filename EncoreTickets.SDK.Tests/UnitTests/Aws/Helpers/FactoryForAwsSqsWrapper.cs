using Amazon;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Amazon.SQS;

namespace EncoreTickets.SDK.Tests.UnitTests.Aws.Helpers
{
    internal class FactoryForAwsSqsWrapper : BaseFactoryForAwsSqsTests
    {
        public override IAmazonSQS CreateAmazonSqsClient(AWSOptions options)
        {
            OptionsUsedForCreatingAmazonSqs = options;
            return new AmazonSQSClient(new AnonymousAWSCredentials(), RegionEndpoint.APEast1);
        }
    }
}