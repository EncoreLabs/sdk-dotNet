using EncoreTickets.SDK.Api.Context;
using EncoreTickets.SDK.EntertainApi;

namespace EncoreTickets.SDK.Basket
{
    /// <summary>
    /// The wrapper class for the Basket service API.
    /// </summary>
    public class BasketServiceApi : BaseEntertainApi
    {
        /// <summary>
        /// Default constructor for the Basket service.
        /// </summary>
        /// <param name="context">The API context for requests.</param>
        public BasketServiceApi(ApiContext context) : base(context, "basket-service.{0}tixuk.io/api/")
        {
        }
    }
}
