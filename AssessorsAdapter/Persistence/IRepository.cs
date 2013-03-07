namespace AssessorsAdapter.Persistence
{
    public interface IRepository<TKey, TValue>
    {
        void Save(TKey key, TValue value);
        void Delete(TKey key);
        bool ContainsValue(TValue value);
        bool ContainsKey(TKey key);
    }
}
