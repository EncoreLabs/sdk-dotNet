using System;
using EncoreTickets.SDK.Utilities.Common.Cache;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.UnitTests.Utilities
{
    [TestFixture]
    internal class UtilitiesCacheHandlerTests
    {
        private LazyCacheHandler _lazyCacheHandler;

        [SetUp]
        public void SetupCacheHandler()
        {
            _lazyCacheHandler = new LazyCacheHandler(new DefaultMemoryCache(GetRandomKey()));
        }

        [Test]
        public void Utilities_CacheHandler_IfDataWithKeyWasAdded_ExistMethodReturnsTrue()
        {
            var key = GetRandomKey();
            _lazyCacheHandler.Set(key, () => new object(), null);

            var result = _lazyCacheHandler.Contains(key);

            Assert.IsTrue(result);
        }

        [Test]
        public void Utilities_CacheHandler_IfDataWithKeyWasNotAdded_ExistMethodReturnsFalse()
        {
            var key = GetRandomKey();

            var result = _lazyCacheHandler.Contains(key);

            Assert.IsFalse(result);
        }

        [TestCase(4)]
        [TestCase("test")]
        [TestCase(1.2)]
        public void Utilities_CacheHandler_IfDataWithKeyWasAdded_GetMethodReturnsData<T>(T data)
        {
            var key = GetRandomKey();
            _lazyCacheHandler.Set(key, () => data, null);

            var result = _lazyCacheHandler.Get<T>(key);

            Assert.AreEqual(data, result);
        }

        [Test]
        public void Utilities_CacheHandler_IfDataWithKeyWasNotAdded_GetMethodReturnsDefault()
        {
            var key = GetRandomKey();

            var result = _lazyCacheHandler.Get<object>(key);

            Assert.AreEqual(default, result);
        }

        [TestCase(4)]
        [TestCase("test")]
        [TestCase(1.2)]
        public void Utilities_CacheHandler_IfDataWithKeyWasAddedAndNotNull_GetWithCacheMethodReturnsData<T>(T data)
        {
            var key = GetRandomKey();
            _lazyCacheHandler.Set(key, () => data, null);

            var result = _lazyCacheHandler.AddOrGetExisting<T>(key, null, null);

            Assert.AreEqual(data, result);
        }

        [TestCase("1730")]
        [TestCase("Success")]
        public void Utilities_CacheHandler_IfDataWithKeyWasNotAddedAndDefaultDataIsNull_GetWithCacheMethodAddAndReturnsData<T>(T instance)
        {
            var key = GetRandomKey();

            var result = _lazyCacheHandler.AddOrGetExisting(key, () => instance, null);

            Assert.AreEqual(instance, result);
            Assert.IsTrue(_lazyCacheHandler.Contains(key));
        }

        [Test]
        public void Utilities_CacheHandler_SetMethodOverwritesExistingValue()
        {
            var key = GetRandomKey();
            var instance = new object();

            _lazyCacheHandler.Set(key, () => new object(), null);
            _lazyCacheHandler.Set(key, () => instance, null);

            Assert.AreEqual(instance, _lazyCacheHandler.Get<object>(key));
        }

        [Test]
        public void Utilities_CacheHandler_IfAddedItemsWereDeleted_ExistMethodReturnsFalseForAllDeletedItems()
        {
            var keys = new[] {GetRandomKey(), GetRandomKey()};
            _lazyCacheHandler.Set(keys[0], () => new object(), null);
            _lazyCacheHandler.Set(keys[1], () => new object(), null);

            _lazyCacheHandler.Remove(keys[0]);
            _lazyCacheHandler.Remove(keys[1]);

            Assert.IsFalse(_lazyCacheHandler.Contains(keys[0]));
            Assert.IsFalse(_lazyCacheHandler.Contains(keys[1]));
        }

        [Test]
        public void Utilities_CacheHandler_IfTryToDeleteDataByNullKeys_DeleteMethodDoesNOtThrowExceptions()
        {
            var keys = new[] { null, "test", null };

            Assert.DoesNotThrow(() => _lazyCacheHandler.Remove(keys[0]));
            Assert.DoesNotThrow(() => _lazyCacheHandler.Remove(keys[1]));
            Assert.DoesNotThrow(() => _lazyCacheHandler.Remove(keys[2]));
        }

        private static string GetRandomKey() => Guid.NewGuid().ToString();
    }
}
