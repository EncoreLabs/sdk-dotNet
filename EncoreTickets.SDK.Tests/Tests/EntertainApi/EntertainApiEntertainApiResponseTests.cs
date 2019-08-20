using System.Collections.Generic;
using EncoreTickets.SDK.EntertainApi;
using EncoreTickets.SDK.EntertainApi.Model;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.Tests.EntertainApi
{
    internal class EntertainApiEntertainApiResponseTests
    {
        [Test]
        public void EntertainApi_Customer_Constructor_InitializesCorrectly()
        {
            var item = new EntertainApiResponse();
            Assert.IsNotNull(item.customers);
            Assert.IsNotNull(item.reservations);
            Assert.IsNotNull(item.status);
        }

        [TestCase(true, "10/10/1970", true)]
        [TestCase(true, "10/10/2970", false)]
        [TestCase(false, "10/10/1970", false)]
        [TestCase(false, "10/10/2970", false)]
        public void EntertainApi_EntertainApiResponse_HasExpiredTickets_ReturnsCorrect(bool enta, string basketExpiry, bool result)
        {
            var response = new EntertainApiResponse
            {
                reservations = new List<Reservation>
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
        public void EntertainApi_EntertainApiResponse_ErrorHasOccured_IsCorrect(string errorMessage, string errorSeverity, bool result)
        {
            var response = new EntertainApiResponse
            {
                errorMessage = errorMessage,
                errorSeverity = errorSeverity
            };
            Assert.AreEqual(result, response.ErrorHasOccured());
        }
    }
}
