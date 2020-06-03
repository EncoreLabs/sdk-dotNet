using EncoreTickets.SDK.Utilities.CommonModels;

namespace EncoreTickets.SDK.Inventory.Models
{
    public class BaseSeat : IEntityWithAggregateReference
    {
        public string SeatIdentifier { get; set; }

        public string AggregateReference { get; set; }

        public string Row { get; set; }

        public int? Number { get; set; }
    }
}