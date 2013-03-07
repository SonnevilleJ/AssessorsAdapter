using System.Collections.Generic;
using AssessorsAdapter.Persistence;

namespace AssessorsAdapterTest.Persistence
{
    public class MockRepository<TKey, TValue> : IRepository<TKey, TValue>
    {
        private readonly Dictionary<TKey, TValue> _repo = new Dictionary<TKey, TValue>();

        public void Save(TKey key, TValue value)
        {
            _repo.Add(key, value);
        }

        public bool Contains(TKey key, TValue value)
        {
            TValue val;
            var success = _repo.TryGetValue(key, out val);
            return success && val.Equals(value);
        }

        public void Delete(TKey key)
        {
            _repo.Remove(key);
        }

        public TValue Get(TKey key)
        {
            if (!_repo.ContainsKey(key)) throw new KeyNotFoundException();
            return _repo[key];
        }
    }
}