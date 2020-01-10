using System;
using EncoreTickets.SDK.Utilities.Cache;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.UnitTests.Utilities.Cache
{
    [TestFixture]
    internal class LazyCacheDecoratorTests
    {
        private LazyCacheDecorator lazyCache;

        [SetUp]
        public void SetupCache()
        {
            lazyCache = new LazyCacheDecorator(new MemoryCache(GetRandomKey()));
        }

        [Test]
        public void Contains_IfDataWithKeyWasAdded_ReturnsTrue()
        {
            var key = GetRandomKey();
            lazyCache.Set(key, () => new object(), null);

            var result = lazyCache.Contains(key);

            Assert.IsTrue(result);
        }

        [Test]
        public void Contains_IfDataWithKeyWasNotAdded_ReturnsFalse()
        {
            var key = GetRandomKey();

            var result = lazyCache.Contains(key);

            Assert.IsFalse(result);
        }

        [Test]
        public void Contains_IfAddedItemsWereDeleted_ReturnsFalseForAllDeletedItems()
        {
            var keys = new[] { GetRandomKey(), GetRandomKey() };
            lazyCache.Set(keys[0], () => new object(), null);
            lazyCache.Set(keys[1], () => new object(), null);

            lazyCache.Remove(keys[0]);
            lazyCache.Remove(keys[1]);

            Assert.IsFalse(lazyCache.Contains(keys[0]));
            Assert.IsFalse(lazyCache.Contains(keys[1]));
        }

        [TestCase(4)]
        [TestCase("test")]
        [TestCase(1.2)]
        public void Get_IfDataWithKeyWasAdded_ReturnsData<T>(T data)
        {
            var key = GetRandomKey();
            lazyCache.Set(key, () => data, null);

            var result = lazyCache.Get<T>(key);

            Assert.AreEqual(data, result);
        }

        [Test]
        public void Get_IfDataWithKeyWasNotAdded_ThrowsException()
        {
            var key = GetRandomKey();

            Assert.Throws<CacheKeyNotFoundException>(() => lazyCache.Get<object>(key));
        }

        [TestCase(4)]
        [TestCase("test")]
        [TestCase(1.2)]
        public void AddOrGetExisting_IfDataWithKeyWasAddedAndNotNull_ReturnsData<T>(T data)
        {
            var key = GetRandomKey();
            lazyCache.Set(key, () => data, null);
            var factoryCalled = false;

            var result = lazyCache.AddOrGetExisting<T>(key, () =>
            {
                factoryCalled = true;
                return default;
            }, null);

            Assert.IsFalse(factoryCalled);
            Assert.AreEqual(data, result);
        }

        [TestCase("1730")]
        [TestCase("Success")]
        public void AddOrGetExisting_IfDataWithKeyWasNotAddedAndDefaultDataIsNull_AddAndReturnsData_And_FactoryIsCalled<T>(T instance)
        {
            var key = GetRandomKey();
            var factoryCalled = false;

            var result = lazyCache.AddOrGetExisting(key, () =>
            {
                factoryCalled = true;
                return instance;
            }, null);

            Assert.IsTrue(factoryCalled);
            Assert.AreEqual(instance, result);
            Assert.IsTrue(lazyCache.Contains(key));
        }

        [Test]
        public void Set_OverwritesExistingValue_And_FactoryIsCalled()
        {
            var key = GetRandomKey();
            var instance = new object();
            var firstFactoryCalled = false;
            var secondFactoryCalled = false;

            lazyCache.Set(key, () =>
            {
                firstFactoryCalled = true;
                return new object();
            }, null);
            lazyCache.Set(key, () =>
            {
                secondFactoryCalled = true;
                return instance;
            }, null);

            Assert.AreEqual(instance, lazyCache.Get<object>(key));
            Assert.IsFalse(firstFactoryCalled);
            Assert.IsTrue(secondFactoryCalled);
        }

        [Test]
        public void Remove_IfTryToDeleteDataByNullKeys_DoesNOtThrowExceptions()
        {
            var keys = new[] { null, "test", null };

            Assert.DoesNotThrow(() => lazyCache.Remove(keys[0]));
            Assert.DoesNotThrow(() => lazyCache.Remove(keys[1]));
            Assert.DoesNotThrow(() => lazyCache.Remove(keys[2]));
        }

        private static string GetRandomKey() => Guid.NewGuid().ToString();
    }
}
