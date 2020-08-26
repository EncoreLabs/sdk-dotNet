using EncoreTickets.SDK.Api.Models;

namespace EncoreTickets.SDK.Tests.Helpers
{
    internal static class ApiContextTestHelper
    {
        public static ApiContext DefaultApiContext => new ApiContext(Environments.Sandbox);

        public static void ResetContextToDefault(ApiContext context)
        {
            var defaultApiContext = DefaultApiContext;
            context.Environment = defaultApiContext.Environment;
            context.AuthenticationMethod = defaultApiContext.AuthenticationMethod;
            context.AccessToken = defaultApiContext.AccessToken;
            context.UserName = defaultApiContext.UserName;
            context.Password = defaultApiContext.Password;
            context.Affiliate = defaultApiContext.Affiliate;
            context.Correlation = defaultApiContext.Correlation;
            context.Market = defaultApiContext.Market;
            context.AgentCredentials = defaultApiContext.AgentCredentials;
            context.DisplayCurrency = defaultApiContext.DisplayCurrency;
        }
    }
}
