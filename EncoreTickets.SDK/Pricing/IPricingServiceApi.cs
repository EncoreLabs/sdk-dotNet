using System;
using System.Collections.Generic;
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

        /// <summary>
        /// Returns price bands for the specified product.
        /// </summary>
        /// <returns>Price bands.</returns>
        IList<PriceBand> GetPriceBands(string productId, int quantity, DateTime? performanceDateTime = null);

        /// <summary>
        /// Returns price ranges broken down by day.
        /// </summary>
        /// <returns>Daily price ranges.</returns>
        IList<DailyPriceRange> GetDailyPriceRanges(string productId, int quantity, DateTime fromDate, DateTime toDate);

        /// <summary>
        /// Returns price ranges broken down by month.
        /// </summary>
        /// <returns>Monthly price ranges.</returns>
        IList<MonthlyPriceRange> GetMonthlyPriceRanges(string productId, int quantity, DateTime fromDate, DateTime toDate);

        /// <summary>
        /// Returns the list of all price rule summaries.
        /// Authorization required.
        /// </summary>
        /// <returns>Price rule summaries.</returns>
        IList<PriceRuleSummary> GetPriceRuleSummaries();

        /// <summary>
        /// Returns the details of a price rule with the specified ID.
        /// Authorization required.
        /// </summary>
        /// <returns>Price rule details.</returns>
        PriceRule GetPriceRule(int id);
    }
}
