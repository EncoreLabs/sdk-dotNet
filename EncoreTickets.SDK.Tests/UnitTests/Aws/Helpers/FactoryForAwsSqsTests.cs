using Amazon.Extensions.NETCore.Setup;
using Amazon.SQS;

namespace EncoreTickets.SDK.Tests.UnitTests.Aws.Helpers
{
    internal class FactoryForAwsSqsTests : BaseFactoryForAwsSqsTests
    {
        public override IAmazonSQS CreateAmazonSqsClient(AWSOptions options)
        {
            return Mockers.ClientMock.Object;
        }
    }
}