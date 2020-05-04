using System.Collections.Generic;

namespace EncoreTickets.SDK.Payment.Models.RequestModels
{
    public class CreateOrderRequest
    {
        public string Description { get; set; }

        public string ChannelId { get; set; }

        public string ExternalId { get; set; }

        public string RedirectUrl { get; set; }

        public string Origin { get; set; }

        public Amount Amount { get; set; }

        public Amount AmountOriginal { get; set; }

        public Address BillingAddress { get; set; }

        public Shopper Shopper { get; set; }

        public List<OrderItem> Items { get; set; }

        public RiskData RiskData { get; set; }
    }
}