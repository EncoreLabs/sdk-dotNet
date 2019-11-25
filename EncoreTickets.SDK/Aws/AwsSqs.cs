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
        private readonly IAmazonSQS client;

        /// <summary>
        /// Constructor that instantiates this class by the name of an AWS profile and the name of an AWS region.
        /// </summary>
        /// <param name="profileName">The AWS profile name.</param>
        /// <param name="regionName">The requested AWS region name.</param>
        public AwsSqs(string profileName, string regionName)
        {
            client = CreateClient(profileName, regionName);
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
            client = CreateClient(profileName, regionName);
        }

        /// <inheritdoc cref="IAwsSqs"/>
        public async Task<SendMessageResponse> SendMessageAsync(string queueUrl, string messageBody)
        {
            var sqsRequest = new SendMessageRequest
            {
                QueueUrl = queueUrl,
                MessageBody = messageBody
            };
            return await client.SendMessageAsync(sqsRequest);
        }

        private void RegisterProfile(string profileName, string accessKey, string secretKey)
        {
            var options = new CredentialProfileOptions
            {
                AccessKey = accessKey,
                SecretKey = secretKey
            };
            var profile = new CredentialProfile(profileName, options);
            var netSdkFile = new SharedCredentialsFile();
            netSdkFile.RegisterProfile(profile);
        }

        private IAmazonSQS CreateClient(string profileName, string regionName)
        {
            var options = new AWSOptions
            {
                Profile = profileName,
                Region = RegionEndpoint.GetBySystemName(regionName)
            };
            return options.CreateServiceClient<IAmazonSQS>();
        }
    }
}
