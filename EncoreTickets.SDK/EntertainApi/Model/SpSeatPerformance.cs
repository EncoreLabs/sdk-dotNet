using System;

namespace EncoreTickets.SDK.EntertainApi.Model
{
    public class SpSeatPerformance
    {
        public string BlockDescription { get; set; }

        public string BlockId { get; set; }

        public DateTime Date { get; set; }

        public bool Discounted { get; set; }

        public int PerformanceId { get; set; }

        public decimal Price { get; set; }

        public bool RestrictedView { get; set; }

        public string SeatKey { get; set; }

        public string SeatLumps { get; set; }
    }
}