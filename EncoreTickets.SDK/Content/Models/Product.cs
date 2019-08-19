using System;
using EncoreTickets.SDK.Interfaces;

namespace EncoreTickets.SDK.Content.Models
{
    public class Product : IObject
    {
        public string id { get; set; }

        public string name { get; set; }

        public string areaCode { get; set; }

        public ShowType showType { get; set; }

        public DateTime? firstPreviewDate { get; set; }

        public DateTime? openingDate { get; set; }

        public DateTime? boOpensDate { get; set; }

        public DateTime? boClosesDate { get; set; }

        public string runTime { get; set; }

        public int fitMaximum { get; set; }

        public Rating rating { get; set; }

        public string synopsis { get; set; }

        public Venue venue { get; set; }

        public string onSale { get; set; }
    }
}