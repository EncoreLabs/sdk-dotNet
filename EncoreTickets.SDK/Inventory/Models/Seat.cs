namespace EncoreTickets.SDK.Inventory.Models
{
    public class Seat : BaseSeat
    {
        public string ItemReference { get; set; }

        public bool? IsAvailable { get; set; }

        public Attributes Attributes { get; set; }

        public Pricing Pricing { get; set; }

        public AggregateReference AggregateReferenceObject { get; set; }
    }
}