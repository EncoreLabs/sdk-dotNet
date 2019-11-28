using System;
using System.Collections.Generic;

namespace EncoreTickets.SDK.Basket.Models.RequestModels
{
    public class ReservationRequest
    {
        public string venueId { get; set; }

        public string productId { get; set; }

        public DateTimeOffset date { get; set; }

        public int quantity { get; set; }

        public List<ItemRequest> items { get; set; }
    }
}
