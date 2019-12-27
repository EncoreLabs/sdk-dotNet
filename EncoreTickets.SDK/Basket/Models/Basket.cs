using System;
using System.Collections.Generic;

namespace EncoreTickets.SDK.Basket.Models
{
    public class Basket
    {
        public string Reference { get; set; }

        public string Checksum { get; set; }

        public string ChannelId { get; set; }

        public bool Mixed { get; set; }

        public string OfficeCurrency { get; set; }

        public string ShopperCurrency { get; set; }

        public string ShopperReference { get; set; }

        public decimal ExchangeRate { get; set; }

        public Delivery Delivery { get; set; }

        public bool AllowFlexiTickets { get; set; }

        public string Status { get; set; }

        public DateTimeOffset ExpiredAt { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public List<Reservation> Reservations { get; set; }

        public Coupon Coupon { get; set; }

        public Promotion AppliedPromotion { get; set; }

        public List<Promotion> MissedPromotions { get; set; }
    }
}
