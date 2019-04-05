using System;

namespace EncoreTickets.SDK.EntertainApi.Model
{
    public class BasketItemHistory
    {
        public string reservationId { get; set; }
        public string showId { get; set; }
        public string venueId { get; set; }
        public DateTime date { get; set; }
        public string quantity { get; set; }
        public string blockId { get; set; }
        public string seats { get; set; }
        public bool enta { get; set; }
        public DateTime basketExpiry { get; set; }
        public string availabilityUrl { get; set; }
        public bool markForDeletionMarkForDeletion { get; set; }
    }
}