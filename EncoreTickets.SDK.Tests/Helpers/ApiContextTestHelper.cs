using EncoreTickets.SDK.Api.Models;

namespace EncoreTickets.SDK.Tests.Helpers
{
    internal static class ApiContextTestHelper
    {
        public static ApiContext DefaultApiContext => new ApiContext(Environments.Sandbox);

        public static void ResetContextToDefault(ApiContext context)
        {
            context.Environment = DefaultApiContext.Environment;
            context.AuthenticationMethod = DefaultApiContext.AuthenticationMethod;
            context.AccessToken = DefaultApiContext.AccessToken;
            context.UserName = DefaultApiContext.UserName;
            context.Password = DefaultApiContext.Password;
            context.Affiliate = DefaultApiContext.Affiliate;
            context.Correlation = DefaultApiContext.Correlation;
            context.Market = DefaultApiContext.Market;
        }
    }
}
