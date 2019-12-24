using EncoreTickets.SDK.Api;
using EncoreTickets.SDK.Api.Context;

namespace EncoreTickets.SDK.Tests.Helpers.ApiWrappers
{
    internal class BaseApiWrapper : BaseApi, IApiWrapper
    {
        public ApiContext SourceContext => Context;

        public string SourceHost => Host;

        public BaseApiWrapper(ApiContext context, string host) : base(context, host)
        {
        }
    }
}