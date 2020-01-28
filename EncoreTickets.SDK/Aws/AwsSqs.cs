using System.Threading.Tasks;
using Amazon.SQS;
using Amazon.SQS.Model;
using EncoreTickets.SDK.Aws.Utilities;

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
        /// <param name="clientFactory">The factory for creating of AmazonSQS clients</param>
        /// <param name="profileName">The AWS profile name.</param>
        /// <param name="regionName">The requested AWS region name.</param>
        public AwsSqs(ISqsClientFactory clientFactory, string profileName, string regionName)
        {
            Client = clientFactory.CreateClient(profileName, regionName);
        }

        /// <summary>
        /// Constructor that instantiates this class by the name of an AWS profile and the name of an AWS region.
        /// The specified AWS profile is created or updated with the specified credentials.
        /// </summary>
        /// <param name="clientFactory">The factory for creating of AmazonSQS clients</param>
        /// <param name="profileRegistrar">The registrar of Amazon profiles in the system</param>
        /// <param name="profileName">The AWS profile name.</param>
        /// <param name="regionName">The requested AWS region name.</param>
        /// <param name="accessKey">The AWS access key to set to the profile.</param>
        /// <param name="secretKey">The AWS secret key to set to the profile.</param>
        public AwsSqs(
            ISqsClientFactory clientFactory,
            IProfileRegistrar profileRegistrar,
            string profileName,
            string regionName,
            string accessKey,
            string secretKey)
        {
            profileRegistrar.RegisterProfile(profileName, accessKey, secretKey);
            Client = clientFactory.CreateClient(profileName, regionName);
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
    }
}
