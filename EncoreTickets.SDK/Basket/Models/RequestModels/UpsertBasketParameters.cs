using System.Collections.Generic;

namespace EncoreTickets.SDK.Basket.Models.RequestModels
{
    public class UpsertBasketParameters
    {
        /// <summary>
        /// Gets or sets reference (ID) for a basket. This must be unique.
        /// </summary>
        public string Reference { get; set; }

        /// <summary>
        /// Gets or sets channel ID (Office ID) for a basket.
        /// Required.
        /// </summary>
        public string ChannelId { get; set; }

        /// <summary>
        /// Gets or sets basket delivery option.
        /// </summary>
        public Delivery Delivery { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets a flag indicating whether the basket supports Flexi tickets.
        /// </summary>
        public bool HasFlexiTickets { get; set; }

        /// <summary>
        /// Gets or sets shopper currency.
        /// </summary>
        public string ShopperCurrency { get; set; }

        /// <summary>
        /// Gets or sets shopper reference.
        /// </summary>
        public string ShopperReference { get; set; }

        /// <summary>
        /// Gets or sets reservations for the basket.
        /// Required.
        /// </summary>
        public List<ReservationParameters> Reservations { get; set; }

        /// <summary>
        /// Gets or sets coupon for the basket.
        /// </summary>
        public Coupon Coupon { get; set; }
    }
}
