using EncoreTickets.SDK.Tests.Helpers;
using EncoreTickets.SDK.Utilities.BusinessHelpers;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.UnitTests.Utilities.BusinessHelpers
{
    internal class PerformanceHelperTests
    {
        [TestCase("12/02/2008 12:34:56", 19, "M")]
        [TestCase("12/02/2008 19:34:56", 19, "E")]
        [TestCase("12/02/2008 19:00:00", 19, "E")]
        [TestCase("12/02/2008 18:59:59", 19, "M")]
        [TestCase("2/28/2000 23:59", 17, "E")]
        [TestCase("2/28/2000 17:59", 17, "E")]
        [TestCase("2/28/2000 16:59", 17, "M")]
        public void GetPerformanceType_ReturnsCorrectly(string dateStr, int referenceHour, string expected)
        {
            var date = TestHelper.ConvertTestArgumentToDateTime(dateStr);

            var result = PerformanceHelper.GetPerformanceType(date, referenceHour);

            Assert.AreEqual(expected, result);
        }
    }
}
