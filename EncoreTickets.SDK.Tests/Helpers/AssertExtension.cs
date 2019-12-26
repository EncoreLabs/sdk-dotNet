using System;
using System.Linq;
using System.Reflection;
using FluentAssertions;
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
            }
            else
            {
                actual.Should().BeEquivalentTo(expected, options => options
                    .ComparingByMembers<T>()
                    .WithStrictOrdering()
                    .WithTracing());
            }
        }

        public static void ShouldBeEquivalentToObjectWithMoreProperties<TActual, TExpected>(this TActual actual,
            TExpected expected)
        {
            var properties = typeof(TActual)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance).ToList();
            actual.Should().BeEquivalentTo(expected, options => options
                .Including(ctx => properties.Select(property => property.Name)
                    .Contains(ctx.SelectedMemberPath)));
        }
    }
}
