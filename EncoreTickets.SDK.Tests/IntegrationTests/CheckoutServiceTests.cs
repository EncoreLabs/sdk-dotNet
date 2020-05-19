using System.Net;
using EncoreTickets.SDK.Api.Models;
using EncoreTickets.SDK.Api.Results.Exceptions;
using EncoreTickets.SDK.Checkout;
using EncoreTickets.SDK.Checkout.Models;
using EncoreTickets.SDK.Checkout.Models.RequestModels;
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
        public void Checkout_IfPaymentTypeIsCard_Successful()
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
                DeliveryMethod = DeliveryMethod.Collection,
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
        public void Checkout_IfPaymentTypeIsAccount_Successful()
        {
            var (_, _, agentChannel) = GetAgentInfoFromConfig();
            var parameters = new BookingParameters
            {
                Reference = configuration["Checkout:TestBookingReferenceForAgentConfirmation"],
                ChannelId = agentChannel,
                Shopper = new Shopper
                {
                    Email = "agentEmail@mail.com",
                    Title = "Mrs",
                    FirstName = "clientFName",
                    LastName = "clientLName",
                    TelephoneNumber = "123321321321"
                },
                BillingAddress = new Address
                {
                    Line1 = "47-51 Great Suffolk St",
                    Line2 = "",
                    PostalCode = "SE1 0BS",
                    City = "London",
                    CountryCode = "UK"
                },
                RedirectUrl = "http://localhost:8000/",
                DeliveryMethod = DeliveryMethod.Collection,
                PaymentType = PaymentType.Account,
                PaymentId = "111"
            };

            var paymentInfo = service.Checkout(parameters);

            Assert.IsNotNull(paymentInfo.PaymentType);
        }

        [Test]
        public void Checkout_IfParametersAreInvalid_Exception400()
        {
            var invalidParameters = new BookingParameters();

            var exception = Assert.Catch<ApiException>(() =>
            {
                var result = service.Checkout(invalidParameters);
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
                DeliveryMethod = DeliveryMethod.Collection,
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
                var result = service.Checkout(parameters);
            });

            Assert.AreEqual(HttpStatusCode.BadRequest, exception.ResponseCode);
        }

        #endregion

        #region ConfirmBooking

        [Test]
        public void ConfirmBooking_IfAgentBooking_Successful()
        {
            var (agentId, agentPassword, agentChannel) = GetAgentInfoFromConfig();
            var reference = configuration["Checkout:TestBookingReferenceForAgentConfirmation"];
            var parameters = new ConfirmBookingParameters
            {
                ChannelId = agentChannel,
                PaymentId = "agent_payment"
            };

            var result = service.ConfirmBooking(agentId, agentPassword, reference, parameters);

            Assert.IsTrue(result);
        }

        [Test]
        public void ConfirmBooking_IfUsualBooking_Successful()
        {
            var reference = configuration["Checkout:TestBookingReference"];
            var parameters = new ConfirmBookingParameters
            {
                ChannelId = configuration["Checkout:TestChannelId"],
                PaymentId = configuration["Checkout:TestPaymentId"]
            };

            var result = service.ConfirmBooking(reference, parameters);

            Assert.IsTrue(result);
        }

        [Test]
        public void ConfirmBooking_IfPaymentIsNotSet_Exception400()
        {
            var (agentId, agentPassword, agentChannel) = GetAgentInfoFromConfig();
            var reference = configuration["Checkout:TestBookingReferenceForAgentConfirmation"];
            var parameters = new ConfirmBookingParameters
            {
                ChannelId = agentChannel,
            };

            var exception = Assert.Catch<ApiException>(() =>
            {
                var result = service.ConfirmBooking(agentId, agentPassword, reference, parameters);
            });

            Assert.AreEqual(HttpStatusCode.BadRequest, exception.ResponseCode);
        }

        [Test]
        public void ConfirmBooking_IfChannelIsNotSet_Exception400()
        {
            var (agentId, agentPassword, agentChannel) = GetAgentInfoFromConfig();
            var reference = configuration["Checkout:TestBookingReferenceForAgentConfirmation"];
            var parameters = new ConfirmBookingParameters { };

            var exception = Assert.Catch<ApiException>(() =>
            {
                var result = service.ConfirmBooking(agentId, agentPassword, reference, parameters);
            });

            Assert.AreEqual(HttpStatusCode.BadRequest, exception.ResponseCode);
        }

        [Test]
        public void ConfirmBooking_IfBookingIsExpired_Exception400()
        {
            var (agentId, agentPassword, agentChannel) = GetAgentInfoFromConfig();
            var reference = configuration["Checkout:TestExpiredBookingReference"];
            var parameters = new ConfirmBookingParameters
            {
                ChannelId = agentChannel,
            };

            var exception = Assert.Catch<ApiException>(() =>
            {
                var result = service.ConfirmBooking(agentId, agentPassword, reference, parameters);
            });

            Assert.AreEqual(HttpStatusCode.BadRequest, exception.ResponseCode);
        }

        private (string agentId, string agentPassword, string agentChannel) GetAgentInfoFromConfig()
        {
            var agentId = configuration ["Checkout:TestAgentId"];
            var agentPassword = configuration ["Checkout:TestAgentPassword"];
            var channelId = configuration["Checkout:TestAgentChannelId"];
            return (agentId, agentPassword, channelId);
        }

        #endregion
    }
}
