using System.Collections.Generic;
using System.Linq;
using EncoreTickets.SDK.Utilities.CommonModels;
using EncoreTickets.SDK.Utilities.CommonModels.Extensions;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.UnitTests.Utilities.CommonModels
{
    internal class EntityWithStatusExtensionTests
    {
        [TestCaseSource(typeof(EntityWithStatusExtensionTestsSource), nameof(EntityWithStatusExtensionTestsSource.HasStatus_ReturnsCorrectly))]
        public void HasStatus_ReturnsCorrectly(IEntityWithStatus entity, string status, bool expected)
        {
            var actual = entity.HasStatus(status);

            Assert.AreEqual(expected, actual);
        }

        [TestCaseSource(typeof(EntityWithStatusExtensionTestsSource), nameof(EntityWithStatusExtensionTestsSource.HasOneOfStatuses_ReturnsCorrectly))]
        public void HasOneOfStatuses_ReturnsCorrectly(IEntityWithStatus entity, IEnumerable<string> statuses, bool expected)
        {
            var actual = entity.HasOneOfStatuses(statuses.ToArray());

            Assert.AreEqual(expected, actual);
        }
    }

    internal static class EntityWithStatusExtensionTestsSource
    {
        public static IEnumerable<TestCaseData> HasStatus_ReturnsCorrectly { get; } = new[]
        {
            new TestCaseData(
                null,
                "status",
                false),
            new TestCaseData(
                new SDK.Payment.Models.Payment(),
                "status",
                false),
            new TestCaseData(
                new SDK.Payment.Models.Payment
                {
                    Status = "status",
                },
                "status",
                true),
            new TestCaseData(
                new SDK.Payment.Models.Payment
                {
                    Status = "status1",
                },
                "status2",
                false),
            new TestCaseData(
                new SDK.Payment.Models.Payment
                {
                    Status = "status",
                },
                "sTaTus",
                true),
        };

        public static IEnumerable<TestCaseData> HasOneOfStatuses_ReturnsCorrectly { get; } = new[]
        {
            new TestCaseData(
                null,
                new[] { "status1", "status2", "status3" },
                false),
            new TestCaseData(
                new SDK.Payment.Models.Payment(),
                new[] { "status1", "status2", "status3" },
                false),
            new TestCaseData(
                new SDK.Payment.Models.Payment
                {
                    Status = "status4",
                },
                new[] { "status1", "status2", "status3" },
                false),
            new TestCaseData(
                new SDK.Payment.Models.Payment
                {
                    Status = "status2",
                },
                new[] { "status1", "status2", "status3" },
                true),
            new TestCaseData(
                new SDK.Payment.Models.Payment
                {
                    Status = "STATUS3",
                },
                new[] { "status1", "status2", "status3" },
                true),
        };
    }
}
