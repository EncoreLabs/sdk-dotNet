using System.Threading.Tasks;
using Amazon;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime.CredentialManagement;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace EncoreTickets.SDK.Aws
{
    /// <inheritdoc cref="IAwsSqs"/>
    /// <summary>
    /// The default implementation of the IAwsSqs interface.
    /// </summary>
    public class AwsSqs : IAwsSqs
    {
        protected IAmazonSQS Client { get; }

        /// <summary>
        /// Constructor that instantiates this class by the name of an AWS profile and the name of an AWS region.
        /// </summary>
        /// <param name="profileName">The AWS profile name.</param>
        /// <param name="regionName">The requested AWS region name.</param>
        public AwsSqs(string profileName, string regionName)
        {
            Client = CreateClient(profileName, regionName);
        }

        /// <summary>
        /// Constructor that instantiates this class by the name of an AWS profile and the name of an AWS region.
        /// The specified AWS profile is created or updated with the specified credentials.
        /// </summary>
        /// <param name="profileName">The AWS profile name.</param>
        /// <param name="regionName">The requested AWS region name.</param>
        /// <param name="accessKey">The AWS access key to set to the profile.</param>
        /// <param name="secretKey">The AWS secret key to set to the profile.</param>
        public AwsSqs(string profileName, string regionName, string accessKey, string secretKey)
        {
            RegisterProfile(profileName, accessKey, secretKey);
            Client = CreateClient(profileName, regionName);
        }

        /// <inheritdoc cref="IAwsSqs"/>
        public async Task<SendMessageResponse> SendMessageAsync(string queueUrl, string messageBody)
        {
            var sqsRequest = new SendMessageRequest
            {
                QueueUrl = queueUrl,
                MessageBody = messageBody
            };
            return await Client.SendMessageAsync(sqsRequest);
        }

        protected virtual ICredentialProfileStore CreateCredentialProfileStore()
        {
            return new SharedCredentialsFile();
        }

        protected virtual IAmazonSQS CreateAmazonSqsClient(AWSOptions options)
        {
            return options.CreateServiceClient<IAmazonSQS>();
        }

        private void RegisterProfile(string profileName, string accessKey, string secretKey)
        {
            var options = new CredentialProfileOptions
            {
                AccessKey = accessKey,
                SecretKey = secretKey
            };
            var profile = new CredentialProfile(profileName, options);
            var credentialsFile = CreateCredentialProfileStore();
            credentialsFile.RegisterProfile(profile);
        }

        private IAmazonSQS CreateClient(string profileName, string regionName)
        {
            var options = new AWSOptions
            {
                Profile = profileName,
                Region = RegionEndpoint.GetBySystemName(regionName)
            };
            return CreateAmazonSqsClient(options);
        }
    }
}
