using System;
using System.Collections.Generic;

namespace EncoreTickets.SDK.Basket.Models
{
    public class Reservation
    {
        public int Id { get; set; }

        public int LinkedReservationId { get; set; }

        public string VenueId { get; set; }

        public string VenueName { get; set; }

        public string ProductId { get; set; }

        public string ProductType { get; set; }

        public string ProductName { get; set; }

        public DateTimeOffset Date { get; set; }

        public int Quantity { get; set; }

        public List<Seat> Items { get; set; }

        public Price FaceValueInOfficeCurrency { get; set; }

        public Price FaceValueInShopperCurrency { get; set; }

        public Price SalePriceInOfficeCurrency { get; set; }

        public Price SalePriceInShopperCurrency { get; set; }

        public Price AdjustedSalePriceInOfficeCurrency { get; set; }

        public Price AdjustedSalePriceInShopperCurrency { get; set; }

        public Price AdjustmentAmountInOfficeCurrency { get; set; }

        public Price AdjustmentAmountInShopperCurrency { get; set; }
    }
}
