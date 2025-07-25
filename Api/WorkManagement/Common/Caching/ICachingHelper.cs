namespace WorkManagement.Common
{
    public interface ICachingHelper : IDisposable
    {
        T Get<T>(string key);

        T Get<T>(string key, Func<T> acquire, int? cacheTime = null);

        bool Set<T>(string key, T data, TimeSpan timeSpan = default(TimeSpan), bool IsSliding = false);

        bool IsSet(string key);

        void Remove(string key);

        void RemoveByPattern(string pattern);

        void Clear();
    }
}
