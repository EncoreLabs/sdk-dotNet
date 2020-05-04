using System;
using System.Collections.Generic;
using EncoreTickets.SDK.Utilities.CommonModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace EncoreTickets.SDK.Payment.Models
{
    /// <summary>
    /// Stores payment details
    /// </summary>
    public class Payment : IEntityWithStatus
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
        /// Gets or sets payment status.
        /// </summary>
        public string Status { get; set; }

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
        [JsonProperty("paymentServiceProvider")]
        public string PspName { get; set; }

        /// <summary>
        /// Gets or sets name of the account for the Payment Service Provider.
        /// </summary>
        [JsonProperty("merchantAccount")]
        public string PspMerchantAccount { get; set; }
    }
}