using System;
using System.Threading.Tasks;
using Amazon.Extensions.NETCore.Setup;
using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Configuration;

namespace EncoreTickets.ConsoleTester
{
    static class AwsTester
    {
        public static async Task TestAws(IConfiguration configuration)
        {
            var sqsClient = SetupClient(configuration);
            await TestSendMessage(sqsClient, configuration["AWS_SQS:QueueUrl"]);
        }

        private static IAmazonSQS SetupClient(IConfiguration configuration)
        {
            var options = configuration.GetAWSOptions();
            return options.CreateServiceClient<IAmazonSQS>();
        }

        private static async Task TestSendMessage(IAmazonSQS client, string queueUrl)
        {
            Console.WriteLine();
            Console.WriteLine(" ========================================================== ");
            Console.WriteLine(" Test: Send message");
            Console.WriteLine(" ========================================================== ");

            var sqsRequest = new SendMessageRequest
            {
                QueueUrl = queueUrl,
                MessageBody = "{\"reference\": \"0\"}"
            };
            var sqsResponse = await client.SendMessageAsync(sqsRequest);

            Console.WriteLine($"Status code: {sqsResponse.HttpStatusCode}");
        }
    }
}
