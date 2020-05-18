namespace EncoreTickets.SDK.Inventory.Models
{
    public class BaseSeat
    {
        public string SeatIdentifier { get; set; }

        public string AggregateReference { get; set; }

        public string Row { get; set; }

        public int? Number { get; set; }
    }
}