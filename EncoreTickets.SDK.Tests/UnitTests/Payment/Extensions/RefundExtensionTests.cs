using System.Collections.Generic;
using EncoreTickets.SDK.Payment.Extensions;
using EncoreTickets.SDK.Payment.Models;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.UnitTests.Payment.Extensions
{
    internal class RefundExtensionTests
    {
        [TestCaseSource(typeof(RefundExtensionTestsSource), nameof(RefundExtensionTestsSource.IsSuccessfulRefund_ReturnsCorrectly))]
        public void IsSuccessfulRefund_ReturnsCorrectly(Refund refund, bool expected)
        {
            var actual = refund.IsSuccessfulRefund();

            Assert.AreEqual(expected, actual);
        }
    }

    internal static class RefundExtensionTestsSource
    {
        public static IEnumerable<TestCaseData> IsSuccessfulRefund_ReturnsCorrectly = new[]
        {
            new TestCaseData(
                null,
                false
            ),
            new TestCaseData(
                new Refund(),
                false
            ),
            new TestCaseData(
                new Refund
                {
                    Status = "receiv"
                },
                false
            ),
            new TestCaseData(
                new Refund
                {
                    Status = "pending"
                },
                true
            ),
            new TestCaseData(
                new Refund
                {
                    Status = "success"
                },
                true
            ),
            new TestCaseData(
                new Refund
                {
                    Status = "RECEIVED"
                },
                true
            ),
            new TestCaseData(
                new Refund
                {
                    Status = "received"
                },
                true
            ),
        };
    }
}
