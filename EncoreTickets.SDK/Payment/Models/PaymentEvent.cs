using System;

namespace EncoreTickets.SDK.Payment.Models
{
    /// <summary>
    /// PaymentEvent entity stores details on all the payment events.
    /// </summary>
    public class PaymentEvent
    {
        /// <summary>
        /// Gets or sets ID.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets time of the creation.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets time of the creation in Payment Service Provider.
        /// </summary>
        public DateTime PspCreatedAt { get; set; }

        /// <summary>
        /// Gets or sets PSP reference for the payment.
        /// </summary>
        public string PspReference { get; set; }

        /// <summary>
        /// Gets or sets payment event type: 'capture', 'authorisation' etc.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets status in boolean.
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// Gets or sets time of an notification update.
        /// </summary>
        public DateTime NotificationUpdatedAt { get; set; }
    }
}
