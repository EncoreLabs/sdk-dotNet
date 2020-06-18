using System.Collections.Generic;
using EncoreTickets.SDK.Tests.Helpers;
using EncoreTickets.SDK.Venue.Extensions;
using EncoreTickets.SDK.Venue.Models;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.UnitTests.Venue.Extensions
{
    internal class AttributeExtensionTests
    {
        [TestCaseSource(typeof(AttributeExtensionTestsSource), nameof(AttributeExtensionTestsSource.IsValid_ReturnsCorrectly))]
        public void IsValid_ReturnsCorrectly(Attribute attribute, bool expected)
        {
            var actual = attribute.IsValid();

            Assert.AreEqual(expected, actual);
        }

        [TestCaseSource(typeof(AttributeExtensionTestsSource), nameof(AttributeExtensionTestsSource.CreateExtraAttribute_IfWithIntentionAsStr_ReturnsCorrectly))]
        public void CreateExtraAttribute_IfWithIntentionAsStr_ReturnsCorrectly(string description, string intentionAsStr, Attribute expected)
        {
            var actual = AttributeExtension.CreateExtraAttribute(description, intentionAsStr);

            AssertExtension.AreObjectsValuesEqual(expected, actual);
        }

        [TestCaseSource(typeof(AttributeExtensionTestsSource), nameof(AttributeExtensionTestsSource.CreateExtraAttribute_IfWithIntention_ReturnsCorrectly))]
        public void CreateExtraAttribute_IfWithIntention_ReturnsCorrectly(string description, Intention intention, Attribute expected)
        {
            var actual = AttributeExtension.CreateExtraAttribute(description, intention);

            AssertExtension.AreObjectsValuesEqual(expected, actual);
        }
    }

    internal static class AttributeExtensionTestsSource
    {
        public static IEnumerable<TestCaseData> IsValid_ReturnsCorrectly { get; } = new[]
        {
            new TestCaseData(
                null,
                false),
            new TestCaseData(
                new Attribute
                {
                    Title = "title",
                    Description = "description",
                    Intention = Intention.Negative,
                },
                true),
            new TestCaseData(
                new Attribute
                {
                    Title = "   ",
                    Description = "description",
                    Intention = Intention.Positive,
                },
                true),
            new TestCaseData(
                new Attribute
                {
                    Title = "",
                    Description = "description",
                    Intention = Intention.Positive,
                },
                false),
            new TestCaseData(
                new Attribute
                {
                    Description = "description",
                    Intention = Intention.Positive,
                },
                false),
            new TestCaseData(
                new Attribute
                {
                    Title = "title",
                    Description = "",
                    Intention = Intention.Positive,
                },
                false),
            new TestCaseData(
                new Attribute
                {
                    Title = "title",
                    Description = null,
                    Intention = Intention.Positive,
                },
                false),
            new TestCaseData(
                new Attribute
                {
                    Title = "title",
                    Description = "description",
                    Intention = (Intention)3,
                },
                false),
        };

        public static IEnumerable<TestCaseData> CreateExtraAttribute_IfWithIntentionAsStr_ReturnsCorrectly { get; } = new[]
        {
            new TestCaseData(
                "description",
                "Negative",
                new Attribute
                {
                    Title = "Other",
                    Description = "description",
                    Intention = Intention.Negative,
                }),
            new TestCaseData(
                "description",
                "Positive",
                new Attribute
                {
                    Title = "Other",
                    Description = "description",
                    Intention = Intention.Positive,
                }),
            new TestCaseData(
                "description",
                "negative",
                new Attribute
                {
                    Title = "Other",
                    Description = "description",
                    Intention = Intention.Negative,
                }),
            new TestCaseData(
                "description",
                "invalid_value",
                null),
        };

        public static IEnumerable<TestCaseData> CreateExtraAttribute_IfWithIntention_ReturnsCorrectly { get; } = new[]
        {
            new TestCaseData(
                "description",
                Intention.Negative,
                new Attribute
                {
                    Title = "Other",
                    Description = "description",
                    Intention = Intention.Negative,
                }),
            new TestCaseData(
                "description",
                Intention.Positive,
                new Attribute
                {
                    Title = "Other",
                    Description = "description",
                    Intention = Intention.Positive,
                }),
        };
    }
}
