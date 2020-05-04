using System;

namespace EncoreTickets.SDK.Payment.Models
{
    /// <summary>
    /// PaymentMethod entity stores details on the payment method.
    /// </summary>
    public class PaymentMethod
    {
        /// <summary>
        /// Gets or sets type of payment method. We currently support 'card', 'amex', 'paypal'.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets payment method scheme. It can be visa, mc, amex etc. Only used when type is 'card'.
        /// </summary>
        public string Scheme { get; set; }

        /// <summary>
        /// Gets or sets obfuscated card number or account ID. Only used when type is 'card' or 'paypal'..
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Gets or sets expiry date of a card. Only used when type is 'card'.
        /// </summary>
        public DateTime? ExpiryDate { get; set; }

        /// <summary>
        /// Gets or sets holder name of a card or account. Only used when type is 'card' or 'paypal'.
        /// </summary>
        public string HolderName { get; set; }
    }
}