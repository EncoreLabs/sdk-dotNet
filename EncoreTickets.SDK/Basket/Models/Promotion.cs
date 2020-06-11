using System;

namespace EncoreTickets.SDK.Basket.Models
{
    public class Promotion
    {
        /// <summary>
        /// Gets or sets the promotion ID.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the promotion name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the promotion display text.
        /// </summary>
        public string DisplayText { get; set; }

        /// <summary>
        /// Gets or sets the promotion description. 
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the promotion reference.
        /// </summary>
        public string Reference { get; set; }

        /// <summary>
        /// Gets or sets the reporting code.
        /// </summary>
        public string ReportingCode { get; set; }

        /// <summary>
        /// Gets or sets the date from which the promotion is valid.
        /// </summary>
        public DateTimeOffset ValidFrom { get; set; }

        /// <summary>
        /// Gets or sets the date to which the promotion is valid.
        /// </summary>
        public DateTimeOffset ValidTo { get; set; }
    }
}
