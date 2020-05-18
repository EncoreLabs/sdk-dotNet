using EncoreTickets.SDK.Utilities.Encoders;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.UnitTests.Utilities.Encoders
{
    [TestFixture]
    internal class Base64EncoderTests
    {
        [TestCase("some example text", "c29tZSBleGFtcGxlIHRleHQ=")]
        [TestCase(null, "")]
        [TestCase("", "")]
        public void Encode_ReturnsCorrectData(string source, string expected)
        {
            var encoder = new Base64Encoder();

            var actual = encoder.Encode(source);

            Assert.AreEqual(expected, actual);
        }

        [TestCase("c29tZSBleGFtcGxlIHRleHQ=", "some example text")]
        [TestCase(null, "")]
        [TestCase("", "")]
        public void Decode_ReturnsCorrectText(string source, string expected)
        {
            var encoder = new Base64Encoder();

            var actual = encoder.Decode(source);

            Assert.AreEqual(expected, actual);
        }
    }
}
