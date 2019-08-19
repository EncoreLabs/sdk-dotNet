using System.Collections.Generic;

namespace EncoreTickets.SDK.Venue.Models
{
    public class Terminal
    {
        public string name { get; set; }

        public List<Route> routes { get; set; }
    }
}