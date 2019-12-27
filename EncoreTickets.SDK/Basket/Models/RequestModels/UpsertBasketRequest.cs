using System.Collections.Generic;

namespace EncoreTickets.SDK.Basket.Models.RequestModels
{
    internal class UpsertBasketRequest
    {
        public string Reference { get; set; }

        public string ChannelId { get; set; }

        public Delivery Delivery { get; set; }

        public bool HasFlexiTickets { get; set; }

        public string ShopperCurrency { get; set; }

        public string ShopperReference { get; set; }

        public List<ReservationRequest> Reservations { get; set; }

        public Coupon Coupon { get; set; }
    }
}
