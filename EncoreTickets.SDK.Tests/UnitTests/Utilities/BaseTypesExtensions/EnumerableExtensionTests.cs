using System.Collections.Generic;
using System.Linq;
using EncoreTickets.SDK.Tests.Helpers;
using EncoreTickets.SDK.Utilities.BaseTypesExtensions;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.UnitTests.Utilities.BaseTypesExtensions
{
    public class EnumerableExtensionTests
    {
        [Test]
        public void DistinctBy_Null()
        {
            var source = (IEnumerable<dynamic>) null;

            var result = source.DistinctBy(o => o.Id);

            Assert.Null(result);
        }

        [Test]
        public void DistinctBy_Empty()
        {
            var source = Enumerable.Empty<dynamic>();

            var result = source.DistinctBy(o => o.Id);

            Assert.IsEmpty(result);
        }

        [Test]
        public void DistinctBy_Successful()
        {
            var source = new List<dynamic> { new { Id = 1, Value = 5 }, new { Id = 1, Value = 7 }, new { Id = 2, Value = 5 } };

            var result = source.DistinctBy(o => o.Id).ToList();

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(source[0], result[0]);
            Assert.AreEqual(source[2], result[1]);
        }

        [Test]
        public void Prepend_Successful()
        {
            var item = new {Id = 1, Value = "string1"};
            var originalList = new List<object>
            {
                new {Id = 2, Value = "string2"},
                null,
                new {Id = 3, Value = "string3"}
            };

            var result = EnumerableExtension.Prepend(originalList, item).ToList();

            Assert.AreEqual(originalList.Count + 1, result.Count);
            AssertExtension.AreObjectsValuesEqual(item, result[0]);
            for (int i = 0; i < originalList.Count; i++)
            {
                AssertExtension.AreObjectsValuesEqual(originalList[i], result[i + 1]);
            }
        }

        [TestCaseSource(typeof(EnumerableExtensionTestsSource), nameof(EnumerableExtensionTestsSource.ExcludeEmptyStrings_ReturnsCorrectly))]
        public void ExcludeEmptyStrings_ReturnsCorrectly(IEnumerable<string> source, IEnumerable<string> expected)
        {
            var actual = source.ExcludeEmptyStrings();

            AssertExtension.AreObjectsValuesEqual(expected, actual);
        }

        [TestCaseSource(typeof(EnumerableExtensionTestsSource), nameof(EnumerableExtensionTestsSource.NullIfEmptyEnumerable_IfSourceEnumerableIsNotNull_ReturnsCorrectly))]
        public void NullIfEmptyEnumerable_IfSourceEnumerableIsNotNull_ReturnsCorrectly<T>(IEnumerable<T> source, List<T> expected)
        {
            var actual = source.NullIfEmptyEnumerable();

            AssertExtension.AreObjectsValuesEqual(expected, actual);
        }

        [TestCaseSource(typeof(EnumerableExtensionTestsSource), nameof(EnumerableExtensionTestsSource.NullIfEmptyEnumerable_IfSourceEnumerableIsNull_ReturnsCorrectly))]
        public void NullIfEmptyEnumerable_IfSourceEnumerableIsNull_ReturnsCorrectly<T>(IEnumerable<T> defaultEnumerable)
        {
            var source = (IEnumerable<T>) null;

            var actual = source.NullIfEmptyEnumerable();

            Assert.Null(actual);
        }
    }

    public static class EnumerableExtensionTestsSource
    {
        public static IEnumerable<TestCaseData> ExcludeEmptyStrings_ReturnsCorrectly = new[]
        {
            new TestCaseData(
                new [] {"test", "test test", "", null, "   ", "test"},
                new [] { "test", "test test", "   ", "test" }
            ),
            new TestCaseData(
                new [] {null, ""},
                new string[0]
            ),
            new TestCaseData(
                null,
                null
            ),
        };

        public static IEnumerable<TestCaseData> NullIfEmptyEnumerable_IfSourceEnumerableIsNotNull_ReturnsCorrectly = new[]
        {
            new TestCaseData(
                new [] {"test", "test test", "", null, "   ", "test"},
                new List<string> {"test", "test test", "", null, "   ", "test"}
            ),
            new TestCaseData(
                new Queue<int>(new []{1, 2, 3, 4, 5}),
                new List<int> {1, 2, 3, 4, 5}
            ),
            new TestCaseData(
                new string[0],
                null
            ),
            new TestCaseData(
                new List<int>(),
                null
            ),
        };

        public static IEnumerable<TestCaseData> NullIfEmptyEnumerable_IfSourceEnumerableIsNull_ReturnsCorrectly = new[]
        {
            new TestCaseData(
                new List<string>()
            ),
            new TestCaseData(
                new int[0]
            ),
        };
    }
}
