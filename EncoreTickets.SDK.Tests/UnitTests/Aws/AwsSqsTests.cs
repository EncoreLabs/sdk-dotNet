using System.Threading;
using System.Threading.Tasks;
using Amazon.SQS;
using Amazon.SQS.Model;
using EncoreTickets.SDK.Aws;
using EncoreTickets.SDK.Aws.Utilities;
using Moq;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.UnitTests.Aws
{
    internal class AwsSqsTests
    {
        [TestCase("dev", "eu-west-1")]
        public void Constructor_IfProfileAndRegionArguments_InitializesClient(string profileName, string regionName)
        {
            var clientFactoryMock = new Mock<ISqsClientFactory>(MockBehavior.Strict);
            clientFactoryMock
                .Setup(x => x.CreateClient(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(() => null);

            var aws = new AwsSqs(clientFactoryMock.Object, profileName, regionName);

            clientFactoryMock.Verify(
                x => x.CreateClient(It.Is<string>(arg => arg == profileName), It.Is<string>(arg => arg == regionName)),
                Times.Once);
        }

        [TestCase("dev", "eu-west-1", "access_key", "secret_key")]
        public void Constructor_IfProfileAndRegionAndKeysArguments_RegistersProfileAndInitializesClient(
            string profileName, string regionName, string accessKey, string secretKey)
        {
            var profileRegistrarMock = new Mock<IProfileRegistrar>(MockBehavior.Strict);
            profileRegistrarMock.Setup(x =>
                x.RegisterProfile(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));
            var clientFactoryMock = new Mock<ISqsClientFactory>(MockBehavior.Strict);
            clientFactoryMock
                .Setup(x => x.CreateClient(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(() => null);

            var aws = new AwsSqs(clientFactoryMock.Object, profileRegistrarMock.Object, profileName, regionName, accessKey, secretKey);

            profileRegistrarMock.Verify(
                x => x.RegisterProfile(
                    It.Is<string>(arg => arg == profileName),
                    It.Is<string>(arg => arg == accessKey),
                    It.Is<string>(arg => arg == secretKey)),
                Times.Once);
            clientFactoryMock.Verify(
                x => x.CreateClient(It.Is<string>(arg => arg == profileName), It.Is<string>(arg => arg == regionName)),
                Times.Once);
        }

        [TestCase("url", "some message")]
        public async Task SendMessageAsync_(string queueUrl, string messageBody)
        {
            var amazonSqsMock = new Mock<IAmazonSQS>(MockBehavior.Strict);
            amazonSqsMock
                .Setup(x => x.SendMessageAsync(It.IsAny<SendMessageRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((SendMessageResponse)null);
            var clientFactoryMock = new Mock<ISqsClientFactory>(MockBehavior.Strict);
            clientFactoryMock
                .Setup(x => x.CreateClient(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(() => amazonSqsMock.Object);
            var aws = new AwsSqs(clientFactoryMock.Object, It.IsAny<string>(), It.IsAny<string>());

            var result = await aws.SendMessageAsync(queueUrl, messageBody);

            amazonSqsMock.Verify(
                x =>
                x.SendMessageAsync(
                    It.Is<SendMessageRequest>(arg => arg.QueueUrl == queueUrl && arg.MessageBody == messageBody),
                    It.Is<CancellationToken>(arg => arg == default)), Times.Once);
        }
    }
}
