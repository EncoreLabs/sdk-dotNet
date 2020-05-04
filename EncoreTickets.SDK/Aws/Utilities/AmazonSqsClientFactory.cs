using Amazon;
using Amazon.Extensions.NETCore.Setup;
using Amazon.SQS;

namespace EncoreTickets.SDK.Aws.Utilities
{
    public class AmazonSqsClientFactory : ISqsClientFactory
    {
        public IAmazonSQS CreateClient(string profileName, string regionName)
        {
            var options = new AWSOptions
            {
                Profile = profileName,
                Region = RegionEndpoint.GetBySystemName(regionName)
            };
            return CreateAmazonSqsClient(options);
        }

        protected virtual IAmazonSQS CreateAmazonSqsClient(AWSOptions options)
        {
            return options.CreateServiceClient<IAmazonSQS>();
        }
    }
}