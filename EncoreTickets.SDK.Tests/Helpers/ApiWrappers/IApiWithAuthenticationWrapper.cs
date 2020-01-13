namespace EncoreTickets.SDK.Tests.Helpers.ApiWrappers
{
    internal interface IApiWithAuthenticationWrapper : IApiWrapper
    {
        bool SourceAutomaticAuthentication { get; }
    }
}