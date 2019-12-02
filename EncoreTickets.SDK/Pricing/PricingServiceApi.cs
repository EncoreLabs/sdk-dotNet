using EncoreTickets.SDK.Api;
using EncoreTickets.SDK.Api.Context;
using EncoreTickets.SDK.Api.Helpers;
using EncoreTickets.SDK.Api.Results.Response;
using EncoreTickets.SDK.Pricing.Models;
using EncoreTickets.SDK.Pricing.Models.RequestModels;

namespace EncoreTickets.SDK.Pricing
{
    /// <inheritdoc cref="BaseApiWithAuthentication" />
    /// <inheritdoc cref="IPricingServiceApi" />
    /// <summary>
    /// The service to provide an interface for calling Pricing API endpoints.
    /// </summary>
    public class PricingServiceApi : BaseApiWithAuthentication, IPricingServiceApi
    {
        private const string PricingHost = "pricing-service.{0}tixuk.io/api/";
        private const string DateFormat = "yyyy-MM-ddTHH:mm:sszzz";

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context"></param>
        /// /// <param name="automaticAuthentication"></param>
        public PricingServiceApi(ApiContext context, bool automaticAuthentication = false) : base(context, PricingHost, automaticAuthentication)
        {
            context.AuthenticationMethod = AuthenticationMethod.JWT;
        }

        /// <inheritdoc />
        public ResponseForPage<ExchangeRate> GetExchangeRates(ExchangeRatesParameters parameters)
        {
            TriggerAutomaticAuthentication();
            var result = Executor.ExecuteApiWithWrappedResponse<ResponseForPage<ExchangeRate>>(
                "v2/admin/exchange_rates",
                RequestMethod.Get,
                query: parameters,
                dateFormat: DateFormat);
            return result.DataOrException;
        }
    }
}
