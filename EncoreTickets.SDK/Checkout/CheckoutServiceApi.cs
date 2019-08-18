using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using EncoreTickets.SDK.Api;
using EncoreTickets.SDK.Api.Context;

namespace EncoreTickets.SDK.Content
{
    /// <summary>
    /// Wrapper class for the inventory service API
    /// </summary>
    public class CheckoutServiceApi : BaseCapabilityServiceApi
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
