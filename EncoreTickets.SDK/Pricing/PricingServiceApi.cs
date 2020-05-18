using EncoreTickets.SDK.Api;
using EncoreTickets.SDK.Api.Models;
using EncoreTickets.SDK.Api.Results.Response;
using EncoreTickets.SDK.Api.Utilities.RequestExecutor;
using EncoreTickets.SDK.Pricing.Models;
using EncoreTickets.SDK.Pricing.Models.RequestModels;
using EncoreTickets.SDK.Utilities.Enums;

namespace EncoreTickets.SDK.Pricing
{
    /// <inheritdoc cref="BaseApiWithAuthentication" />
    /// <inheritdoc cref="IPricingServiceApi" />
    /// <summary>
    /// The service to provide an interface for calling Pricing API endpoints.
    /// </summary>
    public class PricingServiceApi : BaseApiWithAuthentication, IPricingServiceApi
    {
        private const string PricingApiHost = "pricing-service.{0}tixuk.io/api/";
        private const string DateFormat = "yyyy-MM-ddTHH:mm:sszzz";

        /// <inheritdoc />
        public override int? ApiVersion => 3;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context"></param>
        /// /// <param name="automaticAuthentication"></param>
        public PricingServiceApi(ApiContext context, bool automaticAuthentication = false)
            : base(context, PricingApiHost, automaticAuthentication)
        {
        }

        /// <inheritdoc />
        public ResponseForPage<ExchangeRate> GetExchangeRates(ExchangeRatesParameters ratesParameters)
        {
            TriggerAutomaticAuthentication();
            var parameters = new ExecuteApiRequestParameters
            {
                Endpoint = $"v{ApiVersion}/admin/exchange_rates",
                Method = RequestMethod.Get,
                Query = ratesParameters,
                DateFormat = DateFormat
            };
            var result = Executor.ExecuteApiWithWrappedResponse<ResponseForPage<ExchangeRate>>(parameters);
            return result.DataOrException;
        }
    }
}
