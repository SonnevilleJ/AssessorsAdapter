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
        }

        public void Delete(TKey key)
        {
            throw new System.NotImplementedException();
        }

        public bool ContainsValue(TValue value)
        {
            throw new System.NotImplementedException();
        }

        public bool ContainsKey(TKey key)
        {
            var cacheContains = _cache.ContainsKey(key);
            return cacheContains || _master.ContainsKey(key);
        }
    }
}