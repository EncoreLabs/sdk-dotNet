using EncoreTickets.SDK.Api;
using EncoreTickets.SDK.Api.Models;

namespace EncoreTickets.SDK.Tests.Helpers.ApiWrappers
{
    internal class BaseApiWrapper : BaseApi, IApiWrapper
    {
        public override int? ApiVersion { get; }

        public ApiContext SourceContext => Context;

        public string SourceHost => Host;

        public BaseApiWrapper(ApiContext context, string host) : base(context, host)
        {
        }
    }
}