using EncoreTickets.SDK.EntertainApi;
using RestSharp;
using System;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Xml.XPath;
using EncoreTickets.SDK.EntertainApi.Model;

namespace EncoreTickets.SDK.Basket
{
    /// <summary>
    /// Wrapper class for the Basket service API
    /// </summary>
    public class BasketServiceApi : BaseEntertainApi
    {
        /// <summary>
        /// Default constructor for the Basket service
        /// </summary>
        /// <param name="context"></param>
        public BasketServiceApi(ApiContext context) : base(context, "inventory.{0}.aws.encoretix.co.uk/api/")
        {
        }

        public EntertainApiResponse AddToShoppingBasket(SeatInfo seatInfo)
        {
            var shoppingBasket = new ShoppingBasket(); // GetShoppingBasket();
            var requestBody = new AddToBasketRequest
            {
                product = new Product
                {
                    id = seatInfo.showId,
                    type = "show",
                    venue = new Venue { id = seatInfo.venueId },
                    performance = string.IsNullOrEmpty(seatInfo.eveMat) ? new Performance() : new Performance { type = seatInfo.eveMat.ToUpper() },
                    date = seatInfo.date.HasValue ? seatInfo.date.Value.ToString("yyyy-MM-dd") : "",
                    quantity = seatInfo.noTickets,
                    seat = new Seat { key = seatInfo.seatKey },
                    startFrom = seatInfo.startFrom
                }
            };

            RequestMethod requestMethod;

            if (shoppingBasket != null && shoppingBasket.IsValid())
            {
                requestBody.transaction = new Transaction { Reference = shoppingBasket.Id, Password = shoppingBasket.Password };
                requestMethod = RequestMethod.Put;
            }
            else
            {
                shoppingBasket = new ShoppingBasket();
                requestMethod = RequestMethod.Post;
            }

            var restClientParameters = new RestClientParameters
            {
                RequestUrl = @"AddConfigurationSettings.GetConfigurationSetting('EncoreAPIAddToBasket')",
                RequestBody = requestBody,
                RequestMethod = requestMethod
            };

            var restResponse = _restClientWrapper.Execute(restClientParameters);
            var response = GetResponse(restResponse);

            if (!response.ErrorHasOccured())
            {
                //if (ConfigurationSettings.GetConfigurationSetting("EnableEntaSeatTimer") == "1")
                //{
                    var basketItemHistory = this.GetBasketItemHistory(seatInfo, response.reservations);

                    if (basketItemHistory != null)
                        shoppingBasket.BasketItemHistories.Add(basketItemHistory);
                //}

                //UpdateShoppingBasket(shoppingBasket, response);
            }

            return response;
        }

        private EntertainApiResponse GetResponse(IRestResponse restResponse, bool getPostageCosts = false, bool getCustomer = false, bool getRedirectUrl = false, bool isMobileDevice = false)
        {
            var response = ValidateRestReponse(restResponse);

            if (!response.ErrorHasOccured())
            {
                var document = GetXDocumentFromRestResponse(restResponse);
                var root = document.Element("basket") ?? document.Element("booking");

                if (root != null)
                {
                    // Transaction
                    var transaction = root.Element("transaction");

                    if (transaction != null)
                    {
                        response.bookingId = transaction.Attribute("reference").Value;
                        response.password = transaction.Element("password").Value;
                    }

                    // Reservation details
                    var reservations = root.XPathSelectElements("reservations/reservation").ToList();

                    response.reservations = GetReservations(reservations);

                    // Basket expiry
                    //if (response.Reservations.Any())
                    //    response.BasketExpiry = GetBaskeyExpiry(root);

                    // Basket total
                    var total = root.Element("total");

                    response.totalPrice = total != null
                        ? decimal.Parse(total.Value)
                        : response.reservations.Sum(r => r.total);

                    // Calculate commision
                    //if (response.TotalPrice > 0)
                    //{
                    //    response.TotalPriceCommission = Math.Round(response.TotalPrice * commision, 0);
                    //}
                    response.totalPriceCommission = GetBookingCommission(response);

                    // Total seats
                    response.totalSeats = response.reservations.Sum(r => int.Parse(r.noOfTickets));

                    // Postage details
                    if (getPostageCosts)
                    {
                        response.postageCost = GetPostage(reservations);

                        // The total element is only returned on the confirmation page at which point the total includes the cost of all shows plus postage
                        // Before that the total element only amounts to the cost of all shows without postage 
                        response.totalPriceBeforePostage = total != null
                            ? response.totalPrice - response.postageCost
                            : response.totalPrice;

                        var collectAtBoxOffice = new Decimal(1); // decimal.Parse(ConfigurationSettings.GetConfigurationSetting("CollectAtBoxOffice"));
                        var postToCustomer = new Decimal(1); // decimal.Parse(ConfigurationSettings.GetConfigurationSetting("PostToCustomer"));

                        response.totalPriceCollectAtBoxOffice = response.totalPriceBeforePostage + collectAtBoxOffice;
                        response.totalPricePostToCustomer = response.totalPriceBeforePostage + postToCustomer;
                    }

                    // Customer details
                    if (getCustomer)
                    {
                        // Collection method
                        var collectionMethod = root.XPathSelectElement("collectionMethod") != null
                                                   ? root.XPathSelectElement("collectionMethod").Value.Trim().ToLower()
                                                   : string.Empty;

                        var customer = root.Element("customer");

                        if (customer != null)
                            response.customers = GetCustomer(customer, collectionMethod);
                    }

                    // Redirect url
                    if (getRedirectUrl)
                    {
                        if (transaction != null)
                        {
                            var template = @"ConfigurationSettings.GetConfigurationSetting('EncoreAPIPaymentGatewayUrl')";
                            var redirectUrl = string.Format(template, transaction.Attribute("reference").Value, transaction.Element("password").Value, isMobileDevice ? "mobile" : "");

                            response.redirectUrl = redirectUrl;
                        }
                    }

                    response.bookingTime = DateTime.Now.ToString("HH:mm dddd, d MMM yyyy");
                }
            }

            return response;
        }

        protected decimal GetPostage(IEnumerable<XElement> reservations)
        {
            decimal postage = 0;
            var postageProductId = "-1"; // ConfigurationSettings.GetConfigurationSetting("PostageProductId");

            foreach (var reservation in reservations)
            {
                var product = reservation.Element("product");

                if (product != null)
                {
                    if (product.Attribute("id").Value.Trim().ToLower() == postageProductId)
                    {
                        var price = reservation.Element("price");

                        if (price != null)
                        {
                            var total = price.Element("total");

                            postage = total != null ? decimal.Parse(total.Value.Trim()) : Decimal.MinValue;
                        }
                    }
                }
            }

            return postage;
        }

        
    }
}
