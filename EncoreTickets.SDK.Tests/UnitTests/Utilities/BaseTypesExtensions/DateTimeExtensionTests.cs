using EncoreTickets.SDK.Tests.Helpers;
using EncoreTickets.SDK.Utilities.BaseTypesExtensions;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.UnitTests.Utilities.BaseTypesExtensions
{
    internal class DateTimeExtensionTests
    {
        [TestCase("12/02/2008", "20081202")]
        [TestCase("2/28/2000", "20000228")]
        public void ToEncoreDate_ReturnsCorrectly(string dateStr, string expected)
        {
            var date = TestHelper.ConvertTestArgumentToDateTime(dateStr);

            var result = date.ToEncoreDate();

            Assert.AreEqual(expected, result);
        }

        [TestCase("12/02/2008 12:34", "1234")]
        [TestCase("2/28/2000 23:59", "2359")]
        public void ToEncoreTime_ReturnsCorrectly(string dateStr, string expected)
        {
            var date = TestHelper.ConvertTestArgumentToDateTime(dateStr);

            var result = date.ToEncoreTime();

            Assert.AreEqual(expected, result);
        }
    }
}
