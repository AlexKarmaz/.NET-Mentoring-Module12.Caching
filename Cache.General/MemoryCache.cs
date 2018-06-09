using System;
using System.Runtime.Caching;

namespace Cache.General
{
    public class MemoryCache<T> : ICache<T>
    {
        private readonly ObjectCache cache = MemoryCache.Default;
        private readonly string prefix;

        public MemoryCache(string prefix)
        {
            this.prefix = prefix;
        }

        public T Get(string key)
        {
            var fromCache = cache.Get(prefix + key);
            if (fromCache == null)
            {
                return default(T);
            }

            return (T)fromCache;
        }

        public void Set(string key, T value, DateTimeOffset expirationDate)
        {
            cache.Set(prefix + key, value, expirationDate);
        }

        public void Set(string key, T value, CacheItemPolicy policy)
        {
            cache.Set(prefix + key, value, policy);
        }
    }
}
