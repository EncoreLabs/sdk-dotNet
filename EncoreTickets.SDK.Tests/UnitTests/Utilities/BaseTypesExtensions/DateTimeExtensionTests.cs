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

        [TestCase("12/02/2008", "2008-12-02")]
        [TestCase("2/28/2000", "2000-02-28")]
        [TestCase("12/31/1999 23:59:59", "1999-12-31")]
        public void ToReadableEncoreDate_ReturnsCorrectly(string dateStr, string expected)
        {
            var date = TestHelper.ConvertTestArgumentToDateTime(dateStr);

            var result = date.ToReadableEncoreDate();

            Assert.AreEqual(expected, result);
        }

        [TestCase("12/02/2008 12:34", "12:34")]
        [TestCase("2/28/2000 23:59", "23:59")]
        public void ToReadableEncoreTime_ReturnsCorrectly(string dateStr, string expected)
        {
            var date = TestHelper.ConvertTestArgumentToDateTime(dateStr);

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
            var date = TestHelper.ConvertTestArgumentToDateTime(dateStr);

            var result = date.GetPerformanceType(referenceHour);

            Assert.AreEqual(expected, result);
        }

        [TestCase("12/02/2008 12:34:56")]
        [TestCase("2/28/2000 23:59")]
        [TestCase("12/31/1999 23:59:59")]
        public void StripSeconds_ReturnsCorrectly(string dateStr)
        {
            var date = TestHelper.ConvertTestArgumentToDateTime(dateStr);

            var result = date.StripSeconds();

            Assert.AreEqual(date.Year, result.Year);
            Assert.AreEqual(date.Month, result.Month);
            Assert.AreEqual(date.Day, result.Day);
            Assert.AreEqual(date.Hour, result.Hour);
            Assert.AreEqual(date.Minute, result.Minute);
            Assert.AreEqual(0, result.Second);
            Assert.AreEqual(0, result.Millisecond);
        }

        [TestCase("12/02/2008 12:34:56", 31)]
        [TestCase("2/02/2008 12:34:56", 29)]
        [TestCase("2/28/2000", 29)]
        [TestCase("2/28/2001", 28)]
        [TestCase("4/28/2000", 30)]
        [TestCase("6/28/2000", 30)]
        [TestCase("9/28/2000", 30)]
        [TestCase("11/28/2000", 30)]
        [TestCase("1/1/1999", 31)]
        [TestCase("3/1/1999", 31)]
        [TestCase("5/1/1999", 31)]
        [TestCase("7/1/1999", 31)]
        [TestCase("8/1/1999", 31)]
        [TestCase("10/1/1999", 31)]
        [TestCase("12/1/1999", 31)]
        [TestCase("1/1/0001", 31)]
        public void GetLastDayOfMonth_IfDateArgument_ReturnsCorrectly(string dateStr, int expected)
        {
            var date = TestHelper.ConvertTestArgumentToDateTime(dateStr);

            var result = date.GetLastDayOfMonth();

            Assert.AreEqual(expected, result);
        }

        [TestCase(12, 2008, 31)]
        [TestCase(2, 2008, 29)]
        [TestCase(2, 2000, 29)]
        [TestCase(2, 2001, 28)]
        [TestCase(4, 2000, 30)]
        [TestCase(6, 2000, 30)]
        [TestCase(9, 2000, 30)]
        [TestCase(11, 2000, 30)]
        [TestCase(1, 1999, 31)]
        [TestCase(3, 1999, 31)]
        [TestCase(5, 1999, 31)]
        [TestCase(7, 1999, 31)]
        [TestCase(8, 1999, 31)]
        [TestCase(10, 1999, 31)]
        [TestCase(12, 1999, 31)]
        [TestCase(1, 1, 31)]
        [TestCase(-1, 1, 0)]
        [TestCase(1, 0, 0)]
        [TestCase(1, -1, 0)]
        [TestCase(1, 10000, 0)]
        [TestCase(1, 9999, 31)]
        [TestCase(13, 9999, 0)]
        public void GetLastDayOfMonth_IfYearAndMonthArguments_ReturnsCorrectly(short month, short year, int expected)
        {
            var result = DateTimeExtension.GetLastDayOfMonth(month, year);

            Assert.AreEqual(expected, result);
        }
    }
}
