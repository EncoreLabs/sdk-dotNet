using System;
using System.Collections.Generic;
using EncoreTickets.SDK.Api;
using EncoreTickets.SDK.Api.Models;
using EncoreTickets.SDK.Api.Results.Response;
using EncoreTickets.SDK.Api.Utilities.RequestExecutor;
using EncoreTickets.SDK.Pricing.Models;
using EncoreTickets.SDK.Pricing.Models.RequestModels;
using EncoreTickets.SDK.Utilities;
using EncoreTickets.SDK.Utilities.BaseTypesExtensions;
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
        /// Initialises a new instance of the <see cref="PricingServiceApi"/> class.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="automaticAuthentication"></param>
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
                DateFormat = DateFormat,
            };
            var result = Executor.ExecuteApiWithWrappedResponse<ResponseForPage<ExchangeRate>>(parameters);
            return result.DataOrException;
        }

        /// <inheritdoc />
        public IList<PriceBand> GetPriceBands(string productId, int quantity, DateTime? performanceDateTime = null)
        {
            ThrowArgumentExceptionIfProductIdNotSet(productId);
            var queryParameters = new PriceBandsQueryParameters(performanceDateTime);
            var parameters = new ExecuteApiRequestParameters
            {
                Endpoint = $"v{ApiVersion}/pricing/products/{productId}/quantity/{quantity}/bands",
                Method = RequestMethod.Get,
                Query = queryParameters,
            };
            var result = Executor.ExecuteApiWithWrappedResponse<IList<PriceBand>>(parameters);
            return result.DataOrException;
        }

        /// <inheritdoc />
        public IList<DailyPriceRange> GetDailyPriceRanges(string productId, int quantity, DateTime fromDate, DateTime toDate)
        {
            ThrowArgumentExceptionIfProductIdNotSet(productId);
            var parameters = new ExecuteApiRequestParameters
            {
                Endpoint = $"v{ApiVersion}/pricing/days/products/{productId}/quantity/{quantity}" +
                           $"/from/{fromDate.ToEncoreDate()}/to/{toDate.ToEncoreDate()}",
                Method = RequestMethod.Get,
            };
            var result = Executor.ExecuteApiWithWrappedResponse<IList<DailyPriceRange>>(parameters);
            return result.DataOrException;
        }

        /// <inheritdoc />
        public IList<MonthlyPriceRange> GetMonthlyPriceRanges(string productId, int quantity, DateTime fromDate, DateTime toDate)
        {
            ThrowArgumentExceptionIfProductIdNotSet(productId);
            var parameters = new ExecuteApiRequestParameters
            {
                Endpoint = $"v{ApiVersion}/pricing/months/products/{productId}/quantity/{quantity}" +
                           $"/from/{fromDate.ToEncoreDate()}/to/{toDate.ToEncoreDate()}",
                Method = RequestMethod.Get,
            };
            var result = Executor.ExecuteApiWithWrappedResponse<IList<MonthlyPriceRange>>(parameters);
            return result.DataOrException;
        }

        /// <inheritdoc />
        public IList<PriceRuleSummary> GetPriceRuleSummaries()
        {
            TriggerAutomaticAuthentication();
            var parameters = new ExecuteApiRequestParameters
            {
                Endpoint = $"v{ApiVersion}/admin/pricing/rules",
                Method = RequestMethod.Get,
            };
            var result = Executor.ExecuteApiWithWrappedResponse<IList<PriceRuleSummary>>(parameters);
            return result.DataOrException;
        }

        /// <inheritdoc />
        public PriceRule GetPriceRule(int id)
        {
            TriggerAutomaticAuthentication();
            var parameters = new ExecuteApiRequestParameters
            {
                Endpoint = $"v{ApiVersion}/admin/pricing/rules/{id}",
                Method = RequestMethod.Get,
            };
            var result = Executor.ExecuteApiWithWrappedResponse<PriceRule>(parameters);
            return result.DataOrException;
        }

        /// <inheritdoc />
        public IList<PartnerGroup> GetPartnerGroups()
        {
            TriggerAutomaticAuthentication();
            var parameters = new ExecuteApiRequestParameters
            {
                Endpoint = $"v{ApiVersion}/admin/groups",
                Method = RequestMethod.Get,
            };
            var result = Executor.ExecuteApiWithWrappedResponse<IList<PartnerGroup>>(parameters);
            return result.DataOrException;
        }

        /// <inheritdoc />
        public IList<Partner> GetPartnersInGroup(int partnerGroupId)
        {
            TriggerAutomaticAuthentication();
            var parameters = new ExecuteApiRequestParameters
            {
                Endpoint = $"v{ApiVersion}/admin/groups/{partnerGroupId}/partners",
                Method = RequestMethod.Get,
            };
            var result = Executor.ExecuteApiWithWrappedResponse<IList<Partner>>(parameters);
            return result.DataOrException;
        }

        /// <inheritdoc />
        public Partner GetPartner(int id)
        {
            TriggerAutomaticAuthentication();
            var parameters = new ExecuteApiRequestParameters
            {
                Endpoint = $"v{ApiVersion}/admin/partners/{id}",
                Method = RequestMethod.Get,
            };
            var result = Executor.ExecuteApiWithWrappedResponse<Partner>(parameters);
            return result.DataOrException;
        }

        private static void ThrowArgumentExceptionIfProductIdNotSet(string productId)
        {
            ValidationHelper.ThrowArgumentExceptionIfNotSet(("Product ID", productId));
        }
    }
}
