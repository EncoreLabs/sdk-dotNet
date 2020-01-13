using System;

namespace EncoreTickets.SDK.Basket.Models
{
    public class Promotion
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string DisplayText { get; set; }

        public string Description { get; set; }

        public string Reference { get; set; }

        public string ReportingCode { get; set; }

        public DateTimeOffset ValidFrom { get; set; }

        public DateTimeOffset ValidTo { get; set; }
    }
}
