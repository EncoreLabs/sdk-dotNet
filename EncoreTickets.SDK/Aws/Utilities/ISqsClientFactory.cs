using Amazon.SQS;

namespace EncoreTickets.SDK.Aws.Utilities
{
    public interface ISqsClientFactory
    {
        IAmazonSQS CreateClient(string profileName, string regionName);
    }
}