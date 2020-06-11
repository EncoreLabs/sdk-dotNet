using System;
using System.Collections.Generic;

namespace EncoreTickets.SDK.Basket.Models.RequestModels
{
    public class ReservationParameters
    {
        /// <summary>
        /// Gets or sets venue ID.
        /// Required.
        /// </summary>
        public string VenueId { get; set; }

        /// <summary>
        /// Gets or sets product ID.
        /// Required.
        /// </summary>
        public string ProductId { get; set; }

        /// <summary>
        /// Gets or sets event date and time.
        /// </summary>
        public DateTimeOffset Date { get; set; }

        /// <summary>
        /// Gets or sets quantity.
        /// Required.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets reservation items.
        /// Reservation items count should be equal quantity.
        /// Required.
        /// </summary>
        public List<ReservationItemParameters> Items { get; set; }
    }
}
