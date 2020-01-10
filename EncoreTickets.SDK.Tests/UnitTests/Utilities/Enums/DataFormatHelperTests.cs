using EncoreTickets.SDK.Utilities.Enums;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.UnitTests.Utilities.Enums
{
    internal class DataFormatHelperTests
    {
        [TestCase(DataFormat.Json, "application/json")]
        [TestCase(DataFormat.Xml, "application/xml")]
        public void ToContentType_IfDataFormatExists_ReturnsContentType(DataFormat dataFormat, string expected)
        {
            var actual = DataFormatHelper.ToContentType(dataFormat);

            Assert.AreEqual(expected, actual);
        }

        [TestCase(189)]
        [TestCase(-1)]
        public void ToContentType_IfDataFormatDoesNotExist_ThrowsException(DataFormat dataFormat)
        {
            Assert.Catch(() => DataFormatHelper.ToContentType(dataFormat));
        }
    }
}
