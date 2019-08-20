using System;

namespace EncoreTickets.SDK.Inventory.Models
{
    public class Pricing
    {
        public string priceReference { get; set; }

        public Price salePrice { get; set; }

        public Price faceValue { get; set; }

        public int? percentage { get; set; }

        public bool? offer { get; set; }

        public bool? noBookingFee { get; set; }

        public DateTime? timestamp { get; set; }
    }
}