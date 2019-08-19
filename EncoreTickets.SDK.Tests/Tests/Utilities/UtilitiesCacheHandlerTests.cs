using System;
using EncoreTickets.SDK.Utilities;
using Moq;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.Tests.Utilities
{
    internal class UtilitiesCacheHandlerTests
    {
        [Test]
        public void Utilities_CacheHandler_IfDataWithKeyWasAdded_ExistMethodReturnsTrue()
        {
            var key = GetRandomKey();
            CacheHandler.Add(new object(), key, null);
            var result = CacheHandler.Exists(key);
            Assert.IsTrue(result);
        }

        [Test]
        public void Utilities_CacheHandler_IfDataWithKeyWasNotAdded_ExistMethodReturnsFalse()
        {
            var key = GetRandomKey();
            var result = CacheHandler.Exists(key);
            Assert.IsFalse(result);
        }

        [TestCase(4)]
        [TestCase("test")]
        [TestCase(1.2)]
        public void Utilities_CacheHandler_IfDataWithKeyWasAdded_GetMethodReturnsData<T>(T data)
        {
            var key = GetRandomKey();
            CacheHandler.Add(data, key, null);
            var result = CacheHandler.Get<T>(key);
            Assert.AreEqual(data, result);
        }

        [TestCase(4)]
        [TestCase("test")]
        [TestCase(1.2)]
        public void Utilities_CacheHandler_IfDataWithKeyWasNotAdded_GetMethodReturnsDefault<T>(T instance)
        {
            var key = GetRandomKey();
            var result = CacheHandler.Get<T>(key);
            Assert.AreEqual(default(T), result);
        }

        [TestCase(4)]
        [TestCase("test")]
        [TestCase(1.2)]
        public void Utilities_CacheHandler_IfDataWithKeyWasAddedAndNotNull_GetWithCacheMethodReturnsData<T>(T data)
        {
            var key = GetRandomKey();
            CacheHandler.Add(data, key, null);
            var result = CacheHandler.Get<T>(null, key);
            Assert.AreEqual(data, result);
        }

        [TestCase("1730")]
        [TestCase("Success")]
        public void Utilities_CacheHandler_IfDataWithKeyWasNotAddedAndDefaultDataIsNull_GetWithCacheMethodAddAndReturnsData<T>(T instance)
        {
            var key = GetRandomKey();
            T CacheMethod() => instance;
            var result = CacheHandler.Get(CacheMethod, key);
            Assert.AreEqual(instance, result);
            Assert.IsTrue(CacheHandler.Exists(key));
        }

        [Test]
        public void Utilities_CacheHandler_IfAddedItemsWereDeleted_ExistMethodReturnsFalseForAllDeletedItems()
        {
            var keys = new[] {GetRandomKey(), GetRandomKey()};
            CacheHandler.Add(new object(), keys[0], null);
            CacheHandler.Add(new object(), keys[1], null);
            CacheHandler.Delete(It.IsAny<bool>(), keys);
            Assert.IsFalse(CacheHandler.Exists(keys[0]));
            Assert.IsFalse(CacheHandler.Exists(keys[1]));
        }

        [Test]
        public void Utilities_CacheHandler_IfTryToDeleteDataByNullKeys_DeleteMethodDoesNOtThrowExceptions()
        {
            var keys = new[] { null, "test", null };
            Assert.DoesNotThrow(() => CacheHandler.Delete(It.IsAny<bool>(), keys));
        }

        private static string GetRandomKey() => Guid.NewGuid().ToString();
    }
}
