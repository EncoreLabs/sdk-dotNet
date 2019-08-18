using EncoreTickets.SDK.Api.Context;

namespace EncoreTickets.SDK.Api.Helpers.RestClientWrapper
{
    internal class RestClientWrapperCredentials
    {
        public AuthenticationMethod AuthenticationMethod { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string AccessToken { get; set; }
    }
}