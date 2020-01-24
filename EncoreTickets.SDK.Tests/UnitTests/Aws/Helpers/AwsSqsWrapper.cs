using Amazon.SQS;
using EncoreTickets.SDK.Aws;
using EncoreTickets.SDK.Aws.Factories;

namespace EncoreTickets.SDK.Tests.UnitTests.Aws.Helpers
{
    internal class AwsSqsWrapper : AwsSqs
    {
        public IAmazonSQS SourceClient => Client;

        public AwsSqsWrapper(IFactoryForAwsSqs factory, string profileName, string regionName)
            : base(factory, profileName, regionName)
        {
        }

        public AwsSqsWrapper(IFactoryForAwsSqs factory, string profileName, string regionName, string accessKey, string secretKey)
            : base(factory, profileName, regionName, accessKey, secretKey)
        {
        }
    }
}