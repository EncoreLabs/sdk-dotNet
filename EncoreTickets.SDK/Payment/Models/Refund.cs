using System;
using EncoreTickets.SDK.Utilities.CommonModels;

namespace EncoreTickets.SDK.Payment.Models
{
    /// <summary>
    /// Stores refund details.
    /// </summary>
    public class Refund : IEntityWithStatus
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
        /// Gets or sets PSP reference for the payment.
        /// </summary>
        public string PspReference { get; set; }

        /// <summary>
        /// Gets or sets time of the creation in Payment Service Provider.
        /// </summary>
        public DateTime PspCreatedAt { get; set; }

        /// <summary>
        /// Gets or sets stable ID.
        /// </summary>
        public string StableId { get; set; }

        /// <summary>
        /// Gets or sets amount of the refund.
        /// </summary>
        public Amount Amount { get; set; }

        /// <summary>
        /// Gets or sets reason for the action.
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// Gets or sets current status.
        /// </summary>
        public string Status { get; set; }
    }
}