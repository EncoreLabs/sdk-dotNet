using System.Collections.Generic;

namespace EncoreTickets.SDK.EntertainApi.Model
{
    public class TicketGroup
    {
        public string BlockId { get; set; }

        public string BlockDescription { get; set; }

        public IEnumerable<Ticket> Tickets { get; set; }

        public int Count { get; set; }
    }
}