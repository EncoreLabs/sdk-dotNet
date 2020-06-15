using System;
using System.Collections.Generic;

namespace EncoreTickets.SDK.Pricing.Models
{
    public class PriceRange
    {
        public string DisplayCurrency { get; set; }

        public DateTimeOffset? CreatedAt { get; set; }

        public IList<Price> MinPrice { get; set; }

        public IList<Price> MaxPrice { get; set; }

        public bool Offer { get; set; }

        public bool IncludesBookingFee { get; set; }
    }
}
