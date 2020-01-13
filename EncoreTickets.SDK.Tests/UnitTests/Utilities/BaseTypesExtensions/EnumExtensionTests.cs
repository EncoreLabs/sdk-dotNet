using System;
using System.Collections.Generic;
using EncoreTickets.SDK.Api.Models;
using EncoreTickets.SDK.Api.Results.Response;
using EncoreTickets.SDK.Utilities.BaseTypesExtensions;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.UnitTests.Utilities.BaseTypesExtensions
{
    internal class EnumExtensionTests
    {
        [TestCaseSource(typeof(EnumExtensionTestsSource), nameof(EnumExtensionTestsSource.GetListWithAllEnumValues_ReturnsCorrectly))]
        public void GetListWithAllEnumValues_ReturnsCorrectly<T>(List<T> expected)
            where T : Enum
        {
            var actual = EnumExtension.GetListWithAllEnumValues<T>();

            Assert.AreEqual(expected, actual);
        }
    }

    public static class EnumExtensionTestsSource
    {
        public static IEnumerable<TestCaseData> GetListWithAllEnumValues_ReturnsCorrectly = new[]
        {
            new TestCaseData(
                new List<Environments>
                {
                    Environments.Production,
                    Environments.Staging,
                    Environments.QA,
                    Environments.Sandbox
                }
            ),
            new TestCaseData(
                new List<ErrorWrapping>
                {
                    ErrorWrapping.Context,
                    ErrorWrapping.Errors,
                    ErrorWrapping.MessageWithCode
                }
            ),
        };
    }
}
