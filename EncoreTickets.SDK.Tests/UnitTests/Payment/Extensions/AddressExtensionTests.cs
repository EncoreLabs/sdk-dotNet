using System.Collections.Generic;
using EncoreTickets.SDK.Payment.Extensions;
using EncoreTickets.SDK.Payment.Models;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.UnitTests.Payment.Extensions
{
    internal class AddressExtensionTests
    {
        [TestCaseSource(typeof(AddressExtensionTestsSource), nameof(AddressExtensionTestsSource.ToNullIfEmpty_ReturnsCorrectly))]
        public void ToNullIfEmpty_ReturnsCorrectly(Address address, bool expectedNull)
        {
            var expected = expectedNull ? null : address;

            var actual = address.ToNullIfEmpty();

            Assert.AreEqual(expected, actual);
        }
    }

    internal static class AddressExtensionTestsSource
    {
        public static IEnumerable<TestCaseData> ToNullIfEmpty_ReturnsCorrectly { get; } = new[]
        {
            new TestCaseData(
                null,
                true),
            new TestCaseData(
                new Address(),
                true),
            new TestCaseData(
                new Address
                {
                    City = "minsk",
                    CountryCode = "BLR",
                    PostalCode = "Code",
                    LegacyCountryCode = null,
                    Line1 = "address",
                    Line2 = "house",
                    StateOrProvince = "minsk",
                },
                false),
            new TestCaseData(
                new Address
                {
                    CountryCode = "BLR",
                },
                false),
            new TestCaseData(
                new Address
                {
                    City = "minsk",
                    PostalCode = "Code",
                    LegacyCountryCode = null,
                    Line1 = "address",
                    Line2 = "house",
                    StateOrProvince = "minsk",
                },
                false),
        };
    }
}
