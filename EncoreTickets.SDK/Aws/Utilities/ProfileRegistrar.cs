using Amazon.Runtime.CredentialManagement;

namespace EncoreTickets.SDK.Aws.Utilities
{
    public class ProfileRegistrar : IProfileRegistrar
    {
        public void RegisterProfile(string profileName, string accessKey, string secretKey)
        {
            var options = new CredentialProfileOptions
            {
                AccessKey = accessKey,
                SecretKey = secretKey
            };
            var profile = new CredentialProfile(profileName, options);
            var credentialsFile = CreateCredentialProfileStore();
            credentialsFile.RegisterProfile(profile);
        }

        protected virtual ICredentialProfileStore CreateCredentialProfileStore()
        {
            return new SharedCredentialsFile();
        }
    }
}