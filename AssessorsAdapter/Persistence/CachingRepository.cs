namespace AssessorsAdapter.Persistence
{
    public class CachingRepository<TKey, TValue> : IRepository<TKey, TValue>
    {
        private readonly IRepository<TKey, TValue> _master;
        private readonly IRepository<TKey, TValue> _cache;

        public CachingRepository(IRepository<TKey, TValue> master, IRepository<TKey, TValue> cache)
        {
            _master = master;
            _cache = cache;
        }

        public void Save(TKey key, TValue value)
        {
            _cache.Save(key, value);
            _master.Save(key, value);
        }

        public void Delete(TKey key)
        {
            _cache.Delete(key);
            _master.Delete(key);
        }

        public bool ContainsValue(TValue value)
        {
            return _cache.ContainsValue(value) || _master.ContainsValue(value);
        }

        public bool ContainsKey(TKey key)
        {
            return _cache.ContainsKey(key) || _master.ContainsKey(key);
        }

        public TValue Fetch(TKey key)
        {
            return _cache.ContainsKey(key) ? _cache.Fetch(key) : _master.Fetch(key);
        }

        public void Empty()
        {
            _cache.Empty();
        }
    }
}