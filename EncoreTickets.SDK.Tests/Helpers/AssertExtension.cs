using System;
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
                    .RespectingRuntimeTypes()
                    .ComparingByMembers<T>()
                    .WithStrictOrdering()
                    .WithTracing());
            }
        }
    }
}
