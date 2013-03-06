using System.Collections.Generic;
using Persistence;

namespace PersistenceTest
{
    public class MockRepository<T> : IRepository<T>
    {
        private readonly List<T> _repo = new List<T>();

        public void Save(T obj)
        {
            _repo.Add(obj);
        }

        public bool Contains(T obj)
        {
            return _repo.Contains(obj);
        }

        public void Delete(T obj)
        {
            _repo.Remove(obj);
        }
    }
}