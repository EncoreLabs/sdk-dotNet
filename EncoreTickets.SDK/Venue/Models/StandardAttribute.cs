using EncoreTickets.SDK.Interfaces;

namespace EncoreTickets.SDK.Venue.Models
{
    public class StandardAttribute : IObject
    {
        public string title { get; set; }

        public string description { get; set; }

        public string intention { get; set; }
    }
}
