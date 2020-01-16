using System.Threading;
using Amazon.Runtime.CredentialManagement;
using Amazon.SQS;
using Amazon.SQS.Model;
using Moq;

namespace EncoreTickets.SDK.Tests.UnitTests.Aws
{
    internal class AwsSqsMockers
    {
        public Mock<ICredentialProfileStore> SharedCredentialsFileMock;
        public Mock<IAmazonSQS> ClientMock;

        public AwsSqsMockers()
        {
            SharedCredentialsFileMock = new Mock<ICredentialProfileStore>(MockBehavior.Strict);
            SharedCredentialsFileMock
                .Setup(x => x.RegisterProfile(It.IsAny<CredentialProfile>()))
                .Callback(() => { });
            ClientMock = new Mock<IAmazonSQS>(MockBehavior.Strict);
            ClientMock
                .Setup(x => x.SendMessageAsync(It.IsAny<SendMessageRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((SendMessageResponse)null);
        }
    }
}