using EncoreTickets.SDK.Api;
using EncoreTickets.SDK.Api.Results.Response;
using EncoreTickets.SDK.Pricing.Models;
using EncoreTickets.SDK.Pricing.Models.RequestModels;

namespace EncoreTickets.SDK.Pricing
{
    /// <inheritdoc/>
    /// <summary>
    /// The interface of a pricing service.
    /// </summary>
    public interface IPricingServiceApi : IServiceApiWithAuthentication
    {
        /// <summary>
        /// Returns a page with exchange rates
        /// Authorization required.
        /// </summary>
        /// <returns>Exchange rates.</returns>
        ResponseForPage<ExchangeRate> GetExchangeRates(ExchangeRatesParameters parameters);
    }
}
