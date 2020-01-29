using System;
using System.Collections.Generic;

namespace EncoreTickets.SDK.Payment.Models
{
    /// <summary>
    /// Stores payment details
    /// </summary>
    public class Payment
    {
        /// <summary>
        /// Gets or sets ID of the payment.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets time of the payment creation.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets amount of the payment.
        /// </summary>
        public Amount Amount { get; set; }

        /// <summary>
        /// Gets or sets original amount of the payment.
        /// </summary>
        public Amount AmountOriginal { get; set; }

        /// <summary>
        /// Gets or sets events of the payment.
        /// </summary>
        public List<PaymentEvent> Events { get; set; }

        /// <summary>
        /// Gets or sets payment method.
        /// </summary>
        public PaymentMethod PaymentMethod { get; set; }

        /// <summary>
        /// Gets or sets refunds.
        /// </summary>
        public List<Refund> Refunds { get; set; }

        /// <summary>
        /// Gets or sets compensations.
        /// </summary>
        public List<Refund> Compensations { get; set; }

        /// <summary>
        /// Gets or sets name of the Payment Service Provider.
        /// </summary>
        public string PspName { get; set; }

        /// <summary>
        /// Gets or sets name of the account for the Payment Service Provider.
        /// </summary>
        public string PspMerchantAccount { get; set; }
    }
}