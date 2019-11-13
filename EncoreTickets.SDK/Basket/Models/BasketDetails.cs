using System;
using System.Collections.Generic;

namespace EncoreTickets.SDK.Basket.Models
{
    public class BasketDetails
    {
        public string reference { get; internal set; }

        public string checksum { get; internal set; }

        public string channelId { get; internal set; }

        public bool mixed { get; internal set; }

        public decimal exchangeRate { get; internal set; }

        public Delivery delivery { get; internal set; }

        public bool allowFlexiTickets { get; internal set; }

        public string status { get; set; }

        public DateTimeOffset expiredAt { get; internal set; }

        public DateTimeOffset createdAt { get; internal set; }

        public List<Reservation> reservations { get; internal set; }

        public Coupon coupon { get; internal set; }

        public Promotion appliedPromotion { get; internal set; }

        public List<Promotion> missedPromotions { get; internal set; }
    }
}
