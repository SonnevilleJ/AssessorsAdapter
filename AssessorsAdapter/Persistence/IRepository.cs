namespace AssessorsAdapter.Persistence
{
    public interface IRepository<in TKey, in TValue>
    {
        void Save(TKey key, TValue value);
        void Delete(TKey key);
        bool ContainsValue(TValue value);
        bool ContainsKey(TKey key);
    }
}
