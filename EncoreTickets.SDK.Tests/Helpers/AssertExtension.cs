using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using FluentAssertions.Equivalency;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.Helpers
{
    public static class AssertExtension
    {
        public static void AreObjectsValuesEqual<T>(T expected, T actual)
        {
            if (expected is ValueType)
            {
                Assert.AreEqual(expected, actual);
                return;
            }

            try
            {
                CompareByMembers(expected, actual);
            }
            catch (InvalidOperationException)
            {
                Assert.IsInstanceOf(expected.GetType(), actual);
            }
        }

        public static void ShouldBeEquivalentToObjectWithMoreProperties<TActual, TExpected>(
            this TActual actual,
            TExpected expected)
        {
            var properties = typeof(TActual)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance).ToList();
            var propertiesNames = properties.Select(property => property.Name);
            actual.Should().BeEquivalentTo(expected, options => options
                .Including(ctx => IsValidProperty(ctx, propertiesNames)));
        }

        private static void CompareByMembers<T>(T expected, T actual)
        {
            actual.Should().BeEquivalentTo(expected, options => options
                .RespectingRuntimeTypes()
                .ComparingByMembers<T>()
                .WithStrictOrdering()
                .WithTracing());
        }

        private static bool IsValidProperty(IMemberInfo memberInfo, IEnumerable<string> propertiesNames)
        {
            return propertiesNames?.Contains(memberInfo.SelectedMemberPath) ?? false;
        }
    }
}
