namespace EncoreTickets.SDK.Aws.Utilities
{
    public interface IProfileRegistrar
    {
        void RegisterProfile(string profileName, string accessKey, string secretKey);
    }
}