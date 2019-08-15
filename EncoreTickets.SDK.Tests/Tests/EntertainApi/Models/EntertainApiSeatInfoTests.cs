using EncoreTickets.SDK.EntertainApi.Model;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.Tests.EntertainApi.Models
{
    internal class EntertainApiSeatInfoTests
    {
        [TestCase("2/2/2020", "02")]
        [TestCase("2/28/2000", "28")]
        public void EntertainApi_SeatInfo_Day_IdDateExists_IsCorrect(string date, string result)
        {
            var item = new SeatInfo
            {
                date = TestHelper.ConvertTestArgumentToDateTime(date)
            };
            Assert.AreEqual(result, item.Day);
        }

        [Test]
        public void EntertainApi_SeatInfo_Day_IdDateDoesNotExist_IsEmpty()
        {
            var item = new SeatInfo();
            Assert.AreEqual("", item.Day);
        }

        [TestCase("2/2/2020", "2020Z02")]
        [TestCase("2/28/2000", "2000Z02")]
        public void EntertainApi_SeatInfo_MonthYear_IdDateExists_IsCorrect(string date, string result)
        {
            var item = new SeatInfo
            {
                date = TestHelper.ConvertTestArgumentToDateTime(date)
            };
            Assert.AreEqual(result, item.MonthYear);
        }

        [Test]
        public void EntertainApi_SeatInfo_MonthYear_IdDateDoesNotExist_IsEmpty()
        {
            var item = new SeatInfo();
            Assert.AreEqual("", item.MonthYear);
        }
    }
}
