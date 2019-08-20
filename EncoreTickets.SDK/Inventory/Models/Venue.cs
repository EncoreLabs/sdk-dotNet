using EncoreTickets.SDK.Interfaces;

namespace EncoreTickets.SDK.Inventory.Models
{
    /// <summary>
    /// Represents a venue object
    /// </summary>
    public class Venue : IIdentifiableObject
    {
        public string id { get; set; }
    }
}