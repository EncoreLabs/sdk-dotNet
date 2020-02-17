using Amazon.Runtime.CredentialManagement;
using EncoreTickets.SDK.Aws.Utilities;
using Moq;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.UnitTests.Aws
{
    internal class ProfileRegistrarTests : ProfileRegistrar
    {
        private Mock<ICredentialProfileStore> sharedCredentialsFileMock;

        protected override ICredentialProfileStore CreateCredentialProfileStore()
        {
            return sharedCredentialsFileMock.Object;
        }
        
        [TestCase("dev", "access_key", "secret_key")]
        public void RegisterProfile_RegistersWithCorrectParameters(string profileName, string accessKey, string secretKey)
        {
            sharedCredentialsFileMock = new Mock<ICredentialProfileStore>(MockBehavior.Strict);
            sharedCredentialsFileMock.Setup(x => x.RegisterProfile(It.IsAny<CredentialProfile>()));

            RegisterProfile(profileName, accessKey, secretKey);

            sharedCredentialsFileMock.Verify(x => x.RegisterProfile(It.IsAny<CredentialProfile>()), Times.Once);
        }
    }
}
