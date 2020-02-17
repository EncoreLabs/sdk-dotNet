using System.Linq;
using EncoreTickets.SDK.Venue.Models;

namespace EncoreTickets.SDK.Venue.Extensions
{
    public static class SeatDetailedExtension
    {
        public static bool IsValid(this SeatDetailed seat)
        {
            return seat != null &&
                   !string.IsNullOrEmpty(seat.SeatIdentifier) &&
                   seat.Attributes != null && seat.Attributes.Any();
        }
    }
}