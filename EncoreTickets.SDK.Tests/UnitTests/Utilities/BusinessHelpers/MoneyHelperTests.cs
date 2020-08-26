using System.Threading;
using EncoreTickets.SDK.Tests.Helpers;
using EncoreTickets.SDK.Utilities.BusinessHelpers;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.UnitTests.Utilities.BusinessHelpers
{
    [TestFixture]
    internal class MoneyHelperTests
    {
        [SetUp]
        public void Setup()
        {
            Thread.CurrentThread.CurrentCulture = TestHelper.Culture;
        }

        [TestCase(1000, 2, 10)]
        [TestCase(1000, 3, 1)]
        [TestCase(1000, 4, 0.1)]
        [TestCase(1000, 6, 0.001)]
        [TestCase(1000, null, 10)]
        [TestCase(1000, 0, 1000)]
        [TestCase(-1000, 2, -10)]
        [TestCase(-999999999, null, -9999999.99)]
        public void ConvertFromIntRepresentationToDecimal_ReturnsCorrectly(int sourceAmount, int? decimalPlaces, decimal expected)
        {
            var actual = MoneyHelper.ConvertFromIntRepresentationToDecimal(sourceAmount, decimalPlaces);

            Assert.AreEqual(expected, actual);
        }

        [TestCase(10, 2, 1000)]
        [TestCase(1, 3, 1000)]
        [TestCase(0.1, 4, 1000)]
        [TestCase(0.001, 6, 1000)]
        [TestCase(10, null, 1000)]
        [TestCase(1000, 0, 1000)]
        [TestCase(-10, 2, -1000)]
        [TestCase(-9999999.99, null, -999999999)]
        public void ConvertFromDecimalRepresentationToInt_ReturnsCorrectly(decimal sourceAmount, int? decimalPlaces, int expected)
        {
            var actual = MoneyHelper.ConvertFromDecimalRepresentationToInt(sourceAmount, decimalPlaces);

            Assert.AreEqual(expected, actual);
        }

        [TestCase(10, 2, "10.00")]
        [TestCase(1, 3, "1.000")]
        [TestCase(0.1, 4, "0.1000")]
        [TestCase(0.001, 6, "0.001000")]
        [TestCase(10, null, "10.00")]
        [TestCase(1000, 0, "1000")]
        [TestCase(-10, 2, "-10.00")]
        [TestCase(-9999999.99, null, "-9999999.99")]
        [TestCase(9999999.9999, 2, "10000000.00")]
        [TestCase(123123.123123, null, "123123.12")]
        [TestCase(123123.129, null, "123123.13")]
        public void ConvertFromDecimalRepresentationToString_ReturnsCorrectly(decimal sourceAmount, int? decimalPlaces, string expected)
        {
            var actual = MoneyHelper.ConvertFromDecimalRepresentationToString(sourceAmount, decimalPlaces);

            Assert.AreEqual(expected, actual);
        }
    }
}
