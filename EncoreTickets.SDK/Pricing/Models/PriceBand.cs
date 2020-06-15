using System;
using System.Collections.Generic;

namespace EncoreTickets.SDK.Pricing.Models
{
    public class PriceBand
    {
        public DateTimeOffset? Date { get; set; }

        public string DisplayCurrency { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public int Percentage { get; set; }

        public bool Offer { get; set; }

        public bool NoBookingFee { get; set; }

        public IList<Price> SalePrice { get; set; }

        public IList<Price> FaceValue { get; set; }
    }
}
