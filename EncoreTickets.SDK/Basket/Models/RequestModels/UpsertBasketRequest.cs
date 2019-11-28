using System.Collections.Generic;

namespace EncoreTickets.SDK.Basket.Models.RequestModels
{
    public class UpsertBasketRequest
    {
        public string reference { get; set; }

        public string channelId { get; set; }

        public Delivery delivery { get; set; }

        public bool hasFlexiTickets { get; set; }

        public string shopperCurrency { get; set; }

        public string shopperReference { get; set; }

        public List<ReservationRequest> reservations { get; set; }

        public Coupon coupon { get; set; }
    }
}
