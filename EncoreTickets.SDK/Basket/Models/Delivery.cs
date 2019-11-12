using EncoreTickets.SDK.Inventory.Models;

namespace EncoreTickets.SDK.Basket.Models
{
    public class Delivery
    {
        public string method { get; internal set; }

        public Price charge { get; internal set; }
    }
}
