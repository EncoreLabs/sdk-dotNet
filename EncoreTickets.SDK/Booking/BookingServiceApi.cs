using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using EncoreTickets.SDK.EntertainApi;

namespace EncoreTickets.SDK.Booking
{
    /// <summary>
    /// Wrapper class for the inventory service API
    /// </summary>
    public class BookingServiceApi : BaseEntertainApi
    {
        /// <summary>
        /// Default constructor for the Inventory service
        /// </summary>
        /// <param name="context"></param>
        public BookingServiceApi(ApiContext context) : base(context, "inventory.{0}.aws.encoretix.co.uk/api/")
        {
        }
    }
}
