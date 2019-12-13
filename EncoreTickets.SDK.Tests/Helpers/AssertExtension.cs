using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.Helpers
{
    public static class AssertExtension
    {
        private const int MaxPropertiesDeep = 5;

        private static readonly Dictionary<Type, IEnumerable<string>> ExcludedProperties =
            new Dictionary<Type, IEnumerable<string>>
            {
                {typeof(DateTime), new[] {nameof(DateTime.Now), nameof(DateTime.Kind)}},
                {typeof(List<string>), new[] {nameof(List<string>.Capacity)}},
            };

        public static void AreObjectsValuesEqual<T>(T expected, T actual)
        {
            AreObjectsValuesEqual(expected, actual, ExcludedProperties, null);
        }

        public static void AreObjectsValuesEqual<T>(T expected, T actual,
            params KeyValuePair<Type, IEnumerable<string>>[] excludedProperties)
        {
            var allExcludedProperties = ExcludedProperties.ToList();
            allExcludedProperties.AddRange(excludedProperties);
            var excludedPropertiesDictionary = allExcludedProperties.ToDictionary(x => x.Key, x => x.Value);
            AreObjectsValuesEqual(expected, actual, excludedPropertiesDictionary, null);
        }

        private static void AreObjectsValuesEqual<T>(T expected, T actual,
            Dictionary<Type, IEnumerable<string>> excludedProperties, string propertyName)
        {
            AreValuesOfBasicTypesEqual(expected, actual, excludedProperties, propertyName);
            var propertyDeep = GetPropertyDeep(propertyName);
            if (propertyDeep <= MaxPropertiesDeep)
            {
                var properties = ArePropertiesEqual(expected, actual, excludedProperties, propertyName);
            }
        }

        private static void AreValuesOfBasicTypesEqual<T>(T expected, T actual,
            Dictionary<Type, IEnumerable<string>> excludedProperties, string propertyName)
        {
            if (expected is string || expected is ValueType || expected is IEnumerable<ValueType> ||
                expected == null || actual == null || expected.GetType() != actual.GetType())
            {
                AreEqual(expected, actual, propertyName);
            }
            else
            {
                if (expected is IEnumerable<object>)
                {
                    AreEnumerableEqual(expected as IEnumerable<object>, actual as IEnumerable<object>,
                        excludedProperties, propertyName);
                }
            }
        }

        private static void AreEqual<T>(T expected, T actual, string propertyName)
        {
            if (propertyName == null)
            {
                Assert.AreEqual(expected, actual);
            }
            else
            {
                Assert.AreEqual(expected, actual, $"Property {propertyName} does not match.");
            }
        }

        private static void AreEnumerableEqual<T>(T expected, T actual,
            Dictionary<Type, IEnumerable<string>> excludedProperties, string propertyName)
            where T : IEnumerable<object>
        {
            var expectedLength = expected.Count();
            var actualLength = actual.Count();

            var collectionName = string.IsNullOrEmpty(propertyName)
                ? "Collection"
                : $"Property {propertyName}";
            var message = $"{collectionName} has a different number of items. Expected: {expectedLength} but was: {actualLength}";
            Assert.AreEqual(expectedLength, actualLength, message);

            var expectedArray = expected.ToArray();
            var actualArray = actual.ToArray();
            for (var i = 0; i < expectedLength; i++)
            {
                var elementName = GetNameOfEnumerableItem(propertyName, i);
                AreObjectsValuesEqual(expectedArray[i], actualArray[i], excludedProperties, elementName);
            }
        }

        private static IEnumerable<PropertyInfo> ArePropertiesEqual<T>(T expected, T actual,
            Dictionary<Type, IEnumerable<string>> excludedProperties, string sourcePropertyName)
        {
            var comparedProperties = new List<PropertyInfo>();
            if (expected == null && actual == null)
            {
                return comparedProperties;
            }

            var properties = GetPropertiesOfExpectedObject(expected, excludedProperties);
            foreach (var property in properties)
            {
                try
                {
                    ArePropertiesEqual(property, expected, actual, excludedProperties, sourcePropertyName);
                    comparedProperties.Add(property);
                }
                catch (TargetParameterCountException)
                {
                }
            }

            return comparedProperties;
        }

        private static void ArePropertiesEqual<T>(PropertyInfo property, T expected, T actual,
            Dictionary<Type, IEnumerable<string>> excludedProperties, string sourcePropertyName)
        {
            var expectedValue = property.GetValue(expected, null);
            var actualValue = property.GetValue(actual, null);
            var propertyName = GetPropertyName(property, sourcePropertyName);
            AreObjectsValuesEqual(expectedValue, actualValue, excludedProperties, propertyName);
        }

        private static IEnumerable<PropertyInfo> GetPropertiesOfExpectedObject<T>(T expected,
            Dictionary<Type, IEnumerable<string>> excludedProperties)
        {
            var type = expected.GetType();
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var excludedPropertiesOfType in excludedProperties)
            {
                properties = properties.Where(x =>
                    !(type == excludedPropertiesOfType.Key &&
                      excludedPropertiesOfType.Value.Contains(x.Name))).ToArray();
            }

            return properties;
        }

        private static string GetNameOfEnumerableItem(string sourcePropertyName, int index)
        {
            return $"{sourcePropertyName}[{index}]";
        }

        private static string GetPropertyName(PropertyInfo property, string sourcePropertyName)
        {
            return string.IsNullOrEmpty(sourcePropertyName)
                ? property.Name
                : $"{sourcePropertyName}.{property.Name}";
        }

        private static int GetPropertyDeep(string propertyName)
        {
            return propertyName?.Split('.', '[').Length ?? 0;
        }
    }
}
