using Amazon;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using Amazon.SQS;
using EncoreTickets.SDK.Aws;

namespace EncoreTickets.SDK.Tests.UnitTests.Aws
{
    internal class AwsSqsWrapper : AwsSqs
    {
        public static AwsSqsMockers Mockers;

        public AWSOptions OptionsUsedForCreatingAmazonSqs;

        public IAmazonSQS SourceClient => Client;

        public AwsSqsWrapper(string profileName, string regionName) : base(profileName, regionName)
        {
        }

        public AwsSqsWrapper(string profileName, string regionName, string accessKey, string secretKey)
            : base(profileName, regionName, accessKey, secretKey)
        {
        }

        protected override ICredentialProfileStore CreateCredentialProfileStore()
        {
            return Mockers.SharedCredentialsFileMock.Object;
        }

        protected override IAmazonSQS CreateAmazonSqsClient(AWSOptions options)
        {
            OptionsUsedForCreatingAmazonSqs = options;
            return new AmazonSQSClient(new AnonymousAWSCredentials(), RegionEndpoint.APEast1);
        }
    }
}