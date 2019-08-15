using System.Collections.Generic;
using EncoreTickets.SDK.EntertainApi.Model;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.Tests.EntertainApi.Models
{
    internal class EntertainApiResponseTests
    {
        [Test]
        public void EntertainApi_Response_Constructor_InitializesCorrectly()
        {
            var customer = new Response();
            Assert.IsNotNull(customer.Customers);
            Assert.IsNotNull(customer.Reservations);
            Assert.IsNotNull(customer.CurrentAvailabilityUrl);
            Assert.IsNotNull(customer.Status);
        }

        [TestCase(true, "10/10/1970", true)]
        [TestCase(true, "10/10/2970", false)]
        [TestCase(false, "10/10/1970", false)]
        [TestCase(false, "10/10/2970", false)]
        public void EntertainApi_Response_HasExpiredTickets_ReturnsCorrect(bool enta, string basketExpiry, bool result)
        {
            var response = new Response
            {
                Reservations = new List<Reservation>
                {
                    new Reservation
                    {
                        basketItemHistory = new BasketItemHistory
                        {
                            enta = enta,
                            basketExpiry = TestHelper.ConvertTestArgumentToDateTime(basketExpiry)
                        }
                    }
                },
            };
            Assert.AreEqual(result, response.HasExpiredTickets());
        }

        [TestCase("Test", null, true)]
        [TestCase(null, "Test", true)]
        [TestCase(null, null, false)]
        public void EntertainApi_Response_ErrorHasOccured_ReturnsCorrect(string errorMessage, string errorSeverity, bool result)
        {
            var response = new Response
            {
                ErrorMessage = errorMessage,
                ErrorSeverity = errorSeverity
            };
            Assert.AreEqual(result, response.ErrorHasOccured());
        }
    }
}
