using System;
using System.Collections.Generic;
using EncoreTickets.SDK.Inventory.Models;

namespace EncoreTickets.SDK.Basket.Models
{
    public class Reservation
    {
        public int id { get; internal set; }

        public string venueId { get; internal set; }

        public string venueName { get; internal set; }

        public string productId { get; internal set; }

        public string productName { get; internal set; }

        public DateTimeOffset date { get; internal set; }

        public int quantity { get; internal set; }

        public List<Seat> items { get; internal set; }

        public Price faceValueInOfficeCurrency { get; internal set; }

        public Price faceValueInShopperCurrency { get; internal set; }

        public Price salePriceInOfficeCurrency { get; internal set; }

        public Price salePriceInShopperCurrency { get; internal set; }

        public Price adjustedSalePriceInOfficeCurrency { get; internal set; }

        public Price adjustedSalePriceInShopperCurrency { get; internal set; }

        public Price adjustmentAmountInOfficeCurrency { get; internal set; }

        public Price adjustmentAmountInShopperCurrency { get; internal set; }
    }
}
