﻿using System;

namespace EncoreTickets.SDK.Utilities.Common.Cache
{
    /// <summary>
    /// A lazy-loading wrapper over the ICache implementations
    /// </summary>
    public class LazyCacheHandler : ICache
    {
        private readonly ICache cache;

        /// <summary>
        /// Initializes the instance of the <see cref="LazyCacheHandler"/> class.
        /// </summary>
        public LazyCacheHandler(ICache cache)
        {
            this.cache = cache;
        }

        /// <inheritdoc />
        public T AddOrGetExisting<T>(string key, Func<T> factory, TimeSpan? lifeSpan)
        {
            factory = factory ?? (() => default);
            var lazyFactory = (Func<Lazy<T>>)(() => new Lazy<T>(factory));
            var result = cache.AddOrGetExisting(key, lazyFactory, lifeSpan);
            return result.Value;
        }

        /// <inheritdoc />
        public void Set<T>(string key, Func<T> factory, TimeSpan? lifeSpan)
        {
            cache.Set(key, () => new Lazy<T>(factory), lifeSpan);
        }

        /// <inheritdoc />
        public T Get<T>(string key)
        {
            var cachedItem = cache.Get<Lazy<T>>(key);
            return cachedItem != null ? cachedItem.Value : default;
        }

        /// <inheritdoc />
        public bool Remove(string key)
        {
            return cache.Remove(key);
        }

        /// <inheritdoc />
        public bool Contains(string key)
        {
            return cache.Contains(key);
        }
    }
}
