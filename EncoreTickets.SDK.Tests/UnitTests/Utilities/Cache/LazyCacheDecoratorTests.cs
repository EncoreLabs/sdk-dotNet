using System;
using System.Collections.Generic;
using System.Threading;
using EncoreTickets.SDK.Tests.Helpers;
using EncoreTickets.SDK.Utilities.Cache;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.UnitTests.Utilities.Cache
{
    [TestFixture]
    internal class LazyCacheDecoratorTests
    {
        private LazyCacheDecorator cache;

        [SetUp]
        public void SetupCache()
        {
            var memoryCache = new MemoryCache();
            cache = new LazyCacheDecorator(memoryCache);
        }

        #region AddOrGetExisting

        [Test]
        public void AddOrGetExisting_IfKeyIsNull_ThrowsArgumentNullException()
        {
            string key = null;

            Assert.Throws<ArgumentNullException>(() =>
            {
                cache.AddOrGetExisting(key, () => new object(), null);
            });
        }

        [Test]
        public void AddOrGetExisting_IfNullDataWithKeyWasNotAddedAndLifeSpanIsNull_AddsDataForDefaultTimeAndReturnsData()
        {
            var key = GetRandomKey();
            var factoryCalled = false;

            var actual = cache.AddOrGetExisting<object>(
                key,
                () =>
                {
                    factoryCalled = true;
                    return default;
                },
                null);

            Assert.IsTrue(factoryCalled);
            Assert.True(cache.Contains(key));
            Assert.Null(actual);
        }

        [TestCaseSource(typeof(MemoryCacheTestsSource), nameof(MemoryCacheTestsSource.TestCasesWithNotNullData))]
        public void AddOrGetExisting_IfNotNullDataWithKeyWasNotAddedAndLifeSpanIsNull_AddsDataForDefaultTimeAndReturnsData<T>(T data)
        {
            var key = GetRandomKey();
            var factoryCalled = false;

            var actual = cache.AddOrGetExisting(
                key,
                () =>
                {
                    factoryCalled = true;
                    return data;
                },
                null);

            Assert.IsTrue(factoryCalled);
            Assert.True(cache.Contains(key));
            AssertExtension.AreObjectsValuesEqual(data, actual);
        }

        [TestCaseSource(typeof(MemoryCacheTestsSource), nameof(MemoryCacheTestsSource.TestCasesWithNotNullData))]
        public void AddOrGetExisting_IfDataWithKeyWasNotAddedAndLifeSpanIsSet_AddsDataForCertainTimeAndReturnsData<T>(T data)
        {
            var key = GetRandomKey();
            var factoryCalled = false;
            var lifeTime = new TimeSpan(0, 0, 0, 0, 100);

            var actual = cache.AddOrGetExisting(
                key,
                () =>
                {
                    factoryCalled = true;
                    return data;
                },
                lifeTime);

            Assert.IsTrue(factoryCalled);
            Assert.True(cache.Contains(key));
            AssertExtension.AreObjectsValuesEqual(data, actual);
            Thread.Sleep(lifeTime);
            Assert.False(cache.Contains(key));
        }

        [Test]
        public void AddOrGetExisting_IfDataWithKeyAndWithSameTypeWasAdded_ReturnsAndDoesNotOverwriteExistingValue()
        {
            var key = GetRandomKey();
            var factoryCalled = false;
            var instance1 = new object();
            cache.Set(key, () => instance1, null);
            var instance2 = new object();

            var actual = cache.AddOrGetExisting(
                key,
                () =>
                {
                    factoryCalled = true;
                    return instance2;
                },
                null);

            Assert.IsFalse(factoryCalled);
            Assert.AreEqual(instance1, actual);
        }

        [Test]
        public void AddOrGetExisting_IfDataWithKeyAndWithOtherTypeWasAdded_ThrowsInvalidCastException()
        {
            var key = GetRandomKey();
            var factoryCalled = false;
            const string strInstance = "string";
            cache.Set(key, () => strInstance, null);
            const int intInstance = 100;

            Assert.Throws<InvalidCastException>(() =>
            {
                var result = cache.AddOrGetExisting(
                    key,
                    () =>
                    {
                        factoryCalled = true;
                        return intInstance;
                    },
                    null);
            });
            Assert.IsFalse(factoryCalled);
        }

        #endregion

        #region Set

        [Test]
        public void Set_IfKeyIsNull_ThrowsArgumentNullException()
        {
            string key = null;

            Assert.Throws<ArgumentNullException>(() =>
            {
                cache.Set(key, () => new object(), null);
            });
        }

        [Test]
        public void Set_IfKeyIsNotNullAndDataIsNull_ThrowsArgumentNullException()
        {
            var key = GetRandomKey();

            cache.Set<object>(key, () => null, null);

            Assert.True(cache.Contains(key));
            Assert.Null(cache.Get<object>(key));
        }

        [TestCaseSource(typeof(MemoryCacheTestsSource), nameof(MemoryCacheTestsSource.TestCasesWithNotNullData))]
        public void Set_IfKeyIsNotNullAndLifeSpanIsNull_AddsDataToCacheForDefaultTime<T>(T data)
        {
            var key = GetRandomKey();

            cache.Set(key, () => data, null);

            Assert.True(cache.Contains(key));
        }

        [TestCaseSource(typeof(MemoryCacheTestsSource), nameof(MemoryCacheTestsSource.TestCasesWithNotNullData))]
        public void Set_IfKeyIsNotNullAndLifeSpanIsSet_AddsDataToCacheForCertainTime<T>(T data)
        {
            var key = GetRandomKey();
            var lifeTime = new TimeSpan(0, 0, 0, 0, 100);

            cache.Set(key, () => data, lifeTime);

            Assert.True(cache.Contains(key));
            Thread.Sleep(lifeTime);
            Assert.False(cache.Contains(key));
        }

        [Test]
        public void Set_IfDataWithKeyAndWithSameTypeExists_OverwritesExistingValue()
        {
            var key = GetRandomKey();
            cache.Set(key, () => new object(), null);
            var instance = new object();

            cache.Set(key, () => instance, null);

            Assert.AreEqual(instance, cache.Get<object>(key));
        }

        [Test]
        public void Set_IfDataWithKeyAndWithOtherTypeExists_OverwritesExistingValue()
        {
            var key = GetRandomKey();
            const string strInstance = "string";
            cache.Set(key, () => strInstance, null);
            const int intInstance = 100;

            cache.Set(key, () => intInstance, null);

            Assert.AreEqual(intInstance, cache.Get<int>(key));
            Assert.Throws<CacheKeyNotFoundException>(() =>
            {
                var actual = cache.Get<string>(key);
            });
        }

        #endregion

        #region Get

        [Test]
        public void Get_IfKeyIsNull_ThrowsArgumentNullException()
        {
            const string key = null;

            Assert.Throws<ArgumentNullException>(() =>
            {
                var actual = cache.Get<object>(key);
            });
        }

        [TestCaseSource(typeof(LazyCacheDecoratorTestsSource), nameof(LazyCacheDecoratorTestsSource.TestCasesWithNotNullData))]
        public void Get_IfDataWithKeyWasAddedAndTryToGetDataWithSameType_ReturnsData<T>(T data)
        {
            var key = GetRandomKey();
            cache.Set(key, () => data, null);

            var result = cache.Get<T>(key);

            AssertExtension.AreObjectsValuesEqual(data, result);
        }

        [Test]
        public void Get_IfDataWithKeyWasAddedAndTryToGetDataWithOtherType_ThrowsCacheKeyNotFoundException()
        {
            var key = GetRandomKey();
            cache.Set(key, () => "string", null);

            Assert.Throws<CacheKeyNotFoundException>(() =>
            {
                var actual = cache.Get<bool>(key);
            });
        }

        [Test]
        public void Get_IfDataWithKeyWasNotAdded_ThrowsCacheKeyNotFoundException()
        {
            var key = GetRandomKey();

            Assert.Throws<CacheKeyNotFoundException>(() =>
            {
                var actual = cache.Get<object>(key);
            });
        }

        #endregion

        #region Remove

        [Test]
        public void Remove_IfKeyIsNull_ReturnsFalseAndDoesNotThrowExceptions()
        {
            var actual = cache.Remove(null);

            Assert.False(actual);
        }

        [Test]
        public void Remove_IfKeyIsNotNullAndObjectByKeyDoesNotExist_ReturnsFalseAndDoesNotThrowExceptions()
        {
            var key = GetRandomKey();

            var actual = cache.Remove(key);

            Assert.False(actual);
        }

        [Test]
        public void Remove_IfKeyIsNotNullAndObjectByKeyExists_ReturnsTrue()
        {
            var key = GetRandomKey();
            cache.Set(key, () => new object(), null);

            var actual = cache.Remove(key);

            Assert.True(actual);
        }

        #endregion

        #region Contains

        [Test]
        public void Contains_IfKeyIsNull_ThrowsArgumentNullException()
        {
            const string key = null;

            Assert.Throws<ArgumentNullException>(() =>
            {
                var actual = cache.Contains(key);
            });
        }

        [Test]
        public void Contains_IfDataWithKeyWasAdded_ReturnsTrue()
        {
            var key = GetRandomKey();
            cache.Set(key, () => new object(), null);

            var result = cache.Contains(key);

            Assert.IsTrue(result);
        }

        [Test]
        public void Contains_IfDataWithKeyWasNotAdded_ReturnsFalse()
        {
            var key = GetRandomKey();

            var result = cache.Contains(key);

            Assert.IsFalse(result);
        }

        [Test]
        public void Contains_IfAddedItemsWereDeleted_ReturnsFalseForAllDeletedItems()
        {
            var key = GetRandomKey();
            cache.Set(key, () => new object(), null);
            cache.Remove(key);

            var result = cache.Contains(key);

            Assert.IsFalse(result);
        }

        #endregion

        private static string GetRandomKey() => Guid.NewGuid().ToString();
    }

    internal static class LazyCacheDecoratorTestsSource
    {
        public static IEnumerable<TestCaseData> TestCasesWithNotNullData { get; } = new[]
        {
            new TestCaseData("1730"),
            new TestCaseData(4),
            new TestCaseData(1.2),
            new TestCaseData("Success"),
            new TestCaseData(new List<string> { "a", "b", "c" }),
        };
    }
}
