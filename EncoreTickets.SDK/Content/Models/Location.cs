using System.Collections.Generic;

namespace EncoreTickets.SDK.Content.Models
{
    /// <summary>
    /// Location.
    /// </summary>
    public class Location
    {
        public string Name { get; set; }

        public string IsoCode { get; set; }

        public List<Location> SubLocations { get; set; }
    }
}
