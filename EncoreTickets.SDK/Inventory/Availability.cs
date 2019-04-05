using System;
using System.Collections.Generic;
using System.Text;

namespace EncoreTickets.SDK.Inventory
{
    public class Attributes
    {
        public bool restrictedView { get; set; }
        public bool sideView { get; set; }
    }

    public class Price
    {
        public int? value { get; set; }
        public string currency { get; set; }

        public override string ToString()
        {
            return string.Format("{0}{1}", this.currency, this.value / 100);
        }
    }

    public class Pricing
    {
        public string priceReference { get; set; }
        public Price salePrice { get; set; }
        public Price faceValue { get; set; }
        public int? percentage { get; set; }
        public bool? offer { get; set; }
        public bool? noBookingFee { get; set; }
        public DateTime? timestamp { get; set; }
    }

    public class Seat
    {
        public string aggregateReference { get; set; }
        public string itemReference { get; set; }
        public string row { get; set; }
        public int? number { get; set; }
        public bool? isAvailable { get; set; }
        public Attributes attributes { get; set; }
        public Pricing pricing { get; set; }
    }

    public class Grouping
    {
        public string aggregateReference { get; set; }
        public string itemReference { get; set; }
        public string row { get; set; }
        public int? seatNumberStart { get; set; }
        public int? seatNumberEnd { get; set; }
        public int? availableCount { get; set; }
        public bool? isAvailable { get; set; }
        public Attributes attributes { get; set; }
        public Pricing pricing { get; set; }
        public List<Seat> seats { get; set; }
    }

    public class Area
    {
        public int? availableCount { get; set; }
        public DateTime? date { get; set; }
        public string name { get; set; }
        public string mode { get; set; }
        public bool? isAvailable { get; set; }
        public List<Grouping> groupings { get; set; }
        public string aggregateReference { get; set; }
        public string itemReference { get; set; }
    }

    public class Availability
    {
        public int? availableCount { get; set; }
        public List<Area> areas { get; set; }
        public bool isAvailable { get; set; }
    }
}
