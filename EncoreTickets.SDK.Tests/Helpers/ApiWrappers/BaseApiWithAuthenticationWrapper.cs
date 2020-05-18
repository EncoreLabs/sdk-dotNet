using EncoreTickets.SDK.Api;
using EncoreTickets.SDK.Api.Models;

namespace EncoreTickets.SDK.Tests.Helpers.ApiWrappers
{
    internal class BaseApiWithAuthenticationWrapper : BaseApiWithAuthentication, IApiWithAuthenticationWrapper
    {
        public override int? ApiVersion { get; }

        public ApiContext SourceContext => Context;

        public string SourceHost => Host;

        public bool SourceAutomaticAuthentication => AutomaticAuthentication;

        public BaseApiWithAuthenticationWrapper(ApiContext context, string host, bool automaticAuthentication = false)
            : base(context, host, automaticAuthentication)
        {
        }
    }
}