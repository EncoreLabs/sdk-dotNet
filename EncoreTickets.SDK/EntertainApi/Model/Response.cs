using System.Collections.Generic;
using System.Linq;

namespace EncoreTickets.SDK.EntertainApi.Model
{
    public class Response
    {
        public string BookingId { get; set; }
        public string BookingTime { get; set; }
        public bool ConfirmationDisplayed { get; set; }
        public string CurrentAvailabilityUrl { get; set; }
        public List<Customer> Customers { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorSeverity { get; set; }
        public string Password { get; set; }
        public decimal PostageCost { get; set; }
        public string RedirectUrl { get; set; }
        public List<Reservation> Reservations { get; set; }
        public bool ShoppingBasketExpired { get; set; }
        public bool ShowCheckoutButtons { get; set; }
        public string Status { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal TotalPriceBeforePostage { get; set; }
        public decimal TotalPriceCollectAtBoxOffice { get; set; }
        public decimal TotalPriceCommission { get; set; }
        public decimal TotalPricePostToCustomer { get; set; }
        public decimal TotalPricePostToCustomerInternational { get; set; }
        public int TotalSeats { get; set; }

        public Response()
        {
            CurrentAvailabilityUrl = string.Empty;
            Customers = new List<Customer>();
            Reservations = new List<Reservation>();
            Status = "1";
        }

        #region Helper Properties & Methods

        public bool HasExpiredTickets()
        {
            return Reservations.Any(reservation => reservation.ExpiredBasketItemHistory);
        }

        public bool ErrorHasOccured()
        {
            return !string.IsNullOrEmpty(ErrorMessage) || !string.IsNullOrEmpty(ErrorSeverity);
        }

        #endregion
    }
}