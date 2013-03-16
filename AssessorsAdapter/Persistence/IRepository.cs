namespace AssessorsAdapter.Persistence
{
    public interface IRepository<in TKey, TValue>
    {
        void Save(TKey key, TValue value);
        void Delete(TKey key);
        bool ContainsValue(TValue value);
        bool ContainsKey(TKey key);
        TValue Fetch(TKey key);
        void Empty();
    }
}
