using System.Threading;
using System.Threading.Tasks;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime.CredentialManagement;
using Amazon.SQS;
using Amazon.SQS.Model;
using EncoreTickets.SDK.Aws;
using Moq;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.UnitTests.Aws
{
    internal class AwsSqsTests : AwsSqs
    {
        public static AwsSqsMockers Mockers;

        static AwsSqsTests()
        {
            Mockers = new AwsSqsMockers();
        }

        public AwsSqsTests() : base("profile_name", "region_name")
        {
        }

        protected override ICredentialProfileStore CreateCredentialProfileStore()
        {
            return Mockers.SharedCredentialsFileMock.Object;
        }

        protected override IAmazonSQS CreateAmazonSqsClient(AWSOptions options)
        {
            return Mockers.ClientMock.Object;
        }

        [TestCase("dev", "eu-west-1")]
        public void Constructor_IfProfileAndRegionArguments_InitializesClient(string profileName, string regionName)
        {
            AwsSqsWrapper.Mockers = new AwsSqsMockers();

            var aws = new AwsSqsWrapper(profileName, regionName);

            AwsSqsWrapper.Mockers.SharedCredentialsFileMock.Verify(
                x => x.RegisterProfile(It.IsAny<CredentialProfile>()), Times.Never());
            Assert.NotNull(aws.SourceClient);
            Assert.NotNull(aws.OptionsUsedForCreatingAmazonSqs);
            Assert.AreEqual(profileName, aws.OptionsUsedForCreatingAmazonSqs.Profile);
            Assert.AreEqual(regionName, aws.OptionsUsedForCreatingAmazonSqs.Region.SystemName);
        }

        [TestCase("dev", "eu-west-1", "access_key", "secret_key")]
        public void Constructor_IfProfileAndRegionAndKeysArguments_RegistersProfileAndInitializesClient(
            string profileName, string regionName, string accessKey, string secretKey)
        {
            AwsSqsWrapper.Mockers = new AwsSqsMockers();

            var aws = new AwsSqsWrapper(profileName, regionName, accessKey, secretKey);

            AwsSqsWrapper.Mockers.SharedCredentialsFileMock.Verify(x => x.RegisterProfile(It.Is<CredentialProfile>(pr =>
                pr.Name == profileName && pr.Options.AccessKey == accessKey && pr.Options.SecretKey == secretKey)), Times.Once);
            Assert.NotNull(aws.SourceClient);
            Assert.NotNull(aws.OptionsUsedForCreatingAmazonSqs);
            Assert.AreEqual(profileName, aws.OptionsUsedForCreatingAmazonSqs.Profile);
            Assert.AreEqual(regionName, aws.OptionsUsedForCreatingAmazonSqs.Region.SystemName);
        }

        [TestCase("url", "some message")]
        public async Task SendMessageAsync_(string queueUrl, string messageBody)
        {
            var result = await SendMessageAsync(queueUrl, messageBody);

            Mockers.ClientMock.Verify(x =>
                x.SendMessageAsync(
                    It.Is<SendMessageRequest>(arg => arg.QueueUrl == queueUrl && arg.MessageBody == messageBody),
                    It.Is<CancellationToken>(arg => arg == default)), Times.Once);
        }
    }
}
