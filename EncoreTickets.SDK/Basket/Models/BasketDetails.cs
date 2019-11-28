using System;
using System.Collections.Generic;

namespace EncoreTickets.SDK.Basket.Models
{
    public class BasketDetails
    {
        public string reference { get; set; }

        public string checksum { get; set; }

        public string channelId { get; set; }

        public bool mixed { get; set; }

        public decimal exchangeRate { get; set; }

        public Delivery delivery { get; set; }

        public bool allowFlexiTickets { get; set; }

        public string status { get; set; }

        public DateTimeOffset expiredAt { get; set; }

        public DateTimeOffset createdAt { get; set; }

        public List<Reservation> reservations { get; set; }

        public Coupon coupon { get; set; }

        public Promotion appliedPromotion { get; set; }

        public List<Promotion> missedPromotions { get; set; }
    }
}
