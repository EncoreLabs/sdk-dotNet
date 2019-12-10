namespace EncoreTickets.SDK.Venue.Models
{
    public class SeatSettings
    {
        public bool SeatsSupplied { get; set; }

        public SeatSelectionMode SeatSelectionMode { get; set; }

        public AllocationType AllocationType { get; set; }
    }
}