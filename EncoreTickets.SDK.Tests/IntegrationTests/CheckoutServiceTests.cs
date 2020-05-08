using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using EncoreTickets.SDK.Api.Models;
using EncoreTickets.SDK.Api.Results.Exceptions;
using EncoreTickets.SDK.Checkout;
using EncoreTickets.SDK.Checkout.Models;
using EncoreTickets.SDK.Checkout.Models.RequestModels;
using EncoreTickets.SDK.Inventory;
using EncoreTickets.SDK.Tests.Helpers;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.IntegrationTests
{
    [TestFixture]
    internal class CheckoutServiceTests
    {
        private IConfiguration configuration;
        private CheckoutServiceApi service;
        private ApiContext context;

        [SetUp]
        public void SetupState()
        {
            configuration = ConfigurationHelper.GetConfiguration();
            context = new ApiContext(Environments.QA);
            service = new CheckoutServiceApi(context);
        }

        #region Checkout

        [Test]
        public void Checkout_Successful()
        {
            var parameters = new BookingParameters
            {
                Reference = configuration["Checkout:TestBookingReference"],
                ChannelId = configuration["Checkout:TestChannelId"],
                Shopper = new Shopper
                {
                    Email = "user@example.com",
                    Title = "Miss",
                    FirstName = "Shawn",
                    LastName = "Butler",
                    TelephoneNumber = "07882571812",
                    ExternalId = "8263702"
                },
                BillingAddress = new Address
                {
                    Line1 = "Barnard's Inn",
                    Line2 = "86 Fetter Lane",
                    PostalCode = "EC4A 1EN",
                    City = "London",
                    CountryCode = "GB",
                    CountryName = "United Kingdom",
                    StateOrProvince = "NY"
                },
                Origin = "https://example.com",
                RedirectUrl = "https://example.com",
                DeliveryMethod = DeliveryMethod.C,
                DeliveryCharge = 245,
                RecipientName = "Mr. Someone Else",
                GiftVoucherMessage = "Happy Birthday to you.",
                DeliveryAddress = new Address
                {
                    Line1 = "Barnard's Inn",
                    Line2 = "86 Fetter Lane",
                    PostalCode = "EC4A 1EN",
                    City = "London",
                    CountryCode = "GB",
                    CountryName = "United Kingdom",
                    StateOrProvince = "NY"
                },
                HasFlexiTickets = false,
                PaymentType = PaymentType.Card
            };

            var paymentInfo = service.Checkout(parameters);

            Assert.IsNotNull(paymentInfo.PaymentId);
        }

        [Test]
        public void Checkout_IfParametersAreInvalid_Exception400()
        {
            var invalidParameters = new BookingParameters();

            var exception = Assert.Catch<ApiException>(() =>
            {
                var products = service.Checkout(invalidParameters);
            });

            Assert.AreEqual(HttpStatusCode.BadRequest, exception.ResponseCode);
        }

        [Test]
        public void Checkout_IfBookingIsExpired_Exception400()
        {
            var parameters = new BookingParameters
            {
                Reference = configuration["Checkout:TestExpiredBookingReference"],
                ChannelId = configuration["Checkout:TestChannelId"],
                Shopper = new Shopper
                {
                    Email = "user@example.com",
                    Title = "Miss",
                    FirstName = "Shawn",
                    LastName = "Butler",
                    TelephoneNumber = "07882571812",
                    ExternalId = "8263702"
                },
                BillingAddress = new Address
                {
                    Line1 = "Barnard's Inn",
                    Line2 = "86 Fetter Lane",
                    PostalCode = "EC4A 1EN",
                    City = "London",
                    CountryCode = "GB",
                    CountryName = "United Kingdom",
                    StateOrProvince = "NY"
                },
                Origin = "https://example.com",
                RedirectUrl = "https://example.com",
                DeliveryMethod = DeliveryMethod.C,
                DeliveryCharge = 245,
                RecipientName = "Mr. Someone Else",
                GiftVoucherMessage = "Happy Birthday to you.",
                DeliveryAddress = new Address
                {
                    Line1 = "Barnard's Inn",
                    Line2 = "86 Fetter Lane",
                    PostalCode = "EC4A 1EN",
                    City = "London",
                    CountryCode = "GB",
                    CountryName = "United Kingdom",
                    StateOrProvince = "NY"
                },
                HasFlexiTickets = true,
                PaymentType = PaymentType.Card
            };

            var exception = Assert.Catch<ApiException>(() =>
            {
                var products = service.Checkout(parameters);
            });

            Assert.AreEqual(HttpStatusCode.BadRequest, exception.ResponseCode);
        }

        #endregion
    }
}
