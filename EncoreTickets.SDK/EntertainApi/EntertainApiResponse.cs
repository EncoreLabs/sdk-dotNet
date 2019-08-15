using System.Collections.Generic;
using System.Linq;
using EncoreTickets.SDK.EntertainApi.Model;

namespace EncoreTickets.SDK.EntertainApi
{
    /// <summary>
    /// Entertain API response
    /// </summary>
    public class EntertainApiResponse
    {
        public string bookingId { get; set; }
        public string bookingTime { get; set; }
        public bool confirmationDisplayed { get; set; }
        public string currentAvailabilityUrl { get; set; }
        public List<Customer> customers { get; set; }
        public string errorMessage { get; set; }
        public string errorSeverity { get; set; }
        public string password { get; set; }
        public decimal postageCost { get; set; }
        public string redirectUrl { get; set; }
        public List<Reservation> reservations { get; set; }
        public bool shoppingBasketExpired { get; set; }
        public bool showCheckoutButtons { get; set; }
        public string status { get; set; }
        public decimal totalPrice { get; set; }
        public decimal totalPriceBeforePostage { get; set; }
        public decimal totalPriceCollectAtBoxOffice { get; set; }
        public decimal totalPriceCommission { get; set; }
        public decimal totalPricePostToCustomer { get; set; }
        public decimal totalPricePostToCustomerInternational { get; set; }
        public int totalSeats { get; set; }

        public EntertainApiResponse()
        {
            currentAvailabilityUrl = string.Empty;
            customers = new List<Customer>();
            reservations = new List<Reservation>();
            status = "1";
        }

        #region Helper Properties & Methods

        public bool HasExpiredTickets()
        {
            return reservations.Any(reservation => reservation.ExpiredBasketItemHistory);
        }

        public bool ErrorHasOccured()
        {
            return !string.IsNullOrEmpty(errorMessage) || !string.IsNullOrEmpty(errorSeverity);
        }

        #endregion
    }
}