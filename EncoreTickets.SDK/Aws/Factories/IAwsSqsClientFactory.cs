using Amazon.Extensions.NETCore.Setup;
using Amazon.SQS;

namespace EncoreTickets.SDK.Aws.Factories
{
    public interface IAwsSqsClientFactory
    {
        IAmazonSQS CreateAmazonSqsClient(AWSOptions options);
    }
}