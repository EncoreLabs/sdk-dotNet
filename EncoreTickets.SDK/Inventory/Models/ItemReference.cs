using System;

namespace EncoreTickets.SDK.Inventory.Models
{
    public class ItemReference
    {
        public string SupplierPrefix { get; set; }

        public string VenueId { get; set; }

        public string NativeProductId { get; set; }

        public string InternalItemId { get; set; }

        public DateTime Date { get; set; }

        public TimeSpan Time { get; set; }

        public int Quantity { get; set; }

        public string SeatLocationDescription { get; set; }
    }
}