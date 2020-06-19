using System.Collections.Generic;
using EncoreTickets.SDK.Venue.Extensions;
using EncoreTickets.SDK.Venue.Models;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.UnitTests.Venue.Extensions
{
    internal class SeatDetailedExtensionTests
    {
        [TestCaseSource(typeof(SeatDetailedExtensionTestsSource), nameof(SeatDetailedExtensionTestsSource.IsValid_ReturnsCorrectly))]
        public void IsValid_ReturnsCorrectly(SeatDetailed seat, bool expected)
        {
            var actual = seat.IsValid();

            Assert.AreEqual(expected, actual);
        }
    }

    internal static class SeatDetailedExtensionTestsSource
    {
        public static IEnumerable<TestCaseData> IsValid_ReturnsCorrectly { get; } = new[]
        {
            new TestCaseData(
                new SeatDetailed
                {
                    SeatIdentifier = "circle-8",
                    Attributes = new List<Attribute> { new Attribute() },
                },
                true),
            new TestCaseData(
                null,
                false),
            new TestCaseData(
                new SeatDetailed(),
                false),
            new TestCaseData(
                new SeatDetailed
                {
                    SeatIdentifier = null,
                    Attributes = new List<Attribute> { new Attribute() },
                },
                false),
            new TestCaseData(
                new SeatDetailed
                {
                    SeatIdentifier = "",
                    Attributes = new List<Attribute> { new Attribute() },
                },
                false),
            new TestCaseData(
                new SeatDetailed
                {
                    SeatIdentifier = "circle-8",
                    Attributes = null,
                },
                false),
            new TestCaseData(
                new SeatDetailed
                {
                    SeatIdentifier = "circle-8",
                    Attributes = new List<Attribute>(),
                },
                false),
        };
    }
}
