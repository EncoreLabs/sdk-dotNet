using System;
using System.Collections.Generic;

namespace EncoreTickets.SDK.Basket.Models.RequestModels
{
    public class ReservationRequest
    {
        public string VenueId { get; set; }

        public string ProductId { get; set; }

        public DateTimeOffset Date { get; set; }

        public int Quantity { get; set; }

        public List<ItemRequest> Items { get; set; }
    }
}
