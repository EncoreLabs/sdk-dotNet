using EncoreTickets.SDK.Api.Context;
using EncoreTickets.SDK.Utilities.Common.RestClientWrapper;

namespace EncoreTickets.SDK.Api.Helpers.ApiRestClientBuilder
{
    public interface IApiRestClientBuilder
    {
        RestClientWrapper CreateClientWrapper(ApiContext context);

        RestClientParameters CreateClientWrapperParameters(ApiContext context, string baseUrl,
            ExecuteApiRequestParameters requestParameters);
    }
}