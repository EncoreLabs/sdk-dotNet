namespace EncoreTickets.SDK.Inventory.Models
{
    public class Seat
    {
        public string aggregateReference { get; set; }

        public string itemReference { get; set; }

        public string row { get; set; }

        public int? number { get; set; }

        public bool? isAvailable { get; set; }

        public Attributes attributes { get; set; }

        public Pricing pricing { get; set; }
    }
}