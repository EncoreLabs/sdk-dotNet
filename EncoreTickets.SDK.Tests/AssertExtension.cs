using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests
{
    internal static class AssertExtension
    {
        public static void SimplePropertyValuesAreEquals<T>(T expected, T actual)
        {
            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                var expectedValue = property.GetValue(expected, null);
                var actualValue = property.GetValue(actual, null);
                var isTypeForEqual = IsValueTypeForEqualMethod(actualValue);
                if (isTypeForEqual && !Equals(expectedValue, actualValue))
                {
                    Assert.Fail("Property {0}.{1} does not match. Expected: {2} but was: {3}",
                        property.DeclaringType.Name, property.Name, expectedValue, actualValue);
                }
            }
        }

        public static void EnumerableAreEquals<T>(IEnumerable<T> expected, ICollection actual)
        {
            Assert.AreEqual(expected.Count(), actual.Count);
            foreach (var expectedItem in expected)
            {
                Assert.Contains(expectedItem, actual);
            }
        }

        private static bool IsValueTypeForEqualMethod(object actualValue)
        {
            var isString = actualValue is string;
            var isEnumerable = actualValue is IEnumerable;
            return !(isEnumerable && !isString);
        }
    }
}
