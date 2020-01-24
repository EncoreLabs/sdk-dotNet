using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime.CredentialManagement;
using Amazon.SQS;

namespace EncoreTickets.SDK.Aws.Factories
{
    internal class AwsSqsFactory : IFactoryForAwsSqs
    {
        public ICredentialProfileStore CreateCredentialProfileStore()
        {
            return new SharedCredentialsFile();
        }

        public IAmazonSQS CreateAmazonSqsClient(AWSOptions options)
        {
            return options.CreateServiceClient<IAmazonSQS>();
        }
    }
}