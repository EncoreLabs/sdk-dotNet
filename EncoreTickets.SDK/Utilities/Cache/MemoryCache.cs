using System;

namespace EncoreTickets.SDK.Utilities.Cache
{
    public class MemoryCache : ICache
    {
        private static readonly TimeSpan DefaultCacheLifeSpan = TimeSpan.FromHours(4);

        private readonly System.Runtime.Caching.MemoryCache cache;

        /// <summary>
        /// Creates an instance of <see cref="MemoryCache"/> class with the default instance of <see cref="System.Runtime.Caching.MemoryCache"/> />
        /// </summary>
        public MemoryCache()
        {
            cache = System.Runtime.Caching.MemoryCache.Default;
        }

        /// <summary>
        /// Creates an instance of <see cref="MemoryCache"/> class with the named instance of <see cref="System.Runtime.Caching.MemoryCache"/> />
        /// </summary>
        public MemoryCache(string name)
        {
            cache = new System.Runtime.Caching.MemoryCache(name);
        }

        /// <inheritdoc />
        public T AddOrGetExisting<T>(string key, Func<T> factory, TimeSpan? lifeSpan)
        {
            var objectToAdd = factory();
            var cacheItemExpiryDate = GetCacheItemExpiryDate(lifeSpan);
            var result = cache.AddOrGetExisting(key, objectToAdd, cacheItemExpiryDate) ?? objectToAdd;
            return (T) result;
        }

        /// <inheritdoc />
        public void Set<T>(string key, Func<T> factory, TimeSpan? lifeSpan)
        {
            var objectToAdd = factory();
            var cacheItemExpiryDate = GetCacheItemExpiryDate(lifeSpan);
            cache.Set(key, objectToAdd, cacheItemExpiryDate);
        }

        /// <inheritdoc />
        public T Get<T>(string key)
        {
            return cache.Get(key) is T entry ? entry : throw new CacheKeyNotFoundException();
        }

        /// <inheritdoc />
        public bool Remove(string key)
        {
            return key != null && cache.Remove(key) != null;
        }

        /// <inheritdoc />
        public bool Contains(string key)
        {
            return cache.Contains(key);
        }

        private static DateTimeOffset GetCacheItemExpiryDate(TimeSpan? lifeSpan)
        {
            var resolvedLifeSpan = lifeSpan ?? DefaultCacheLifeSpan;
            return DateTimeOffset.Now + resolvedLifeSpan;
        }
    }
}
