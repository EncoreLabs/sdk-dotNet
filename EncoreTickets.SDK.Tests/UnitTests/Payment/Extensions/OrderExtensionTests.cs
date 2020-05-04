using System.Collections.Generic;
using EncoreTickets.SDK.Payment.Extensions;
using EncoreTickets.SDK.Payment.Models;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.UnitTests.Payment.Extensions
{
    internal class OrderExtensionTests
    {
        [TestCaseSource(typeof(OrderExtensionTestsSource), nameof(OrderExtensionTestsSource.HasSuccessfulPayment_ReturnsCorrectly))]
        public void HasSuccessfulPayment_ReturnsCorrectly(SDK.Payment.Models.Payment payment, bool expected)
        {
            var order = new Order
            {
                Payments = new List<SDK.Payment.Models.Payment> {payment}
            };

            var actual = order.HasSuccessfulPayment();

            Assert.AreEqual(expected, actual);
        }
    }

    internal static class OrderExtensionTestsSource
    {
        public static IEnumerable<TestCaseData> HasSuccessfulPayment_ReturnsCorrectly = new[]
        {
            new TestCaseData(
                null,
                false
            ),
            new TestCaseData(
                new SDK.Payment.Models.Payment(),
                false
            ),
            new TestCaseData(
                new SDK.Payment.Models.Payment
                {
                    Status = "new"
                },
                false
            ),
            new TestCaseData(
                new SDK.Payment.Models.Payment
                {
                    Status = "authorised"
                },
                true
            ),
            new TestCaseData(
                new SDK.Payment.Models.Payment
                {
                    Status = "captured"
                },
                true
            ),
            new TestCaseData(
                new SDK.Payment.Models.Payment
                {
                    Status = "partially_refunded"
                },
                true
            ),
            new TestCaseData(
                new SDK.Payment.Models.Payment
                {
                    Status = "refunded"
                },
                true
            ),
            new TestCaseData(
                new SDK.Payment.Models.Payment
                {
                    Status = "partially_compensated"
                },
                true
            ),
            new TestCaseData(
                new SDK.Payment.Models.Payment
                {
                    Status = "compensated"
                },
                true
            ),
            new TestCaseData(
                new SDK.Payment.Models.Payment
                {
                    Status = "REFUNDEd"
                },
                true
            ),
        };
    }
}
