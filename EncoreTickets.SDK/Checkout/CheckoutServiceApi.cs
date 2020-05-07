using EncoreTickets.SDK.Api;
using EncoreTickets.SDK.Api.Models;

namespace EncoreTickets.SDK.Checkout
{
    /// <inheritdoc cref="BaseApi" />
    /// <inheritdoc cref="ICheckoutServiceApi" />
    /// <summary>
    /// The wrapper class for the checkout service API.
    /// </summary>
    public class CheckoutServiceApi : BaseApi, ICheckoutServiceApi
    {
        public override int? ApiVersion => null;

        /// <summary>
        /// Default constructor for the checkout service
        /// </summary>
        /// <param name="context"></param>
        public CheckoutServiceApi(ApiContext context) : base(context, "checkout-service.{0}tixuk.io/api/")
        {
        }
    }
}
