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
        /// GET api/v{ApiVersion}/admin/exchange_rates.
        /// </summary>
        /// <returns>Exchange rates.</returns>
        ResponseForPage<ExchangeRate> GetExchangeRates(ExchangeRatesParameters parameters);

        /// <summary>
        /// Returns price bands for the specified product.
        /// GET api/v{ApiVersion}/pricing/products/{productId}/quantity/{quantity}/bands.
        /// </summary>
        /// <returns>Price bands.</returns>
        IList<PriceBand> GetPriceBands(string productId, int quantity, DateTime? performanceDateTime = null);

        /// <summary>
        /// Returns price ranges broken down by day.
        /// GET api/v{ApiVersion}/pricing/days/products/{productId}/quantity/{quantity}/from/{fromDate}/to/{toDate}.
        /// </summary>
        /// <returns>Daily price ranges.</returns>
        IList<DailyPriceRange> GetDailyPriceRanges(string productId, int quantity, DateTime fromDate, DateTime toDate);

        /// <summary>
        /// Returns price ranges broken down by month.
        /// GET api/v{ApiVersion}/pricing/months/products/{productId}/quantity/{quantity}/from/{fromDate}/to/{toDate}.
        /// </summary>
        /// <returns>Monthly price ranges.</returns>
        IList<MonthlyPriceRange> GetMonthlyPriceRanges(string productId, int quantity, DateTime fromDate, DateTime toDate);

        /// <summary>
        /// Returns the list of all price rule summaries.
        /// Authorization required.
        /// GET api/v{ApiVersion}/admin/pricing/rules.
        /// </summary>
        /// <returns>Price rule summaries.</returns>
        IList<PriceRuleSummary> GetPriceRuleSummaries();

        /// <summary>
        /// Returns the details of a price rule with the specified ID.
        /// Authorization required.
        /// GET api/v{ApiVersion}/admin/pricing/rules/{id}.
        /// </summary>
        /// <returns>Price rule details.</returns>
        PriceRule GetPriceRule(int id);

        /// <summary>
        /// Returns the summaries of all partner groups.
        /// Authorization required.
        /// GET api/v{ApiVersion}/admin/groups.
        /// </summary>
        /// <returns>Partner group summaries.</returns>
        IList<PartnerGroup> GetPartnerGroups();

        /// <summary>
        /// Returns the details of all partners in the specified partner group.
        /// Authorization required.
        /// GET api/v{ApiVersion}/admin/groups/{partnerGroupId}/partners.
        /// </summary>
        /// <returns>Partner details.</returns>
        IList<Partner> GetPartnersInGroup(int partnerGroupId);

        /// <summary>
        /// Returns the details of the specified partner.
        /// Authorization required.
        /// GET api/v{ApiVersion}/admin/partners/{id}.
        /// </summary>
        /// <returns>Partner details.</returns>
        Partner GetPartner(int id);
    }
}
