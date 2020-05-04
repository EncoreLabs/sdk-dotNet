using System.Collections.Generic;

namespace EncoreTickets.SDK.Payment.Models.RequestModels
{
    public class UpdateOrderRequest
    {
        public Address BillingAddress { get; set; }

        public Shopper Shopper { get; set; }

        public List<OrderItem> Items { get; set; }

        public RiskData RiskData { get; set; }
    }
}
