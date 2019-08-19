namespace EncoreTickets.SDK.Venue.Models
{
    public class SeatSettings
    {
        public bool seatsSupplied { get; set; }

        public SeatSelectionMode seatSelectionMode { get; set; }

        public AllocationType allocationType { get; set; }
    }
}