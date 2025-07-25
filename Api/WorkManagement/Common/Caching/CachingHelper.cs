using Microsoft.Extensions.Caching.Memory;

namespace WorkManagement.Common
{
    public class CachingHelper : ICachingHelper
    {
        private readonly IMemoryCache _cache;

        public CachingHelper(IMemoryCache cache)
        {
            _cache = cache;
        }

        public T Get<T>(string key) => _cache.TryGetValue(key, out T value) ? value : default;

        public T Get<T>(string key, Func<T> acquire, int? cacheTime = null)
        {
            if (_cache.TryGetValue(key, out T value))
                return value;

            value = acquire();
            _cache.Set(key, value, TimeSpan.FromMinutes(cacheTime ?? 30));
            return value;
        }

        public bool Set<T>(string key, T data, TimeSpan timeSpan = default, bool IsSliding = false)
        {
            _cache.Set(key, data, timeSpan == default ? TimeSpan.FromMinutes(30) : timeSpan);
            return true;
        }

        public bool IsSet(string key) => _cache.TryGetValue(key, out _);

        public void Remove(string key) => _cache.Remove(key);

        public void RemoveByPattern(string pattern) { /* optional */ }

        public void Clear() { /* optional */ }

        public void Dispose() { /* optional */ }
    }

}
