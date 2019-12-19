using System;
using System.Runtime.Caching;

namespace EncoreTickets.SDK.Utilities.Common.Cache
{
    public class DefaultMemoryCache : ICache
    {
        private static readonly TimeSpan DefaultCacheLifeSpan = TimeSpan.FromHours(4);

        private MemoryCache cache;

        /// <summary>
        /// Creates an instance of <see cref="DefaultMemoryCache"/> class with the default instance of <see cref="MemoryCache"/> />
        /// </summary>
        public DefaultMemoryCache()
        {
            cache = MemoryCache.Default;
        }

        /// <summary>
        /// Creates an instance of <see cref="DefaultMemoryCache"/> class with the named instance of <see cref="MemoryCache"/> />
        /// </summary>
        public DefaultMemoryCache(string name)
        {
            cache = new MemoryCache(name);
        }

        /// <inheritdoc />
        public T AddOrGetExisting<T>(string key, Func<T> factory, TimeSpan? lifeSpan)
        {
            var resolvedLifeSpan = lifeSpan ?? DefaultCacheLifeSpan;
            var objectToAdd = factory();
            return (T) (cache.AddOrGetExisting(key, objectToAdd, DateTimeOffset.Now + resolvedLifeSpan) ?? objectToAdd);
        }

        /// <inheritdoc />
        public void Set<T>(string key, Func<T> factory, TimeSpan? lifeSpan)
        {
            var resolvedLifeSpan = lifeSpan ?? DefaultCacheLifeSpan;
            cache.Set(key, factory(), DateTimeOffset.Now + resolvedLifeSpan);
        }

        /// <inheritdoc />
        public T Get<T>(string key)
        {
            return cache.Get(key) is T entry ? entry : default;
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
    }
}
