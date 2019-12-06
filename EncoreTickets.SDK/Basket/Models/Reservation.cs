using System;
using System.Collections.Generic;

namespace EncoreTickets.SDK.Basket.Models
{
    public class Reservation
    {
        public int id { get; set; }

        public string venueId { get; set; }

        public string venueName { get; set; }

        public string productId { get; set; }

        public string productName { get; set; }

        public DateTimeOffset date { get; set; }

        public int quantity { get; set; }

        public List<Seat> items { get; set; }

        public Price faceValueInOfficeCurrency { get; set; }

        public Price faceValueInShopperCurrency { get; set; }

        public Price salePriceInOfficeCurrency { get; set; }

        public Price salePriceInShopperCurrency { get; set; }

        public Price adjustedSalePriceInOfficeCurrency { get; set; }

        public Price adjustedSalePriceInShopperCurrency { get; set; }

        public Price adjustmentAmountInOfficeCurrency { get; set; }

        public Price adjustmentAmountInShopperCurrency { get; set; }
    }
}
