using System.Linq;
using EncoreTickets.SDK.Payment.Models;

namespace EncoreTickets.SDK.Payment.Extensions
{
    public static class OrderExtension
    {
        public static bool HasSuccessfulPayment(this Order order)
            => order.Payments.Any(p => p.IsSuccessfulPayment());
    }
}