using System.Collections.Generic;
using EncoreTickets.SDK.Payment.Extensions;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.UnitTests.Payment.Extensions
{
    internal class PaymentExtensionTests
    {
        [TestCaseSource(typeof(PaymentExtensionTestsSource), nameof(PaymentExtensionTestsSource.IsSuccessfulPayment_ReturnsCorrectly))]
        public void IsSuccessfulPayment_ReturnsCorrectly(SDK.Payment.Models.Payment payment, bool expected)
        {
            var actual = payment.IsSuccessfulPayment();

            Assert.AreEqual(expected, actual);
        }
    }

    internal static class PaymentExtensionTestsSource
    {
        public static IEnumerable<TestCaseData> IsSuccessfulPayment_ReturnsCorrectly = new[]
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
