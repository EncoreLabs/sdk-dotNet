using System.Collections;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests
{
    internal static class AssertExtension
    {
        public static void PropertyValuesAreEquals<T>(T expected, T actual)
        {
            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                var expectedValue = property.GetValue(expected, null);
                var actualValue = property.GetValue(actual, null);

                if (!(actualValue is IEnumerable && !(actualValue is string)) && !Equals(expectedValue, actualValue))
                {
                    Assert.Fail("Property {0}.{1} does not match. Expected: {2} but was: {3}",
                        property.DeclaringType.Name, property.Name, expectedValue, actualValue);
                }
            }
        }
    }
}
