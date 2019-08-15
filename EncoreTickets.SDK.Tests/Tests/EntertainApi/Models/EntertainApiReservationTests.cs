using EncoreTickets.SDK.EntertainApi.Model;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.Tests.EntertainApi.Models
{
    internal class EntertainApiReservationTests
    {
        [Test]
        public void EntertainApi_Reservation_Enta_IfBasketItemHistoryDoesNotExist_IsFalse()
        {
            var reservation = new Reservation();
            Assert.AreEqual(reservation.Enta, false);
        }

        [TestCase(true, true)]
        [TestCase(false, false)]
        public void EntertainApi_Reservation_Enta_IfBasketItemHistoryExists_IsCorrect(bool enta, bool result)
        {
            var reservation = new Reservation
            {
                basketItemHistory = new BasketItemHistory
                {
                    enta = enta,
                }
            };
            Assert.AreEqual(result, reservation.Enta);
        }

        [Test]
        public void EntertainApi_Reservation_ExpiredBasketItemHistory_IfBasketItemHistoryDoesNotExist_IsFalse()
        {
            var reservation = new Reservation();
            Assert.AreEqual(false, reservation.ExpiredBasketItemHistory);
        }

        [TestCase(true, "10/10/1970", true)]
        [TestCase(true, "10/10/2970", false)]
        [TestCase(false, "10/10/1970", false)]
        [TestCase(false, "10/10/2970", false)]
        public void EntertainApi_Reservation_ExpiredBasketItemHistory_IfBasketItemHistoryExists_IsCorrect(
            bool enta, string basketExpiry, bool result)
        {
            var reservation = new Reservation
            {
                basketItemHistory = new BasketItemHistory
                {
                    enta = enta,
                    basketExpiry = TestHelper.ConvertTestArgumentToDateTime(basketExpiry)
                }
            };
            Assert.AreEqual(result, reservation.ExpiredBasketItemHistory);
        }

        [TestCase(2, 1, "en-UK", "0.5")]
        [TestCase(2, 1, "fr-FR", "0,5")]
        [TestCase(80.80, 20.20, "en-US", "0.75")]
        [TestCase(1, 1, "en-UK", "")]
        [TestCase(1, 0, "en-UK", "")]
        [TestCase(0, 0, "en-UK", "")]
        public void EntertainApi_Reservation_SavingAsPercentageFormatted_IsCorrect(decimal faceValue, decimal price, string cultureName, string result)
        {
            var reservation = new Reservation
            {
                facevalue = faceValue,
                price = price
            };
            TestHelper.SetCultureGlobally(cultureName);
            Assert.AreEqual(result, reservation.SavingAsPercentageFormatted);
        }

        [TestCase("test", "test", "test:test")]
        [TestCase("test", " t e s t ", "test:test")]
        [TestCase("", "", ":")]
        public void EntertainApi_Reservation_Tag_IsCorrect(string blockId, string block, string result)
        {
            var reservation = new Reservation
            {
                block = block,
                blockId = blockId
            };
            Assert.AreEqual(result, reservation.Tag);
        }
    }
}
