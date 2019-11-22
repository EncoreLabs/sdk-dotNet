using System;
using System.Threading.Tasks;
using EncoreTickets.SDK.Aws;
using Microsoft.Extensions.Configuration;

namespace EncoreTickets.ConsoleTester
{
    static class AwsTester
    {
        public static async Task TestAws(IConfiguration configuration)
        {
            var awsSqs = CreateSqs(configuration);
            await TestSendMessage(awsSqs, configuration["AWS_SQS:QueueUrl"]);
        }

        private static IAwsSqs CreateSqs(IConfiguration configuration)
        {
            var options = configuration.GetAWSOptions();
            var profile = options.Profile;
            var region = options.Region.SystemName;
            var accessKey = configuration["AWS_SQS:Credentials:AccessKey"];
            var secretKey = configuration["AWS_SQS:Credentials:SecretKey"];
            return new AwsSqs(profile, region, accessKey, secretKey);
        }

        private static async Task TestSendMessage(IAwsSqs sqs, string queueUrl)
        {
            Console.WriteLine();
            Console.WriteLine(" ========================================================== ");
            Console.WriteLine(" Test: Send message");
            Console.WriteLine(" ========================================================== ");

            var sqsResponse = await sqs.SendMessageAsync(queueUrl, "testMessage");

            Console.WriteLine($"Status code: {sqsResponse.HttpStatusCode}");
        }
    }
}
