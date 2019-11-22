using System.Threading.Tasks;
using Amazon;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime.CredentialManagement;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace EncoreTickets.SDK.Aws
{
    public class AwsSqs : IAwsSqs
    {
        private readonly IAmazonSQS client;

        public AwsSqs(string profileName, string regionName)
        {
            client = CreateClient(profileName, regionName);
        }

        public AwsSqs(string profileName, string regionName, string accessKey, string secretKey)
        {
            RegisterProfile(profileName, accessKey, secretKey);
            client = CreateClient(profileName, regionName);
        }

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
