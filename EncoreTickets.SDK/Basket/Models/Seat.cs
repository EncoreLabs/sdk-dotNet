namespace EncoreTickets.SDK.Basket.Models
{
    public class Seat
    {
        public string aggregateReference { get; internal set; }

        public string areaId { get; internal set; }

        public string areaName { get; internal set; }

        public string row { get; internal set; }

        public string number { get; internal set; }

        public string locationDescription { get; internal set; }
    }
}
