using Amazon.Runtime.CredentialManagement;

namespace EncoreTickets.SDK.Aws.Factories
{
    public interface ICredentialProfileStoreFactory
    {
        ICredentialProfileStore CreateCredentialProfileStore();
    }
}