using System;
using System.Collections.Generic;
using EncoreTickets.SDK.Payment.Extensions;
using EncoreTickets.SDK.Payment.Models;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.UnitTests.Payment.Extensions
{
    internal class PaymentMethodExtensionTests
    {
        [TestCaseSource(typeof(PaymentMethodExtensionTestsSource), nameof(PaymentMethodExtensionTestsSource.GetMonthFromPaymentCardExpiredDate_ReturnsCorrectly))]
        public void GetMonthFromPaymentCardExpiredDate_ReturnsCorrectly(PaymentMethod paymentMethod, int expected)
        {
            var actual = paymentMethod.GetMonthFromPaymentCardExpiredDate();

            Assert.AreEqual(expected, actual);
        }

        [TestCaseSource(typeof(PaymentMethodExtensionTestsSource), nameof(PaymentMethodExtensionTestsSource.GetYearFromPaymentCardExpiredDate_ReturnsCorrectly))]
        public void GetYearFromPaymentCardExpiredDate_ReturnsCorrectly(PaymentMethod paymentMethod, int expected)
        {
            var actual = paymentMethod.GetYearFromPaymentCardExpiredDate();

            Assert.AreEqual(expected, actual);
        }
    }

    internal static class PaymentMethodExtensionTestsSource
    {
        public static IEnumerable<TestCaseData> GetMonthFromPaymentCardExpiredDate_ReturnsCorrectly { get; } = new[]
        {
            new TestCaseData(
                null,
                0),
            new TestCaseData(
                new PaymentMethod(),
                0),
            new TestCaseData(
                new PaymentMethod
                {
                    ExpiryDate = new DateTime(2020, 12, 31, 23, 59, 59),
                },
                12),
            new TestCaseData(
                new PaymentMethod
                {
                    ExpiryDate = DateTime.MinValue,
                },
                1),
        };

        public static IEnumerable<TestCaseData> GetYearFromPaymentCardExpiredDate_ReturnsCorrectly { get; } = new[]
        {
            new TestCaseData(
                null,
                0),
            new TestCaseData(
                new PaymentMethod(),
                0),
            new TestCaseData(
                new PaymentMethod
                {
                    ExpiryDate = new DateTime(2020, 12, 31, 23, 59, 59),
                },
                2020),
            new TestCaseData(
                new PaymentMethod
                {
                    ExpiryDate = DateTime.MinValue,
                },
                1),
        };
    }
}
