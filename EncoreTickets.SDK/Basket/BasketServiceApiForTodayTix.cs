using System;
using EncoreTickets.SDK.Api.Models;
using EncoreTickets.SDK.Basket.Models;

namespace EncoreTickets.SDK.Basket
{
    /// <inheritdoc cref="BasketServiceApi" />
    /// <inheritdoc cref="IBasketServiceApi" />
    /// <summary>
    /// The wrapper class for the Basket service API with version used to work with TodayTix.
    /// </summary>
    public class BasketServiceApiForTodayTix : BasketServiceApi, IBasketServiceApi
    {
        /// <inheritdoc/>
        public override int? ApiVersion => 2;

        /// <summary>
        /// Initialises a new instance of the <see cref="BasketServiceApiForTodayTix"/> class.
        /// </summary>
        /// <param name="context">The API context for requests.</param>
        public BasketServiceApiForTodayTix(ApiContext context)
            : base(context)
        {
            if (context.Environment == Environments.Production)
            {
                throw new NotSupportedException("The API for TodayTix is not available for production.");
            }
        }

        /// <inheritdoc />
        public new Models.Basket UpsertPromotion(string basketReference, string couponName)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public new Models.Basket UpsertPromotion(string basketReference, Coupon coupon)
        {
            throw new NotImplementedException();
        }
    }
}