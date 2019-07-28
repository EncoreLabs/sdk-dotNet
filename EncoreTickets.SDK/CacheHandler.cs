using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EncoreTickets.SDK
{
    /// <summary>
    /// Cache Handler class
    /// </summary>
    public class CacheHandler
    {

        #region Member Variables

        /// <summary>
        /// cache lock object
        /// </summary>
        private static readonly object CacheLock = new object();

        /// <summary>
        /// Default cache time
        /// </summary>
        private static readonly TimeSpan DefaultCacheTime = TimeSpan.FromHours(4);

        // todo replace
        private static Dictionary<string, object> cache = new Dictionary<string, object>();

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes static members of the <see cref="CacheHandler"/> class.
        /// </summary>
        static CacheHandler()
        {
        }

        #endregion

        #region Events and delegates

        /// <summary>
        /// A method that should be executed if no value is found in the cache.
        /// </summary>
        /// <typeparam name="T">The type of object the cache method returns.</typeparam>
        /// <returns>The object to cache.</returns>
        public delegate T CacheMethod<T>();

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Get an object from cache with the specified key
        /// </summary>
        /// <typeparam name="T">the type of object to get</typeparam>
        /// <param name="key">The key of the object.</param>
        /// <returns></returns>
        public static T Get<T>(string key)
        {
            object cachedObject = null;

            if (!cache.TryGetValue(key, out cachedObject))
            {
                // fire the object not found in cache handler, which can actually "re-load" an object into the cache
                /*
                ObjectNotFoundInCacheEventArgs args = null;
                if (OnObjectNotFoundInCache(key, out args))
                {
                    cachedObject = AddInternal(key, args.Data, null, CacheItemPriority.Default);
                }
                */
            }

            return (cachedObject != null) ? (T)cachedObject : default(T);
        }

        /// <summary>
        /// Get an object from cache with the specified key. If the key is not found <paramref name="cacheMethod"/> is called and its value cached and returned.
        /// </summary>
        /// <typeparam name="T">the type of object to get</typeparam>
        /// <param name="cacheMethod">The method to execute if a value is not found in the cache.</param>
        /// <param name="key">The key of the object.</param>
        /// <param name="dependentKeys">The dependent keys.</param>
        /// <returns></returns>
        public static T Get<T>(CacheMethod<T> cacheMethod, string key)
        {
            T cachedObject = Get<T>(key);

            if (cachedObject == null)
            {
                lock (string.Intern(key))
                {
                    cachedObject = Get<T>(key);

                    if (cachedObject == null)
                    {
                        cachedObject = cacheMethod();

                        if (cachedObject != null)
                        {
                            Add(cachedObject, key, null);
                        }
                    }
                }
            }

            return cachedObject;
        }

        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="data">The data to add.</param>
        /// <param name="cacheKey">The cache key.</param>
        /// <param name="timeOut">The time out.</param>
        /// <param name="cacheItemPriority">The cache item priority.</param>
        /// <returns></returns>
        public static bool Add(object data, string cacheKey, TimeSpan? timeOut)
        {
            cache.Add(cacheKey, data);

            return true;
        }

        /// <summary>
        /// Determines if an object exists in cache
        /// </summary>
        /// <param name="key">The key of the object to check exists.</param>
        /// <returns></returns>
        public static bool Exists(string key)
        {
            return cache.Keys.Contains(key);
        }


        #endregion

        #region Internal Methods

        /// <summary>
        /// Removes the keys from the cache.
        /// </summary>
        /// <param name="synchronize">If the cache removal should be synchronized across web servers.</param>
        /// <param name="keys">The keys.</param>
        public static void Delete(bool synchronize, params string[] keys)
        {
            foreach (string key in keys)
            {
                if (key != null)
                {
                    cache.Remove(key);
                }
            }
        }


        #endregion

        #endregion

        #region Private Methods

        /// <summary>
        /// Called when an object is removed from cache
        /// </summary>
        /// <param name="key">The key of the object to remove.</param>
        /// <param name="data">The data to remove.</param>
        private static void OnObjectRemovedFromCache(string key, object data)
        {
            /*// raise the event, if necessary
            if (ObjectRemovedFromCache != null)
            {
                ObjectRemovedFromCache(null, new CacheEventArgs(key, data));
            }*/
        }

        /// <summary>
        /// Called when an object is added to the cache
        /// </summary>
        /// <param name="key">The key of the object.</param>
        /// <param name="data">The data added.</param>
        private static void OnObjectAddedToCache(string key, object data)
        {
            /*
            if (ObjectAddedToCache != null)
            {
                ObjectAddedToCache(null, new CacheEventArgs(key, data));
            }*/
        }

        #endregion

    }

}
