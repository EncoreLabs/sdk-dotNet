using EncoreTickets.SDK.EntertainApi.Model;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.EntertainApi.Models
{
    public class EntertainApiTicketTests
    {
        [TestCase(null, "Mon, 1 Jan 0001")]
        [TestCase("4/16/1999", "Fri, 16 Apr 1999")]
        [TestCase("8/14/2019", "Wed, 14 Aug 2019")]
        public void EntertainApi_Ticket_DateFormatted_IsCorrect(string date, string result)
        {
            var item = new Ticket
            {
                Date = TestHelper.ConvertTestArgumentToDateTime(date),
            };
            Assert.AreEqual(result, item.DateFormatted);
        }

        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase("ENTATEST", true)]
        [TestCase("entaTest", true)]
        [TestCase("_entaTest", false)]
        public void EntertainApi_Ticket_Enta_IsCorrect(string seatKey, bool result)
        {
            var item = new Ticket
            {
                SeatKey = seatKey
            };
            Assert.AreEqual(result, item.Enta);
        }

        [TestCase(23.45, "en-US", "$23.45")]
        [TestCase(16325.62, "en-GB", "£16,325.62")]
        [TestCase(0.899, "es-ES", "0,90 €")]
        [TestCase(45980.899, "en-CA", "$45,980.90")]
        public void EntertainApi_Ticket_FaceValueFormatted_IsCorrect(decimal faceValue, string cultureName, string result)
        {
            var item = new Ticket
            {
                FaceValue = faceValue
            };
            TestHelper.SetCultureGlobally(cultureName);
            Assert.AreEqual(result, item.FaceValueFormatted);
        }

        [TestCase("4", 4)]
        [TestCase("-5674", -5674)]
        [TestCase("2147483647", int.MaxValue)]
        [TestCase("-2147483648", int.MinValue)]
        [TestCase("4.9", 0)]
        [TestCase("", 0)]
        [TestCase(null, 0)]
        [TestCase("test", 0)]
        public void EntertainApi_Ticket_FirstAsInt_IsCorrect(string first, int result)
        {
            var item = new Ticket
            {
                First = first
            };
            Assert.AreEqual(result, item.FirstAsInt);
        }

        [TestCase("4", 4)]
        [TestCase("-5674", -5674)]
        [TestCase("2147483647", int.MaxValue)]
        [TestCase("-2147483648", int.MinValue)]
        [TestCase("4.9", 0)]
        [TestCase("", 0)]
        [TestCase(null, 0)]
        [TestCase("test", 0)]
        public void EntertainApi_Ticket_LastAsInt_IsCorrect(string last, int result)
        {
            var item = new Ticket
            {
                Last = last
            };
            Assert.AreEqual(result, item.LastAsInt);
        }

        [TestCase(23.45, "en-US", "$23.45")]
        [TestCase(16325.62, "en-GB", "£16,325.62")]
        [TestCase(1.999, "fr-FR", "2,00 €")]
        [TestCase(45980.899, "en-NZ", "$45,980.90")]
        public void EntertainApi_Ticket_PriceFormatted_IsCorrect(decimal price, string cultureName, string result)
        {
            var item = new Ticket
            {
                Price = price
            };
            TestHelper.SetCultureGlobally(cultureName);
            Assert.AreEqual(result, item.PriceFormatted);
        }

        [TestCase("test", "test", "test:test")]
        [TestCase("test", " t e s t ", "test:test")]
        [TestCase("", "", ":")]
        public void EntertainApi_Ticket_Tag_IsCorrect(string blockId, string block, string result)
        {
            var item = new Ticket
            {
                Block = block,
                BlockId = blockId
            };
            Assert.AreEqual(result, item.Tag);
        }

        [TestCase(null, "12:00am")]
        [TestCase("4/16/1999", "12:00am")]
        [TestCase("8/14/2019 23:00:10", "11:00pm")]
        public void EntertainApi_Ticket_TimeFormatted_IsCorrect(string date, string result)
        {
            var item = new Ticket
            {
                Date = TestHelper.ConvertTestArgumentToDateTime(date),
            };
            Assert.AreEqual(result, item.TimeFormatted);
        }
    }
}
