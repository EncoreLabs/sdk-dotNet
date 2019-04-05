using EncoreTickets.SDK.EntertainApi.Model;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Xml.Linq;


namespace EncoreTickets.SDK.EntertainApi
{
    public abstract class BaseEntertainApi : BaseApi
    {
        protected readonly RestClientWrapper _restClientWrapper;
  
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="host"></param>
        protected BaseEntertainApi(ApiContext context, string host) : base(context, host)
        {
        }

        protected BasketItemHistory GetBasketItemHistory(SeatInfo seatInfo, List<Reservation> reservations)
        {
            if (reservations.Any())
            {
                var reservation = reservations.LastOrDefault(r => r.showId == seatInfo.showId && r.venueId == seatInfo.venueId && r.performance == seatInfo.date && r.noOfTickets == seatInfo.noTickets 
                && r.blockId == seatInfo.blockId);

                if (reservation != null)
                {
                    // var venue = new Venue() { id = int.Parse(seatInfo.venueId) };
                    var timeout = 2;  //venue.RUT

                    var basketItemHistory = new BasketItemHistory
                    {
                        reservationId = reservation.reservationId,
                        showId = reservation.showId,
                        venueId = reservation.venueId,
                        date = reservation.performance,
                        quantity = reservation.noOfTickets,
                        blockId = reservation.blockId,
                        seats = reservation.seats,
                        enta = seatInfo.seatKey.ToLower().Contains("enta"),
                        availabilityUrl = seatInfo.availabilityUrl,
                        basketExpiry = seatInfo.seatKey.ToLower().Contains("enta") ? DateTime.Now.AddSeconds(timeout) : DateTime.MinValue
                    };

                    return basketItemHistory;
                }
            }

            return null;
        }

        protected List<Reservation> GetReservations(IEnumerable<XElement> elements)
        {
            var flexiticketProductId = "-2"; // ConfigurationSettings.GetConfigurationSetting("FlexiticketProductId");
            var postageProductId = "-1"; // ConfigurationSettings.GetConfigurationSetting("PostageProductId");
            var reservations = new List<Reservation>();

            foreach (var reservation in elements)
            {
                var product = reservation.Element("product");

                if (product != null)
                {
                    var productId = product.Attribute("id").Value.Trim();

                    if (productId != flexiticketProductId && productId != postageProductId)
                    {
                        var reservationToAdd = new Reservation
                        {
                            reservationId = reservation.Attribute("id") != null ? reservation.Attribute("id").Value.Trim() : string.Empty,
                            showId = reservation.Element("product") != null ? reservation.Element("product").Attribute("id").Value.Trim() : string.Empty,
                            showName = reservation.Element("product") != null ? reservation.Element("product").Value.Trim() : string.Empty,
                            venueId = reservation.Element("venue") != null ? reservation.Element("venue").Attribute("id").Value.Trim() : string.Empty,
                            venueName = reservation.Element("venue") != null ? reservation.Element("venue").Value.Trim() : string.Empty,
                            performance = reservation.Element("date") != null ? DateTime.Parse(reservation.Element("date").Attribute("dt").Value.Trim()).ToUniversalTime() : DateTime.MinValue
                        };

                        // Seating details
                        var block = reservation.Element("block");

                        if (block != null)
                        {
                            reservationToAdd.blockId = block.Attribute("id").Value.Trim();
                            reservationToAdd.block = block.Element("title").Value.Trim();
                            reservationToAdd.seats = block.Element("seats").Value.Trim();
                        }

                        // Pricing details
                        var price = reservation.Element("price");

                        if (price != null)
                        {
                            reservationToAdd.noOfTickets = price.Element("quantity").Value.Trim();
                            reservationToAdd.facevalue = price.Element("faceValue") != null ? decimal.Parse(price.Element("faceValue").Value.Trim()) : Decimal.MinValue;
                            reservationToAdd.price = price.Element("salePrice") != null ? decimal.Parse(price.Element("salePrice").Value.Trim()) : Decimal.MinValue;
                            reservationToAdd.total = price.Element("total") != null ? decimal.Parse(price.Element("total").Value.Trim()) : Decimal.MinValue;
                        }

                        // Further seating details
                        reservationToAdd.block = "GetSeatBlockDescription(reservationToAdd.block, reservationToAdd.Tag)";

                        // Get show details from local db
                        //if (!string.IsNullOrEmpty(reservationToAdd.showId))
                        //{
                        //    var show = _showsRepository.GetShowByMerchantShowId(int.Parse(reservationToAdd.showId));

                        //    reservationToAdd.showName = show.UIName;
                        //    reservationToAdd.showImageUrl = show.CoverImageSrc;
                        //    reservationToAdd.hideFaceValue = show.HideFaceValue;
                        //    reservationToAdd.commission = show.Commission;
                        //}

                        // Get venue details from local db
                        //if (!String.IsNullOrEmpty(reservationToAdd.venueId))
                        //{
                        //    var venue = _venuesRepository.GetVenueByMerchantVenueId(int.Parse(reservationToAdd.venueId));

                        //    reservationToAdd.venueAddress = venue.Address;
                        //    reservationToAdd.venueMap = venue.GoogleMapsUrl;
                        //}

                        reservations.Add(reservationToAdd);
                    }
                }
            }

            return reservations;
        }

        protected decimal GetBookingCommission(EntertainApiResponse response)
        {
            decimal bookingTotal = 0;
            decimal globalCommission = new Decimal(0.10); // decimal.Parse(0.10); // decimal.Parse(ConfigurationSettings.GetConfigurationSetting("Commision"));

            foreach (var reservation in response.reservations)
            {
                if (reservation.total > 0)
                {
                    var commision = reservation.commission.HasValue
                        ? Math.Round(reservation.total * reservation.commission.Value, 0)
                        : Math.Round(reservation.total * globalCommission, 0);

                    bookingTotal += commision;
                }
            }

            return bookingTotal;
        }

        protected List<Customer> GetCustomer(XContainer customer, string collectionMethod)
        {
            var customers = new List<Customer>();
            var customerToAdd = new Customer
            {
                title = customer.Element("title").Value,
                firstName = customer.Element("firstName").Value,
                surname = customer.Element("lastName").Value,
                email = customer.Element("email").Value,
                phoneNo = customer.Element("phone").Value
            };

            var address = customer.Element("address");

            if (address != null)
            {
                customerToAdd.address1 = address.Element("line1").Value;
                customerToAdd.address2 = address.Element("line2").Value;
                customerToAdd.city = address.Element("city").Value;
                customerToAdd.postcode = address.Element("postcode").Value;
                customerToAdd.country = address.Element("country").Value;
            }

            customerToAdd.ticketDispatch = collectionMethod == "c" ? "Collect tickets at the theatre box office" : "Tickets will be posted to you";
            customers.Add(customerToAdd);

            return customers;
        }

        protected EntertainApiResponse ValidateRestReponse(IRestResponse restResponse)
        {
            var response = new EntertainApiResponse();

            if (!_restClientWrapper.IsGoodResponse(restResponse))
            {
                response.errorSeverity = restResponse.ErrorException != null ? restResponse.ErrorException.ToString() : restResponse.StatusDescription;
                response.errorMessage = string.IsNullOrEmpty(restResponse.ErrorMessage) ? restResponse.StatusDescription : restResponse.ErrorMessage;
            }

            CheckXDocumentForErrors(restResponse, response);

            return response;
        }

        protected void CheckXDocumentForErrors(IRestResponse restResponse, EntertainApiResponse response)
        {
            var document = GetXDocumentFromRestResponse(restResponse);
            var error = document.Element("error");

            if (error != null)
            {
                var state = error.Attribute("state");

                if (state != null)
                    response.errorSeverity += " - " + state.Value;

                var message = error.Element("message");

                if (message != null)
                    response.errorMessage += " - " + message.Value;
            }
        }

        protected XDocument GetXDocumentFromRestResponse(IRestResponse response)
        {
            return !string.IsNullOrEmpty(response.Content) && response.Content.Contains("<?xml version")
                ? XDocument.Parse(response.Content)
                : new XDocument();
        }
    }

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
            return true; // Reservations.Any(reservation => reservation.ExpiredBasketItemHistory);
        }

        public bool ErrorHasOccured()
        {
            return !string.IsNullOrEmpty(errorMessage) || !string.IsNullOrEmpty(errorSeverity);
        }

        #endregion
    }
}
