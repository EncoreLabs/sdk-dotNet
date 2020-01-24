using System.Threading;
using System.Threading.Tasks;
using Amazon.Runtime.CredentialManagement;
using Amazon.SQS.Model;
using EncoreTickets.SDK.Aws;
using EncoreTickets.SDK.Tests.UnitTests.Aws.Helpers;
using Moq;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.UnitTests.Aws
{
    internal class AwsSqsTests : AwsSqs
    {
        private static readonly FactoryForAwsSqsTests SourceFactory;

        static AwsSqsTests()
        {
            SourceFactory = new FactoryForAwsSqsTests();
        }

        public AwsSqsTests() : base(SourceFactory, "profile_name", "region_name")
        {
        }

        [TestCase("dev", "eu-west-1")]
        public void Constructor_IfProfileAndRegionArguments_InitializesClient(string profileName, string regionName)
        {
            var factory = new FactoryForAwsSqsWrapper();

            var aws = new AwsSqsWrapper(factory, profileName, regionName);

            factory.Mockers.SharedCredentialsFileMock.Verify(
                x => x.RegisterProfile(It.IsAny<CredentialProfile>()), Times.Never());
            Assert.NotNull(aws.SourceClient);
            Assert.NotNull(factory.OptionsUsedForCreatingAmazonSqs);
            Assert.AreEqual(profileName, factory.OptionsUsedForCreatingAmazonSqs.Profile);
            Assert.AreEqual(regionName, factory.OptionsUsedForCreatingAmazonSqs.Region.SystemName);
        }

        [TestCase("dev", "eu-west-1", "access_key", "secret_key")]
        public void Constructor_IfProfileAndRegionAndKeysArguments_RegistersProfileAndInitializesClient(
            string profileName, string regionName, string accessKey, string secretKey)
        {
            var factory = new FactoryForAwsSqsWrapper();

            var aws = new AwsSqsWrapper(factory, profileName, regionName, accessKey, secretKey);

            factory.Mockers.SharedCredentialsFileMock.Verify(x => x.RegisterProfile(It.Is<CredentialProfile>(pr =>
                pr.Name == profileName && pr.Options.AccessKey == accessKey && pr.Options.SecretKey == secretKey)), Times.Once);
            Assert.NotNull(aws.SourceClient);
            Assert.NotNull(factory.OptionsUsedForCreatingAmazonSqs);
            Assert.AreEqual(profileName, factory.OptionsUsedForCreatingAmazonSqs.Profile);
            Assert.AreEqual(regionName, factory.OptionsUsedForCreatingAmazonSqs.Region.SystemName);
        }

        [TestCase("url", "some message")]
        public async Task SendMessageAsync_(string queueUrl, string messageBody)
        {
            var result = await SendMessageAsync(queueUrl, messageBody);

            SourceFactory.Mockers.ClientMock.Verify(x =>
                x.SendMessageAsync(
                    It.Is<SendMessageRequest>(arg => arg.QueueUrl == queueUrl && arg.MessageBody == messageBody),
                    It.Is<CancellationToken>(arg => arg == default)), Times.Once);
        }
    }
}
