using System;
using System.Collections.Generic;
using EncoreTickets.SDK.Api.Models;
using EncoreTickets.SDK.Api.Results.Response;
using EncoreTickets.SDK.Utilities.BaseTypesExtensions;
using EncoreTickets.SDK.Venue.Models;
using Moq;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.UnitTests.Utilities.BaseTypesExtensions
{
    internal class EnumExtensionTests
    {
        [TestCaseSource(typeof(EnumExtensionTestsSource), nameof(EnumExtensionTestsSource.GetEnumValues_ReturnsCorrectly))]
        public void GetEnumValues_ReturnsCorrectly<T>(List<T> expected)
            where T : Enum
        {
            var actual = EnumExtension.GetEnumValues<T>();

            Assert.AreEqual(expected, actual);
        }

        [TestCaseSource(typeof(EnumExtensionTestsSource), nameof(EnumExtensionTestsSource.GetEnumFromString_IfEnumValueExists_ReturnsCorrectly))]
        public void GetEnumFromString_IfEnumValueExists_ReturnsCorrectly<T>(string source, T expected)
            where T : Enum
        {
            var actual = EnumExtension.GetEnumFromString<T>(source);

            Assert.AreEqual(expected, actual);
        }

        [TestCaseSource(typeof(EnumExtensionTestsSource), nameof(EnumExtensionTestsSource.GetEnumFromString_IfEnumValueDoesNotExist_ThrowsArgumentException))]
        public void GetEnumFromString_IfEnumValueDoesNotExist_ThrowsArgumentException<T>(string source, T expected)
            where T : Enum
        {
            Assert.Catch<ArgumentException>(() => EnumExtension.GetEnumFromString<T>(source));
        }
    }

    internal static class EnumExtensionTestsSource
    {
        public static IEnumerable<TestCaseData> GetEnumValues_ReturnsCorrectly { get; } = new[]
        {
            new TestCaseData(
                new List<Environments>
                {
                    Environments.Sandbox,
                    Environments.QA,
                    Environments.Staging,
                    Environments.Production,
                }),
            new TestCaseData(
                new List<ErrorWrapping>
                {
                    ErrorWrapping.MessageWithCode,
                    ErrorWrapping.Errors,
                    ErrorWrapping.Context,
                    ErrorWrapping.NotParsedContent,
                }),
        };

        public static IEnumerable<TestCaseData> GetEnumFromString_IfEnumValueExists_ReturnsCorrectly { get; } = new[]
        {
            new TestCaseData(
                "1234",
                (Environments)1234),
            new TestCaseData(
                "Sandbox",
                Environments.Sandbox),
            new TestCaseData(
                "sandbox",
                Environments.Sandbox),
            new TestCaseData(
                "Production",
                Environments.Production),
            new TestCaseData(
                "Staging",
                Environments.Staging),
            new TestCaseData(
                "QA",
                Environments.QA),
            new TestCaseData(
                "qa",
                Environments.QA),
            new TestCaseData(
                "Negative",
                Intention.Negative),
        };

        public static IEnumerable<TestCaseData> GetEnumFromString_IfEnumValueDoesNotExist_ThrowsArgumentException { get; } = new[]
        {
            new TestCaseData(
                "dev",
                It.IsAny<Environments>()),
            new TestCaseData(
                "prod",
                It.IsAny<Environments>()),
            new TestCaseData(
                "12.34",
                It.IsAny<Environments>()),
        };
    }
}
