using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime.CredentialManagement;
using Amazon.SQS;
using EncoreTickets.SDK.Aws.Factories;

namespace EncoreTickets.SDK.Tests.UnitTests.Aws.Helpers
{
    internal abstract class BaseFactoryForAwsSqsTests : IFactoryForAwsSqs
    {
        public AwsSqsMockers Mockers;

        public AWSOptions OptionsUsedForCreatingAmazonSqs;

        protected BaseFactoryForAwsSqsTests()
        {
            Mockers = new AwsSqsMockers();
        }

        public abstract IAmazonSQS CreateAmazonSqsClient(AWSOptions options);

        public ICredentialProfileStore CreateCredentialProfileStore()
        {
            return Mockers.SharedCredentialsFileMock.Object;
        }
    }
}