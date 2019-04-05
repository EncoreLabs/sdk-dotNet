using System;

namespace EncoreTickets.SDK.EntertainApi.Model
{
    public class Stock
    {
        public int ShowId { get; set; }
        public int VenueId { get; set; }
        public string Performance { get; set; }
        public DateTime Date { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public int LargestLump { get; set; }
        public int Seats { get; set; }
        public bool Discounted { get; set; }
    }
}