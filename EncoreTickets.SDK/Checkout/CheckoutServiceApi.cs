using EncoreTickets.SDK.Api;
using EncoreTickets.SDK.Api.Context;

namespace EncoreTickets.SDK.Checkout
{
    /// <summary>
    /// Wrapper class for the inventory service API
    /// </summary>
    public class CheckoutServiceApi : BaseApi
    {
        /// <summary>
        /// Default constructor for the Inventory service
        /// </summary>
        /// <param name="context"></param>
        public CheckoutServiceApi(ApiContext context) : base(context, "checkout-service.{0}tixuk.io/api/")
        {
        }
    }
}
