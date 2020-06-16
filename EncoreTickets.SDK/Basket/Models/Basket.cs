using System;
using System.Collections.Generic;

namespace EncoreTickets.SDK.Basket.Models
{
    public class Basket
    {
        /// <summary>
        /// Gets or sets reference (ID) for a basket. This must be unique.
        /// </summary>
        public string Reference { get; set; }

        /// <summary>
        /// Gets or sets checksum (password) for a basket.
        /// </summary>
        public string Checksum { get; set; }

        /// <summary>
        /// Gets or sets channel ID (Office ID) for a basket.
        /// </summary>
        public string ChannelId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets a flag indicating whether the basket is mixed.
        /// </summary>
        public bool Mixed { get; set; }

        /// <summary>
        /// Gets or sets office currency.
        /// </summary>
        public string OfficeCurrency { get; set; }

        /// <summary>
        /// Gets or sets shopper currency.
        /// </summary>
        public string ShopperCurrency { get; set; }

        /// <summary>
        /// Gets or sets shopper reference.
        /// </summary>
        public string ShopperReference { get; set; }

        /// <summary>
        /// Gets or sets exchange Rate for a basket.
        /// </summary>
        public decimal ExchangeRate { get; set; }

        /// <summary>
        /// Gets or sets a basket delivery option.
        /// </summary>
        public Delivery Delivery { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets FlexiTicket flag.
        /// </summary>
        public bool AllowFlexiTickets { get; set; }

        /// <summary>
        /// Gets or sets a basket current status.
        /// </summary>
        public BasketStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the expiration date of the basket.
        /// </summary>
        public DateTimeOffset ExpiredAt { get; set; }

        /// <summary>
        /// Gets or sets the creation date of the basket.
        /// </summary>
        public DateTimeOffset CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets reservations of the basket.
        /// </summary>
        public List<Reservation> Reservations { get; set; }

        /// <summary>
        /// Gets or sets a coupon for the basket.
        /// </summary>
        public Coupon Coupon { get; set; }

        /// <summary>
        /// Gets or sets an applied promotion.
        /// </summary>
        public Promotion AppliedPromotion { get; set; }

        /// <summary>
        /// Gets or sets a missed promotion.
        /// </summary>
        public List<Promotion> MissedPromotions { get; set; }
    }
}
