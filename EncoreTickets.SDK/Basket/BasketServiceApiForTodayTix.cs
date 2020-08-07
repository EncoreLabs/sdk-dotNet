using EncoreTickets.SDK.Api.Models;

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
        }
    }
}