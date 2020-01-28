using EncoreTickets.SDK.Tests.Helpers;
using EncoreTickets.SDK.Utilities.BaseTypesExtensions;
using FluentAssertions.Common;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.UnitTests.Utilities.BaseTypesExtensions
{
    internal class DateTimeOffsetExtensionTests
    {
        [TestCase("12/02/2008", "2008-12-02")]
        [TestCase("2/28/2000", "2000-02-28")]
        [TestCase("12/31/1999 23:59:59", "1999-12-31")]
        public void ToReadableEncoreDate_ReturnsCorrectly(string dateStr, string expected)
        {
            var date = TestHelper.ConvertTestArgumentToDateTime(dateStr).ToDateTimeOffset();

            var result = date.ToReadableEncoreDate();

            Assert.AreEqual(expected, result);
        }

        [TestCase("12/02/2008 12:34", "12:34")]
        [TestCase("2/28/2000 23:59", "23:59")]
        public void ToReadableEncoreTime_ReturnsCorrectly(string dateStr, string expected)
        {
            var date = TestHelper.ConvertTestArgumentToDateTime(dateStr).ToDateTimeOffset();

            var result = date.ToReadableEncoreTime();

            Assert.AreEqual(expected, result);
        }

        [TestCase("12/02/2008 12:34:56", 19, "M")]
        [TestCase("12/02/2008 19:34:56", 19, "E")]
        [TestCase("12/02/2008 19:00:00", 19, "E")]
        [TestCase("12/02/2008 18:59:59", 19, "M")]
        [TestCase("2/28/2000 23:59", 17, "E")]
        [TestCase("2/28/2000 17:59", 17, "E")]
        [TestCase("2/28/2000 16:59", 17, "M")]
        public void GetPerformanceType_ReturnsCorrectly(string dateStr, int referenceHour, string expected)
        {
            var dateOffset = TestHelper.ConvertTestArgumentToDateTime(dateStr).ToDateTimeOffset();

            var result = dateOffset.GetPerformanceType(referenceHour);

            Assert.AreEqual(expected, result);
        }
    }
}
