using System;
using System.Collections.Generic;

namespace EncoreTickets.SDK.Basket.Models
{
    public class Reservation
    {
        /// <summary>
        /// Gets or sets the reservation ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the linked reservation ID.
        /// </summary>
        public int LinkedReservationId { get; set; }

        /// <summary>
        /// Gets or sets the venue ID.
        /// </summary>
        public string VenueId { get; set; }

        /// <summary>
        /// Gets or sets the venue name.
        /// </summary>
        public string VenueName { get; set; }

        /// <summary>
        /// Gets or sets the product ID.
        /// </summary>
        public string ProductId { get; set; }

        /// <summary>
        /// Gets or sets the product type.
        /// </summary>
        public string ProductType { get; set; }

        /// <summary>
        /// Gets or sets the product name.
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// Gets or sets the date of the reservation.
        /// </summary>
        public DateTimeOffset Date { get; set; }

        /// <summary>
        /// Gets or sets the quantity of the reservation items.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the reservation items.
        /// </summary>
        public List<ReservationItem> Items { get; set; }

        /// <summary>
        /// Gets or sets the face value of the reservation represented in the office currency.
        /// </summary>
        public Price FaceValueInOfficeCurrency { get; set; }

        /// <summary>
        /// Gets or sets the face value of the reservation represented in the shopper currency.
        /// </summary>
        public Price FaceValueInShopperCurrency { get; set; }

        /// <summary>
        /// Gets or sets the sale price of the reservation represented in the office currency.
        /// </summary>
        public Price SalePriceInOfficeCurrency { get; set; }

        /// <summary>
        /// Gets or sets the sale price of the reservation represented in the shopper currency.
        /// </summary>
        public Price SalePriceInShopperCurrency { get; set; }

        /// <summary>
        /// Gets or sets the adjusted sale price of the reservation represented in the office currency.
        /// </summary>
        public Price AdjustedSalePriceInOfficeCurrency { get; set; }

        /// <summary>
        /// Gets or sets the adjusted sale price of the reservation represented in the shopper currency.
        /// </summary>
        public Price AdjustedSalePriceInShopperCurrency { get; set; }

        /// <summary>
        /// Gets or sets the adjustment amount of the reservation represented in the office currency.
        /// </summary>
        public Price AdjustmentAmountInOfficeCurrency { get; set; }

        /// <summary>
        /// Gets or sets the adjustment amount of the reservation represented in the shopper currency.
        /// </summary>
        public Price AdjustmentAmountInShopperCurrency { get; set; }
    }
}
