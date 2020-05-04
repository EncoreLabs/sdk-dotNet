using System;

namespace EncoreTickets.SDK.Utilities.Cache
{
    /// <summary>
    /// A lazy-loading wrapper over the ICache implementations
    /// </summary>
    public class LazyCacheDecorator : ICache
    {
        private readonly ICache cache;

        /// <summary>
        /// Initializes the instance of the <see cref="LazyCacheDecorator"/> class.
        /// </summary>
        public LazyCacheDecorator(ICache cache)
        {
            this.cache = cache;
        }

        /// <inheritdoc />
        public T AddOrGetExisting<T>(string key, Func<T> factory, TimeSpan? lifeSpan)
        {
            var lazyFactory = (Func<Lazy<T>>)(() => new Lazy<T>(factory));
            var result = cache.AddOrGetExisting(key, lazyFactory, lifeSpan);
            return result.Value;
        }

        /// <inheritdoc />
        public void Set<T>(string key, Func<T> factory, TimeSpan? lifeSpan)
        {
            var lazyFactory = (Func<Lazy<T>>) (() => new Lazy<T>(factory));
            cache.Set(key, lazyFactory, lifeSpan);
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
