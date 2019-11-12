using System.Globalization;
using EncoreTickets.SDK.Api;
using EncoreTickets.SDK.Api.Context;
using EncoreTickets.SDK.Api.Helpers;
using EncoreTickets.SDK.Api.Results;
using EncoreTickets.SDK.Authentication;
using EncoreTickets.SDK.Pricing.Models;
using EncoreTickets.SDK.Pricing.Models.RequestModels;

namespace EncoreTickets.SDK.Pricing
{
    /// <inheritdoc/>
    /// <summary>
    /// The service to provide an interface for calling Pricing API endpoints.
    /// </summary>
    public class PricingServiceApi : BaseApi
    {
        private const string PricingHost = "pricing-service.{0}tixuk.io/api/";

        private readonly string DateFormat = "yyyy-MM-ddTHH:mm:sszzz";

        /// <summary>
        /// Gets the authentication service for the current Pricing service./>
        /// </summary>
        public AuthenticationService AuthenticationService { get; }

        public PricingServiceApi(ApiContext context) : base(context, PricingHost)
        {
            context.AuthenticationMethod = AuthenticationMethod.JWT;
            AuthenticationService = new AuthenticationService(context, PricingHost, "login");
        }

        /// <summary>
        /// Returns a page with exchange rates
        /// Authorization required.
        /// </summary>
        /// <returns>Exchange rates.</returns>
        public ResponseForPage<ExchangeRate> GetExchangeRates(ExchangeRatesParameters parameters)
        {
            var result = Executor.ExecuteApiWithWrappedResponse<ResponseForPage<ExchangeRate>>(
                "v2/admin/exchange_rates",
                RequestMethod.Get,
                query: parameters,
                dateFormat: DateFormat);
            return result.DataOrException;
        }
    }
}
