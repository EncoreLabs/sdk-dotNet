using EncoreTickets.SDK.Utilities;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.UnitTests.Utilities
{
    [TestFixture]
    internal class HashHelperTests
    {
        [TestCase("24588862", "bf414cad7f3bb93106947d0268853018")]
        public void CreateMd5Hash_ReturnsCorrectData(string source, string expected)
        {
            var actual = HashHelper.CreateMd5Hash(source);

            Assert.AreEqual(expected, actual);
        }
    }
}