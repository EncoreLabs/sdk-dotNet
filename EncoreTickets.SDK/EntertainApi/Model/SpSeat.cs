using System.Collections.Generic;

namespace EncoreTickets.SDK.EntertainApi.Model
{
    public class SpSeat
    {
        public int Number { get; set; }

        public string Row { get; set; }

        public List<SpSeatPerformance> SeatPerformances { get; set; }

        public int X { get; set; }

        public int Y { get; set; }
    }
}